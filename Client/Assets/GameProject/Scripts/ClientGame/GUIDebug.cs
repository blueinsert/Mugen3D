using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.UGFramework;

public class GUIDebug : Singleton<GUIDebug>
{

    private Vector2[] m_scrollPositions = new Vector2[4];
    private Vector2[] m_areaStartPos = new Vector2[4] { new Vector2(0, 0), new Vector2(Screen.width / 2, 0), new Vector2(0, Screen.height / 2), new Vector2(Screen.width / 2, Screen.height / 2) };
    private Vector2 m_areaSize = new Vector2(Screen.width / 2, Screen.height / 2);

    private Dictionary<int, Dictionary<string, string>> m_msg = new Dictionary<int, Dictionary<string, string>>();

    public void SetMsg(int index, string key, string value)
    {
        if (!m_msg.ContainsKey(index))
        {
            m_msg[index] = new Dictionary<string, string>();
        }
        m_msg[index][key] = value;
    }

    private void ShowMessage(Dictionary<string, string> msgDic, int areaIndex)
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //设置背景填充
        fontStyle.normal.textColor = new Color(0, 0, 1);   //设置字体颜色
        fontStyle.fontSize = 15;       //字体大小
 
        var areaStartPos = m_areaStartPos[areaIndex];
        GUI.color = Color.blue;
        GUILayout.BeginArea(new UnityEngine.Rect(areaStartPos.x, areaStartPos.y, m_areaSize.x, m_areaSize.y));
        m_areaStartPos[areaIndex] = GUILayout.BeginScrollView(m_areaStartPos[areaIndex], GUILayout.Width(Screen.width / 2), GUILayout.Height(Screen.height));
        GUILayout.BeginVertical();
        foreach (var kv in msgDic)
        {
            GUILayout.Label(new GUIContent(kv.Key + ":" + kv.Value), fontStyle);
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void OnGUI()
    {
        int i = 0;
        foreach(var pair in m_msg)
        {
            if (i < 4)
            {
                ShowMessage(pair.Value, i++);
            }
        }
    }
}

