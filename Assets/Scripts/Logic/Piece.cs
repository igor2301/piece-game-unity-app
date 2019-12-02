using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Piece
{
    public readonly bool IsPlayer;

    public float realX;
    public float realY;

    public Action<float, float> OnMove = null;

    public Cell cellRef;

    public Piece(bool isPlayer)
    {
        IsPlayer = isPlayer;

        cellRef = null;
    }

    public void SetPosition(float x, float y)
    {
        realX = x;
        realY = y;

        OnMove?.Invoke(realX, realY);
    }
}
