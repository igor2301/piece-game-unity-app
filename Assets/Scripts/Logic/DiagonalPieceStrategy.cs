using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DiagonalPieceStrategy : PieceStrategy
{
    public override List<Cell> GetAvailableCells(Cell origin)
    {
        List<Cell> cells = new List<Cell>();
        int[] dir = new int[2] { -1, 1 };
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
            {
                int dx = 1;
                int dy = 1;

                Cell cell = board[origin.X + dx * dir[i], origin.Y + dy * dir[j]];

                int occupiedCounter = 0;

                while (cell != null)
                {
                    if (occupiedCounter > 1)
                        break;

                    if (board.GetPiece(cell) != null)
                    {
                        dx++;
                        dy++;

                        cell = board[origin.X + dir[i] * dx, origin.Y + dir[j] * dy];

                        occupiedCounter++;

                        continue;
                    }
                    else
                    {
                        dx++;
                        dy++;

                        cells.Add(cell);

                        cell = board[origin.X + dir[i] * dx, origin.Y + dir[j] * dy];

                        occupiedCounter = 0;
                    }
                }
            }

        return cells;
    }
}
