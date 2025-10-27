#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseHoldItem), true)]
public class BaseHoldItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BaseHoldItem item = (BaseHoldItem)target;

        string assetPath = AssetDatabase.GetAssetPath(item);
        if (string.IsNullOrEmpty(assetPath))
            return;

        int resourcesIndex = assetPath.IndexOf("/Resources/");
        if (resourcesIndex == -1)
            return;

        string resoursecPath = assetPath.Substring(resourcesIndex + 11); // 11 = длина "/Resources/"
        resoursecPath = resoursecPath.Replace(".prefab", "");

        if (item.PrefabPath != resoursecPath)
        {
            item.PrefabPath = resoursecPath;
            EditorUtility.SetDirty(item);
        }

        //запрет на изменение пути вручную
        GUI.enabled = false;
        EditorGUILayout.TextField("PrefabPath (auto) : ", item.PrefabPath);
        GUI.enabled = true;
    }
}
#endif
