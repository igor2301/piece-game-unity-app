using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OneStepPieceStrategy : PieceStrategy
{
    public override List<Cell> GetAvailableCells(Cell origin)
    {
        List<Cell> cells = new List<Cell>();
        int[] dir = new int[3] { -1, 0, 1 };
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                int dx = 1;
                int dy = 1;

                Cell cell = board[origin.X + dx * dir[i], origin.Y + dy * dir[j]];

                if (cell != null && cell != origin)
                {
                    if (board.GetPiece(cell) == null)
                    {
                        cells.Add(cell);
                    }
                }
            }

        return cells;
    }
}
