using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GuiFrame
{
    public virtual string PrefabName
    {
        get
        {
            return this.GetType().Name;
        }
    }

    public virtual GameObject Root
    {
        get;
        protected set;
    }

    public GuiForm parentForm;

    protected T GetView<T>()
    {
        return Root.GetComponent<T>();
    }

    public bool IsActiveInHeirarchy
    {
        get
        {
            if (Root != null)
            {
                return Root.activeInHierarchy;
            }
            else
                return false;
        }
    }

    public GameObject SetItemFrame(Transform parent)
    {
        Root = FormManager.Instance.GetFrame(PrefabName);

        if (Root != null)
        {
            AttachToHierarchy(parent, Vector3.zero, Vector3.one, Quaternion.identity);
            InitFormControls();

            return Root;
        }

        return null;
    }

    public void AttachToHierarchy(Transform parent, Vector2 position, Vector3 scale, Quaternion rotation)
    {
        RectTransform rectTransf = Root.GetComponent<RectTransform>();
        rectTransf.SetParent(parent, false);
        rectTransf.localScale = scale;
        rectTransf.anchoredPosition = position;
        rectTransf.rotation = rotation;
    }

    public void SetPosition(Vector2 position)
    {
        RectTransform rectTransf = Root.GetComponent<RectTransform>();
        rectTransf.anchoredPosition = position;
    }

    protected virtual void InitFormControls()
    {
    }

    public virtual void Dispose()
    {
        GameObject.Destroy(Root);
        Root = null;
    }
}
