using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ScoreData
{
    public bool hasPlayerWon;
    public bool canMove;
}

public class ScoreFrame : GuiFrame
{
    private ScoreViewFrame view = null;

    public event Action OnClosed = null;

    protected override void InitFormControls()
    {
        base.InitFormControls();

        view = GetView<ScoreViewFrame>();

        view.menuButton.onClick.AddListener(() =>
        {
            OnClosed?.Invoke();
        });
    }

    private ScoreData data;
    public ScoreData Data
    {
        get { return data; }
        set
        {
            data = value;
            UpdateData();
        }
    }

    private void UpdateData()
    {
        view.Text.text = data.hasPlayerWon ? "Победа!" : "Поражение";
        view.Text.color = data.hasPlayerWon ? Color.green : Color.red;
    }
}
