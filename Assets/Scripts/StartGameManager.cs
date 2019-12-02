using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    private GameObject mainCanvas;

    private void Start()
    {
        mainCanvas = GameObject.Find("MainCanvas");

        FormManager.Instance.Initialize(mainCanvas);
        FormManager.Instance.OpenForm<StartForm>();
    }
}
