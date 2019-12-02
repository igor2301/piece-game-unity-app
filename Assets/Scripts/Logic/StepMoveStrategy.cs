using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StepMoveStrategy : IMoveStrategy
{
    private Piece piece;
    private Board board;
    private List<Cell> path;
    private bool isCompleted;

    public StepMoveStrategy(Piece piece, Board board, List<Cell> path)
    {
        this.piece = piece;
        this.board = board;
        this.path = path;
    }

    public bool IsCompleted
    {
        get
        {
            return isCompleted;
        }
    }

    public IEnumerable Move()
    {
        board.SetPiece(piece, path[path.Count - 1].X, path[path.Count - 1].Y);

        for (int i = 0; i < path.Count; i++)
        {
            Cell cell = path[i];

            if (cell == path[path.Count - 1])
            {
                isCompleted = true;
            }

            yield return new Vector2(cell.X, cell.Y);
        }
    }

    public YieldInstruction Delay()
    {
        return new WaitForSeconds(0.3f);
    }
}
