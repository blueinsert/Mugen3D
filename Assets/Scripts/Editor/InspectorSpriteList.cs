using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteList))]  
public class InspectorSpriteList : Editor {
    private WindowSpriteList m_curWindow;
    private SpriteList m_target;
    public override void OnInspectorGUI()
    {
        if(m_target == null)
            m_target = (SpriteList)serializedObject.targetObject;
        GUILayout.Label("NUM:" + m_target.GetSpriteNum());
        if (GUILayout.Button("edit"))
        {
            // Get existing open window or if none, make a new one:
            WindowSpriteList window = (WindowSpriteList)EditorWindow.GetWindow(typeof(WindowSpriteList));
            if (window != m_curWindow)
            {
                m_curWindow = window;
                m_curWindow.SetTarget(m_target);
                m_curWindow.onSave += OnSave;
                m_curWindow.Show();
                Debug.Log("new window");
            }      
        }
    }

    private void OnSave()
    {
        var go = m_target.gameObject;
        var prefabGo = GetPrefabInstanceParent(go);
        var prefabAsset = PrefabUtility.GetPrefabParent(prefabGo);
        PrefabUtility.ReplacePrefab(prefabGo, prefabAsset, ReplacePrefabOptions.ConnectToPrefab);
        AssetDatabase.SaveAssets();
        Debug.Log("save "+prefabGo.name+" to " + prefabAsset.name);
    }

    //遍历获取prefab节点所在的根prefab节点
    static GameObject GetPrefabInstanceParent(GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        PrefabType pType = EditorUtility.GetPrefabType(go);
        if (pType != PrefabType.PrefabInstance)
        {
            return null;
        }
        if (go.transform.parent == null)
        {
            return go;
        }
        pType = EditorUtility.GetPrefabType(go.transform.parent.gameObject);
        if (pType != PrefabType.PrefabInstance)
        {
            return go;
        }
        return GetPrefabInstanceParent(go.transform.parent.gameObject);
    }
}
