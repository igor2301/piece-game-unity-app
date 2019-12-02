using UnityEditor;
using UnityEngine;
using System.IO;

public class ExportScript : MonoBehaviour
{
    [MenuItem("Export/Создать данные о префабах")]
    public static int GenerateFormAndFramesPaths()
    {
        PrefabData prefabData = new PrefabData();


        string[] allFormFiles = Directory.GetFiles(Application.dataPath + "/Resources/Gui/Forms/", "*.prefab", SearchOption.AllDirectories);
        for (int i = 0; i < allFormFiles.Length; i++)
        {
            string resultName = allFormFiles[i];

            string relativePath = "";

            int index = allFormFiles[i].LastIndexOf("/");
            if (index != -1)
            {
                resultName = resultName.Substring(index + 1);
                relativePath = "Gui/Forms/" + resultName.Replace("\\", "/").Replace(".prefab", "");
            }

            index = resultName.LastIndexOf(Path.DirectorySeparatorChar);
            if (index != -1)
                resultName = resultName.Substring(index + 1);
            resultName = resultName.Replace(".prefab", "");

            prefabData.formPaths.Add(new FileItem() { prefabName = resultName, path = relativePath });
        }


        string[] allFrameFiles = Directory.GetFiles(Application.dataPath + "/Resources/Gui/Frames/", "*.prefab", SearchOption.AllDirectories);
        for (int i = 0; i < allFrameFiles.Length; i++)
        {
            string resultName = allFrameFiles[i];

            string relativePath = "";

            int index = allFrameFiles[i].LastIndexOf("/");
            if (index != -1)
            {
                resultName = resultName.Substring(index + 1);
                relativePath = "Gui/Frames/" + resultName.Replace("\\", "/").Replace(".prefab", "");
            }

            index = resultName.LastIndexOf(Path.DirectorySeparatorChar);
            if (index != -1)
                resultName = resultName.Substring(index + 1);
            resultName = resultName.Replace(".prefab", "");

            prefabData.framePaths.Add(new FileItem() { prefabName = resultName, path = relativePath });
        }


        string resultJson = JsonUtility.ToJson(prefabData);
        File.WriteAllText(Application.dataPath + "/Resources/PrefabData.json", resultJson);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return 0;
    }
}
