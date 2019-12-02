using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Базовый класс для формы
/// </summary>
public abstract class GuiForm
{
    protected List<GuiFrame> attachedFrames = new List<GuiFrame>();

    public virtual string PrefabName
    {
        get
        {
            return GetType().Name;
        }
    }

    private GameObject Root
    {
        get;
        set;
    }

    public bool IsOpen
    {
        get;
        private set;
    }

    public virtual void Init(GameObject formRoot)
    {
        Root = formRoot;
        IsOpen = false;
    }

    public virtual void Dispose()
    {
        Root = null;
    }

    public virtual void Show(bool playSound)
    {
        if (Root != null)
        {
            IsOpen = true;
            Root.SetActive(true);
        }
    }

    public virtual void Hide()
    {
        if (Root != null)
        {
            IsOpen = false;
            Root.SetActive(false);
        }
    }

    protected T GetView<T>()
    {
        return Root.GetComponent<T>();
    }

    public T AddFrame<T>(RectTransform parentTransform) where T : GuiFrame, new()
    {
        T newFrame = new T();
        newFrame.parentForm = this;
        newFrame.SetItemFrame(parentTransform);
        attachedFrames.Add(newFrame);

        return newFrame;
    }

    public void RemoveFrame(GuiFrame removedFrame)
    {
        attachedFrames.Remove(removedFrame);
        removedFrame.Dispose();
    }
}
