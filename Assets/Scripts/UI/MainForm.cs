using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Основной интерфейс приложения
/// </summary>
public class MainForm : GuiForm
{
    private MainFormView view = null;
    private PieceStrategy strategy = null;
    private Board board = null;

    private List<CellFrame> cellFrames = new List<CellFrame>();
    private List<PieceFrame> pieceFrames = new List<PieceFrame>();

    private DecisionMaker ai = new DecisionMaker();

    private Cell destination;

    public override void Init(GameObject formRoot)
    {
        base.Init(formRoot);

        view = GetView<MainFormView>();

        ai.OnDecisionMade += AiOnDecisionMade;
    }

    private void AiOnDecisionMade(Piece piece, List<Cell> path)
    {
        if (coroutine != null)
            CoroutineExecutor.Instance.StopCoroutine(coroutine);

        destination = path[path.Count - 1];

        coroutine = CoroutineExecutor.Instance.StartCoroutine(MovePieceAI(piece, piece.cellRef, path));
    }

    public void SetData(Board board, PieceStrategy strategy)
    {
        this.board = board;
        this.strategy = strategy;

        ai.Update(board, strategy);
    }

    /// <summary>
    /// Показать форму и создать представление поля
    /// </summary>
    /// <param name="playSound"></param>
    public override void Show(bool playSound)
    {
        base.Show(playSound);

        char[] alpha = "abcdefghejklmnopqrstuvwxyz".ToCharArray();

        RectTransform content = view.Board.GetComponent<RectTransform>();
        for (int i = 0; i < board.Rows + 2; i++)
            for (int j = 0; j < board.Cols + 2; j++)
            {
                bool isCoord = i == 0 || j == 0 || i == board.Rows + 1 || j == board.Cols + 1;

                CellFrame frame = AddFrame<CellFrame>(content);
                cellFrames.Add(frame);

                if (isCoord)
                {
                    string value = "";

                    if (i == 0 || i == board.Rows + 1)
                        value = (j == 0 || j == board.Cols + 1) ? "" : alpha[j - 1].ToString();
                    if (j == 0 || j == board.Cols + 1)
                        value = (i == 0 || i == board.Rows + 1) ? "" : (board.Rows - i + 1).ToString();

                    frame.DrawCoord(isCoord, value);
                }
                else
                {
                    frame.OnClicked += OnClickCell;
                    frame.Data = board[i - 1, j - 1];
                }
            }

        CreateTeam(0, 0, false);
        CreateTeam(5, 5, true);

        UpdateTurn();

        board.OnWin += OnWin;
    }

    private ScoreFrame scoreFrame;

    private void OnWin(bool hasPlayerWon)
    {
        scoreFrame = AddFrame<ScoreFrame>(view.GetComponent<RectTransform>());
        scoreFrame.OnClosed += OnClosed;
        scoreFrame.Data = new ScoreData() { hasPlayerWon = hasPlayerWon };
    }

    private void OnClosed()
    {
        FormManager.Instance.OpenForm<StartForm>();
    }

    public override void Hide()
    {
        foreach (CellFrame cell in cellFrames)
            RemoveFrame(cell);

        foreach (PieceFrame piece in pieceFrames)
            RemoveFrame(piece);

        if (scoreFrame != null)
            RemoveFrame(scoreFrame);

        base.Hide();
    }

    private void UpdateTurn()
    {
        view.TurnText.text = board.IsPlayerTurn ? "Ваш ход" : "Ход противника";
    }

    private Coroutine coroutine;
    private Piece activePiece;

    /// <summary>
    /// Кликаем на ячейку
    /// </summary>
    private void OnClickCell(CellFrame originUI)
    {
        if (!board.IsPlayerTurn || destination != null)
            return;

        Piece piece  = board.GetPiece(originUI.Data);
        if (piece != null)
        {
            if (!(piece.IsPlayer ^ board.IsPlayerTurn))
            {
                if (coroutine != null)
                    CoroutineExecutor.Instance.StopCoroutine(coroutine);

                coroutine = CoroutineExecutor.Instance.StartCoroutine(MovePiece(piece, originUI.Data));
            }
        }
        else if (activePiece != null)
        {
            destination = originUI.Data;
        }
    }

    /// <summary>
    /// Перемещать фигуру AI
    /// </summary>
    private IEnumerator MovePieceAI(Piece piece, Cell origin, List<Cell> path)
    {
        activePiece = piece;

        SmoothMoveStrategy moveStrategy = new SmoothMoveStrategy(piece, board, path);
        foreach (Vector2 pos in moveStrategy.Move())
        {
            Vector2 realPos = ConvertToBoard(pos.x, pos.y);
            piece.SetPosition(realPos.x, realPos.y);

            if (moveStrategy.IsCompleted)
            {
                board.CompleteStep(strategy);
                UpdateTurn();

                destination = null;
                activePiece = null;

                if (!board.IsPlayerTurn)
                {
                    ai.MakeDecision();
                }

                yield break;
            }

            yield return moveStrategy.Delay();
        }
    }

    /// <summary>
    /// Перемещаем фигуру игрока
    /// </summary>
    private IEnumerator MovePiece(Piece piece, Cell origin)
    {
        activePiece = piece;

        foreach (var cell in cellFrames)
            cell.Select(cell.Data == origin);

        List<Cell> availableCells = strategy.GetAvailableCells(origin);

        if (availableCells.Count == 0)
        {
            foreach (var cell in cellFrames)
                cell.Select(false);

            destination = null;
            activePiece = null;

            yield break;
        }

        foreach (Cell cell in availableCells)
            cell.Highlight(true);

        // Ждем указания конечной точки
        while (destination == null)
            yield return null;

        foreach (Cell cell in availableCells)
            cell.Highlight(false);

        foreach (var cell in cellFrames)
            cell.Select(false);

        if (!availableCells.Contains(destination))
        {
            destination = null;
            activePiece = null;

            yield break;
        }
        else
        {
            List<Cell> path = strategy.ComputePath(origin, destination);

            SmoothMoveStrategy moveStrategy = new SmoothMoveStrategy(piece, board, path);
            foreach (Vector2 pos in moveStrategy.Move())
            {
                Vector2 realPos = ConvertToBoard(pos.x, pos.y);
                piece.SetPosition(realPos.x, realPos.y);

                if (moveStrategy.IsCompleted)
                {
                    bool isGameFinished = board.CompleteStep(strategy);
                    UpdateTurn();

                    destination = null;
                    activePiece = null;

                    if (!isGameFinished && !board.IsPlayerTurn)
                    {
                        ai.MakeDecision();
                    }

                    yield break;
                }

                yield return moveStrategy.Delay();
            }
        }
    }

    /// <summary>
    /// Создание группы фигур
    /// </summary>
    private void CreateTeam(int x, int y, bool isPlayer)
    {
        for (int i = x; i < x + 3; i++)
            for (int j = y; j < y + 3; j++)
                AddPiece(i, j, isPlayer);
    }

    /// <summary>
    /// Добавить фигуру
    /// </summary>
    private void AddPiece(int x, int y, bool isPlayer)
    {
        Piece piece = new Piece(isPlayer);

        PieceFrame frame = AddFrame<PieceFrame>(view.PieceHolder);
        frame.Data = piece;
        pieceFrames.Add(frame);

        board.AddPiece(piece, x, y);

        Vector2 realPos = ConvertToBoard(x, y);
        piece.SetPosition(realPos.x, realPos.y);
    }

    /// <summary>
    /// Переводим координаты в простраство сцены
    /// </summary>
    private Vector2 ConvertToBoard(float x, float y)
    {
        int boardWidth = (int)(view.Board.cellSize.x * board.Cols + (view.Board.spacing.x - 1) * board.Cols);
        int boardHeight = (int)(view.Board.cellSize.y * board.Rows + (view.Board.spacing.y - 1) * board.Rows);

        float realX = ((y + 0.5f) * (view.Board.cellSize.x + view.Board.spacing.x) - boardWidth / 2);
        float realY = boardHeight / 2 - (x + 0.5f) * (view.Board.cellSize.y + view.Board.spacing.y);

        return new Vector2(realX, realY);
    }
}
