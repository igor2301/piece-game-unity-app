using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PieceFrame : GuiFrame
{
    private PieceViewFrame view = null;

    private Piece data = null;
    public Piece Data
    {
        get { return data; }
        set
        {
            data = value;
            data.OnMove = OnMove;

            UpdateData();
        }
    }

    private void OnMove(float x, float y)
    {
        SetPosition(new Vector2(x, y));
    }

    protected override void InitFormControls()
    {
        base.InitFormControls();

        view = GetView<PieceViewFrame>();
    }

    private void UpdateData()
    {
        view.Player.gameObject.SetActive(data.IsPlayer);
        view.Enemy.gameObject.SetActive(!data.IsPlayer);
    }
}
