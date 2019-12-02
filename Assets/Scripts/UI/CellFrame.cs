using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CellFrame : GuiFrame
{
    private CellViewFrame view = null;
    private Color initialColor;

    public event Action<CellFrame> OnClicked = null;

    private Cell data;
    public Cell Data
    {
        get { return data; }
        set
        {
            data = value;
            data.OnHighlighted = TurnGreen;
            data.OnSelected = TurnRed;
        }
    }

    private void TurnGreen(bool enable)
    {
        view.Image.color = enable ? Color.green : initialColor;
    }

    private void TurnRed(bool enable)
    {
        view.Image.color = enable ? Color.red : initialColor;
    }

    protected override void InitFormControls()
    {
        base.InitFormControls();

        view = GetView<CellViewFrame>();

        view.Button.onClick.AddListener(() => OnClicked?.Invoke(this));

        initialColor = view.Image.color;
    }

    public void DrawCoord(bool isCoord, string value)
    {
        view.Image.enabled = !isCoord;
        view.Text.text = value;
    }

    public void Select(bool isSelected)
    {
        view.Image.color = isSelected ? Color.red : initialColor;
    }
}
