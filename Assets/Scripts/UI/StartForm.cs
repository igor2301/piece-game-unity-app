using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartForm : GuiForm
{
    private StartFormView view = null;

    public override void Init(GameObject formRoot)
    {
        base.Init(formRoot);

        view = GetView<StartFormView>();

        view.btnPlayModeDialognal.onClick.AddListener(() => StartGame<DiagonalPieceStrategy>());
        view.btnPlayModeStraight.onClick.AddListener(() => StartGame<StraightPieceStrategy>());
        view.btnPlayModeOneStep.onClick.AddListener(() => StartGame<OneStepPieceStrategy>());
    }

    private void StartGame<T>() where T : PieceStrategy, new()
    {
        Board board = new Board(8, 8);
        T strategy = new T();
        strategy.SetBoard(board);

        FormManager.Instance.Get<MainForm>().SetData(board, strategy);
        FormManager.Instance.OpenForm<MainForm>();
    }
}
