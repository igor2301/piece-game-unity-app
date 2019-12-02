using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineExecutor : MonoBehaviour
{
    private static CoroutineExecutor instance = null;
    public static CoroutineExecutor Instance
    {
        get
        {
            if (instance == null)
            {
                Type type = typeof(CoroutineExecutor);
                instance = GameObject.FindObjectOfType(type) as CoroutineExecutor;

                if (instance == null)
                {
                    Debug.Log("No instance of " + type.ToString() + ", a temporary one is created.");
                    instance = new GameObject("Temp Instance of " + type.ToString(), type).GetComponent<CoroutineExecutor>();
                }
                instance.Init();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as CoroutineExecutor;
            instance.Init();
        }
        else
        {
            //DestroyImmediate (this);
        }
    }

    protected virtual void Init()
    {
    }

    private void OnApplicationQuit()
    {
        instance = null;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public UnityEngine.Object Execute(IEnumerator execMethod)
    {
        StartCoroutine(execMethod);
        return null;
    }
}
