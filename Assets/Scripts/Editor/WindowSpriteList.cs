using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEditor;

public class WindowSpriteList : EditorWindow
{
    public UnityAction onSave;
    Rect m_rect;
    Rect m_previewRect;
    private Vector2 m_scrollPosition;

    private SpriteList m_target;
    private Object[] m_dragObjects;
    private Vector2 m_mousePosition;
    private Sprite m_chosenSprite;
    private int m_spriteNum;
    private bool[] m_toggleIsOn;

    public void SetTarget(SpriteList spriteList)
    {
        m_target = spriteList;
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        m_spriteNum = m_target.GetSpriteNum() ;
        m_toggleIsOn = new bool[m_spriteNum];
    }

    private void UpdateToggleStatus(int activeIndex)
    {
        for (int i = 0; i < m_toggleIsOn.Length; i++)
        {
            if (i != activeIndex)
            {
                m_toggleIsOn[i] = false;
            }
        }
    }

    void OnGUI()
    {
        List<Sprite> removeList = new List<Sprite>();
        GUILayout.Label("NUM:" + m_target.GetSpriteNum());
        m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition, GUILayout.Width(350), GUILayout.Height(200));
        for (int i = 0; i < m_spriteNum; i++)
        {
            var sprite = m_target.GetSprite()[i];
            GUILayout.BeginHorizontal();
            m_toggleIsOn[i] = GUILayout.Toggle(m_toggleIsOn[i], m_toggleIsOn[i] ? "ON " : "OFF");
            EditorGUI.LabelField(EditorGUILayout.GetControlRect(GUILayout.Width(30)), "" + i);
            if (m_toggleIsOn[i])
            {
                UpdateToggleStatus(i);
                m_chosenSprite = sprite;
            }
            EditorGUI.LabelField(EditorGUILayout.GetControlRect(GUILayout.Width(70)), sprite.name);
            if (GUILayout.Button("delete"))
            {
                removeList.Add(sprite);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        if(m_chosenSprite != null){
            float height = 240;
            float radio = m_chosenSprite.rect.width / m_chosenSprite.rect.height;
            float width = height * radio;
            EditorGUILayout.LabelField("width:" + m_chosenSprite.rect.width + " height:" + m_chosenSprite.rect.height);
            m_previewRect = EditorGUILayout.GetControlRect(GUILayout.Height(height), GUILayout.Width(width));
            EditorGUI.DrawPreviewTexture(m_previewRect, m_chosenSprite.texture);
        }
        else
        {
            float width = 320;
            float height = 240;
            m_previewRect = EditorGUILayout.GetControlRect(GUILayout.Height(height), GUILayout.Width(width));
            EditorGUI.DrawRect(m_previewRect, Color.gray);
            EditorGUI.LabelField(m_previewRect, "none");
        }
        m_rect = EditorGUILayout.GetControlRect(GUILayout.Height(100));
        EditorGUI.DrawRect(m_rect, Color.gray);
        EditorGUI.LabelField(m_rect, "drag here to add");
        if (GUILayout.Button("SAVE"))
        {
            if (onSave != null)
            {
                onSave();
            }
        }
        if (GUILayout.Button("RemoveDuplicate"))
        {
            m_target.RemoveDuplicate();
        }
        
        if (Event.current.type == EventType.DragUpdated)
        {
            m_mousePosition = Event.current.mousePosition;
            if (m_rect.Contains(m_mousePosition))
            {
                if (m_dragObjects == null)
                {
                    m_dragObjects = DragAndDrop.objectReferences;
                }
                //改变鼠标的外表  
                if (m_dragObjects[0] is Sprite)
                {

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                }
                else
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                }
            }
        }
        if ( Event.current.type == EventType.DragExited)
        {
            //Debug.Log("drag exit");
            if (m_rect.Contains(m_mousePosition))
            {
                //Debug.Log("m_dragObjects:" + m_dragObjects);
                foreach (var o in m_dragObjects)
                {
                    if (o is Sprite)
                    {
                        var sprite = o as Sprite;
                        m_target.AddSprite(sprite);
                    }
                }
            }
            m_dragObjects = null;
        }

        if (removeList.Count != 0)
        {
            foreach (var sprite in removeList)
            {
                m_target.RemoveSprite(sprite);
            }
        }
        if (m_target.GetSpriteNum() != m_spriteNum)
        {
            UpdateStatus();
        }
    }

    public void OnDestroy()
    {
        Debug.Log("window destroy");
        if (onSave != null)
        {
            onSave();
        }
    }
}
