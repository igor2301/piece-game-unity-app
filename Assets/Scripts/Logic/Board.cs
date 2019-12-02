using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Board
{
    private Cell[,] cells;

    private List<Piece> pieces = new List<Piece>();

    public bool IsPlayerTurn = true;

    public int Rows
    {
        get
        {
            return cells.GetLength(0);
        }
    }

    public int Cols
    {
        get
        {
            return cells.GetLength(1);
        }
    }

    public Board(int row, int col)
    {
        cells = new Cell[row, col];
        
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                cells[i, j] = new Cell(i, j);
            }
    }

    public Cell this[int x, int y]
    {
        get
        {
            if (x < 0 || y < 0 || x >= Rows || y >= Cols)
                return null;

            return cells[x, y];
        }
    }

    public void AddPiece(Piece piece, int x, int y)
    {
        pieces.Add(piece);

        SetPiece(piece, x, y);
    }

    public void SetPiece(Piece piece, int x, int y)
    {
        if (piece == null)
            return;

        Cell cell = cells[x, y];
        piece.cellRef = cell;
    }

    public Piece GetPiece(Cell cell)
    {
        foreach (Piece piece in pieces)
            if (piece.cellRef == cell)
                return piece;

        return null;
    }

    public bool IsInEnemyCamp(Cell cell)
    {
        return cell.X >= 0 && cell.X <= 2 && cell.Y >= 0 && cell.Y <= 2;
    }

    public bool IsInPlayerCamp(Cell cell)
    {
        return cell.X >= 5 && cell.X <= 7 && cell.Y >= 5 && cell.Y <= 7;
    }

    public event Action<bool> OnWin = null;

    /// <summary>
    /// Завершаем шаг и проверяем выиграла ли одна из команд
    /// Команда выигрывает если все ее фигуры, которые могут двигаться 
    /// достигли лагеря противоположенной команды
    /// </summary>
    public bool CompleteStep(PieceStrategy strategy)
    {
        bool hasAllPlayerPiecesReachedEnemyCamp = true;
        bool hasAllEnemyPiecesReachedPlayerCamp = true;

        bool playerIsBlocked = true;
        bool enemyIsBlocked = true;

        foreach (Piece piece in pieces)
        {
            // Проверям может ли двигаться фигура
            List<Cell> availableCells = strategy.GetAvailableCells(piece.cellRef);
            
            if (piece.IsPlayer)
            {
                bool isInEnemyCamp = IsInEnemyCamp(piece.cellRef);
                if (!isInEnemyCamp)
                {
                    hasAllPlayerPiecesReachedEnemyCamp = false;
                }

                if (!isInEnemyCamp && availableCells.Count > 0)
                {
                    playerIsBlocked = false;
                }
            }
            else
            {
                bool isInPlayerCamp = IsInPlayerCamp(piece.cellRef);
                if (!isInPlayerCamp)
                {
                    hasAllEnemyPiecesReachedPlayerCamp = false;
                }

                if (availableCells.Count > 0)
                {
                    enemyIsBlocked = false;
                }
            }
        }

        bool hasPlayerWon = hasAllPlayerPiecesReachedEnemyCamp || playerIsBlocked;
        bool hasEnemyWon = hasAllEnemyPiecesReachedPlayerCamp || enemyIsBlocked;

        // Все фишки игрока достигли вражеского лагеря либо заблокированы противником
        if (hasPlayerWon)
            OnWin?.Invoke(true);

        // Все фишки противника достигли лагеря игрока либо заблокированы игроком
        if (hasEnemyWon)
            OnWin?.Invoke(false);

        IsPlayerTurn = !IsPlayerTurn;

        return hasPlayerWon || hasEnemyWon;
    }

    public List<Piece> GetPieces() { return pieces; }
}
