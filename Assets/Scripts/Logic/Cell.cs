using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cell
{
    public readonly int X;
    public readonly int Y;

    public Action<bool> OnHighlighted = null;
    public Action<bool> OnSelected = null;

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Highlight(bool enable)
    {
        OnHighlighted?.Invoke(enable);
    }

    public void Select(bool enable)
    {
        OnSelected?.Invoke(enable);
    }
}
