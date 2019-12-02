using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DecisionMaker
{
    private PieceStrategy strategy;
    private Board board;

    private Random random = new Random();
    private Piece lastPiece;

    public event Action<Piece, List<Cell>> OnDecisionMade = null;

    public void Update(Board board, PieceStrategy strategy)
    {
        this.strategy = strategy;
        this.board = board;
    }

    public void MakeDecision()
    {
        List<Piece> movablePieces = new List<Piece>();
        List<Piece> piecesInCamp = new List<Piece>();
        List<Piece> aiPieces = new List<Piece>();

        List<Piece> allPieces = board.GetPieces();
        foreach (Piece piece in allPieces)
        {
            if (!piece.IsPlayer)
            {
                aiPieces.Add(piece);

                if (!board.IsInPlayerCamp(piece.cellRef))
                    movablePieces.Add(piece);
                else
                    piecesInCamp.Add(piece);
            }
        }

        Piece targetPiece = null;
        Cell destination = null;

        float targetDistance = int.MaxValue;

        // Ищем среди фишек не в лагере игрока
        for (int i = 0; i < movablePieces.Count; i++)
        {
            Cell originCell = movablePieces[i].cellRef;

            float originDistance = Math.Abs(originCell.X - board.Cols) + Math.Abs(originCell.Y - board.Rows);
            List<Cell> cells = strategy.GetAvailableCells(movablePieces[i].cellRef);
            for (int j = 0; j < cells.Count; j++)
            {
                // попали в лагерь игрока. Ход удачный
                if (board.IsInPlayerCamp( cells[j]))
                {
                    targetPiece = movablePieces[i];
                    destination = cells[j];
                    targetDistance = 0f;
                    break;
                }

                // Пытаемся приблизиться
                float distance = Math.Abs(cells[j].X - board.Cols) + Math.Abs(cells[j].Y - board.Rows);
                if (distance > originDistance)
                {
                    continue;
                }

                if (distance < targetDistance)
                {
                    targetDistance = distance;
                    targetPiece = movablePieces[i];
                    destination = cells[j];
                }
            }

            // Можем попасть в лагерь. Прекращаем поиск
            if (targetDistance == 0)
            {
                break;
            }
        }

        // Не нашли фишку мне лагеря игрока, ходим фишкой которая уже в лагере
        if (targetPiece == lastPiece || targetDistance > int.MaxValue / 2)
        {
            List<Cell> cells = null;
            for (int i = 0; i < piecesInCamp.Count; i++)
            {
                Cell originCell = piecesInCamp[i].cellRef;

                float originDistance = Math.Abs(originCell.X - board.Cols) + Math.Abs(originCell.Y - board.Rows);

                cells = strategy.GetAvailableCells(piecesInCamp[i].cellRef);

                for (int j = 0; j < cells.Count; j++)
                {
                    // Пытаемся приблизиться
                    float distance = Math.Abs(cells[j].X - board.Cols) + Math.Abs(cells[j].Y - board.Rows);
                    if (distance > originDistance)
                    {
                        continue;
                    }

                    if (distance < targetDistance)
                    {
                        targetDistance = distance;
                        targetPiece = piecesInCamp[i];
                        destination = cells[j];
                    }
                }
            }

            // Делаем рандомный ход
            if (targetDistance > int.MaxValue / 2)
            {
                do
                {
                    targetPiece = aiPieces[random.Next() % aiPieces.Count];
                    cells = strategy.GetAvailableCells(targetPiece.cellRef);
                }
                while (cells.Count == 0);

                destination = cells[random.Next() % cells.Count];
            }
        }

        lastPiece = targetPiece;

        List<Cell> path = strategy.ComputePath(targetPiece.cellRef, destination);

        OnDecisionMade?.Invoke(targetPiece, path);
    }
}
