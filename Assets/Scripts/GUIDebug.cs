using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class GUIDebug : MonoBehaviour
{
    public static GUIDebug Instance;
    private Vector2 m_p1ScrollPosition = new Vector2(Screen.width / 2, 0);
    private Vector2 m_p2ScrollPosition = new Vector2(Screen.width / 2, 0);

    private Dictionary<int, Dictionary<string, string>> m_msg = new Dictionary<int, Dictionary<string, string>>();
    private int m_curIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    public void AddMsg(string key, string value)
    {
        AddMsg(0, key, value);
    }

    public void AddMsg(int index, string key, string value)
    {
        if (!m_msg.ContainsKey(index))
        {
            m_msg[index] = new Dictionary<string, string>();
        }
        m_msg[index][key] = value;
    }

    void OnGUI()
    {
        if (m_msg.Count != 0 && m_msg.ContainsKey(m_curIndex))
        {
            GUI.color = Color.blue;
            GUILayout.BeginArea(new UnityEngine.Rect(0, 0, Screen.width / 2, Screen.height));
            m_p1ScrollPosition = GUILayout.BeginScrollView(m_p1ScrollPosition, GUILayout.Width(Screen.width / 2), GUILayout.Height(Screen.height));
            GUILayout.BeginVertical();
            GUILayout.Label(new GUIContent("curIndex:" + m_curIndex));
            foreach (var kv in m_msg[m_curIndex])
            {
                GUILayout.Label(new GUIContent(kv.Key + ":" + kv.Value));
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
            GUILayout.EndScrollView();
        }
    }
}

