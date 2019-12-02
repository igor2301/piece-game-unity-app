using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StraightPieceStrategy : PieceStrategy
{
    private void ScanColumns(List<Cell> cells, Cell origin, int dirY)
    {
        ScanLine(cells, origin, 0, dirY);
    }

    private void ScanRows(List<Cell> cells, Cell origin, int dirX)
    {
        ScanLine(cells, origin, dirX, 0);
    }

    private void ScanLine(List<Cell> cells, Cell origin, int dirX, int dirY)
    {
        int dx = 1;
        int dy = 1;

        Cell cell = board[origin.X + dx * dirX, origin.Y + dy * dirY];

        int occupiedCounter = 0;

        while (cell != null)
        {
            if (occupiedCounter > 1)
                break;

            if (board.GetPiece(cell) != null)
            {
                dx++;
                dy++;

                cell = board[origin.X + dx * dirX, origin.Y + dirY * dy];

                occupiedCounter++;

                continue;
            }
            else
            {
                dx++;
                dy++;

                cells.Add(cell);

                cell = board[origin.X + dx * dirX, origin.Y + dirY * dy];

                occupiedCounter = 0;
            }
        }
    }

    public override List<Cell> GetAvailableCells(Cell origin)
    {
        List<Cell> cells = new List<Cell>();
        int[] dir = new int[2] { 1, -1 };
        for (int j = 0; j < 2; j++)
        {
            ScanColumns(cells, origin, dir[j]);

            ScanRows(cells, origin, dir[j]);
        }

        return cells;
    }
}
