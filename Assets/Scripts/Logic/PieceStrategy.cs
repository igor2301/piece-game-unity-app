using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class PieceStrategy
{
    protected Board board;

    public void SetBoard(Board board)
    {
        this.board = board;
    }

    public List<Cell> ComputePath(Cell origin, Cell destination)
    {
        List<Cell> cells = new List<Cell>();

        int dx = Math.Sign(destination.X - origin.X);
        int dy = Math.Sign(destination.Y - origin.Y);

        Cell cell = board[origin.X, origin.Y];

        int step = 1;
        while (cell != destination)
        {
            cell = board[origin.X + step * dx, origin.Y + step * dy];

            step++;

            cells.Add(cell);
        }

        return cells;
    }

    public abstract List<Cell> GetAvailableCells(Cell origin);
}
