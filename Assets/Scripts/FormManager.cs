using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FileItem
{
    public string prefabName;
    public string path;
}

[Serializable]
public class PrefabData
{
    public List<FileItem> formPaths = new List<FileItem>();
    public List<FileItem> framePaths = new List<FileItem>();
}

/// <summary>
/// Класс для работы с формами
/// </summary>
public class FormManager
{
    private Dictionary<string, string> prefabPaths = new Dictionary<string, string>();
    private Dictionary<string, string> formPaths = new Dictionary<string, string>();

    private Dictionary<Type, GuiForm> forms = new Dictionary<Type, GuiForm>();
    private Dictionary<string, GameObject> frames = new Dictionary<string, GameObject>();

    private Transform mainCanvas;

    private static FormManager instance = null;
    public static FormManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FormManager();
            }
            return instance;
        }
    }

    public void Initialize(GameObject mainCanvas)
    {
        this.mainCanvas = mainCanvas.transform;
        LoadPrefabPaths();
    }

    /// <summary>
    /// Загружаем данные о префабах форм
    /// </summary>
    private void LoadPrefabPaths()
    {
        prefabPaths.Clear();
        formPaths.Clear();

        TextAsset textAsset = Resources.Load<TextAsset>("PrefabData");
        PrefabData prefabData = JsonUtility.FromJson<PrefabData>(textAsset.text);
        for (int i = 0; i < prefabData.framePaths.Count; i++)
        {
            prefabPaths.Add(prefabData.framePaths[i].prefabName, prefabData.framePaths[i].path);
        }

        for (int i = 0; i < prefabData.formPaths.Count; i++)
        {
            formPaths.Add(prefabData.formPaths[i].prefabName, prefabData.formPaths[i].path);
        }
    }

    public T Get<T>() where T : GuiForm
    {
        foreach (GuiForm f in forms.Values)
        {
            if (f.GetType() == typeof(T))
            {
                return f as T;
            }
        }
        return CreateForm<T>();
    }

    private T CreateForm<T>() where T : GuiForm
    {
        T form = Activator.CreateInstance<T>();
        RegisterForm(form);

        return form;
    }

    /// <summary>
    /// Инстанцируем форму
    /// </summary>
    /// <param name="form"></param>
    private void RegisterForm(GuiForm form)
    {
        GameObject formPrefab = Resources.Load<GameObject>(formPaths[form.PrefabName]);

        GameObject uiObj = UnityEngine.Object.Instantiate(formPrefab, mainCanvas) as GameObject;

        form.Init(uiObj);

        forms.Add(form.GetType(), form);
    }

    public void OpenForm<T>() where T : GuiForm
    {
        OpenForm(typeof(T));
    }

    private GuiForm currentForm = null;

    /// <summary>
    /// Пытаемся получить форму. Если формы еще нет то создаем
    /// </summary>
    public GuiForm Get(Type type)
    {
        GuiForm form = null;
        if (forms.TryGetValue(type, out form))
        {
            return form;
        }
        return CreateForm(type);
    }

    private GuiForm CreateForm(Type formType)
    {
        GuiForm form = (GuiForm)Activator.CreateInstance(formType);
        RegisterForm(form);

        return form;
    }

    public void OpenForm(Type openType)
    {
        if (currentForm != null)
        {
            currentForm.Hide();
        }

        currentForm = Get(openType);

        if (currentForm != null)
        {
            currentForm.Show(true);
        }
    }

    public GameObject GetFrame(string name, Transform parent = null)
    {
        if (!frames.ContainsKey(name))
        {
            if (prefabPaths.ContainsKey(name))
            {
                GameObject framePrefab = Resources.Load<GameObject>(prefabPaths[name]);
                frames.Add(name, framePrefab);
            }
        }

        GameObject uiObj = UnityEngine.Object.Instantiate(frames[name], Vector3.zero, Quaternion.identity) as GameObject;
        return uiObj;
    }
}
