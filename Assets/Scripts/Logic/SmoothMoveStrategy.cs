using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SmoothMoveStrategy : IMoveStrategy
{
    private Piece piece;
    private Board board;
    private List<Cell> path;
    private Cell origin;
    private bool isCompleted;

    public SmoothMoveStrategy(Piece piece, Board board, List<Cell> path)
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
        origin = piece.cellRef;
        float elapsed = 0f;

        board.SetPiece(piece, path[path.Count - 1].X, path[path.Count - 1].Y);

        float realX, realY;
        do
        {
            realX = Mathf.Lerp(origin.X, path[path.Count - 1].X, elapsed);
            realY = Mathf.Lerp(origin.Y, path[path.Count - 1].Y, elapsed);

            elapsed += Time.deltaTime;
         
            if (elapsed >= 1f)
            {
                isCompleted = true;
            }

            yield return new Vector2(realX, realY);

        } while (!isCompleted);
    }

    public YieldInstruction Delay()
    {
        return new WaitForEndOfFrame();
    }
}
