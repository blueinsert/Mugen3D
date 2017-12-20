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
        if (GUILayout.Button("edit"))
        {
            // Get existing open window or if none, make a new one:
            WindowSpriteList window = (WindowSpriteList)EditorWindow.GetWindow(typeof(WindowSpriteList));
            if (window != m_curWindow)
            {
                m_curWindow = window;
                var target = (SpriteList)serializedObject.targetObject;
                m_target = target;
                m_curWindow.SetTarget(target);
                m_curWindow.onSave += OnSave;
                m_curWindow.Show();
                Debug.Log("new window");
            }      
        }
        DrawDefaultInspector();
        //EditorGUI.DrawPreviewTexture(new Rect(0, 0, 400, 400), m_chosenSprite.texture);
    }

    private void OnSave()
    {
        Debug.Log("save");
        Debug.Log(m_target.gameObject);
        //serializedObject.ApplyModifiedProperties();
    }

}
