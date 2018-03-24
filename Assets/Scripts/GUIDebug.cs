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

    public void AddMsg(int index, string key, string value)
    {
        if (!m_msg.ContainsKey(index))
        {
            m_msg[index] = new Dictionary<string, string>();
        }
        m_msg[index][key] = value;            
    }

    void OnGUI() {
        if(m_msg[m_curIndex]!=null){
            GUI.color = Color.blue;
            GUILayout.BeginArea(new UnityEngine.Rect(0, 0, Screen.width / 2, Screen.height));
            m_p1ScrollPosition = GUILayout.BeginScrollView(m_p1ScrollPosition, GUILayout.Width(Screen.width / 2), GUILayout.Height(Screen.height));
            GUILayout.BeginVertical();
            GUILayout.Label(new GUIContent("curIndex:" + m_curIndex));
            foreach (var kv in m_msg[m_curIndex]){
                GUILayout.Label(new GUIContent(kv.Key + ":" + kv.Value));
            }
             GUILayout.EndVertical();
             GUILayout.EndArea();
             GUILayout.EndScrollView();
        }   
    }

    /*
    void Draw(PlayerId id, Player p)
    {
        GUI.color = Color.blue;
        if (id == PlayerId.P1){
            GUILayout.BeginArea(new UnityEngine.Rect(0, 0, Screen.width / 2, Screen.height));
            m_p1ScrollPosition = GUILayout.BeginScrollView(m_p1ScrollPosition, GUILayout.Width(Screen.width / 2), GUILayout.Height(Screen.height));
        }else{
            GUILayout.BeginArea(new UnityEngine.Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height));
            m_p2ScrollPosition = GUILayout.BeginScrollView(m_p2ScrollPosition, GUILayout.Width(Screen.width / 2), GUILayout.Height(Screen.height));
        }

        GUILayout.BeginVertical();
        {
            GUILayout.Label(new GUIContent("playerId:" + id.ToString()));
            GUILayout.Label(new GUIContent("hp:" + p.GetHP()));
            //GUILayout.Label(new GUIContent("ai:" + Triggers.Instance.AiLevel(p)));
            GUILayout.Label(new GUIContent("moveType:" + Triggers.Instance.MoveType(p).ToString()));
            GUILayout.Label(new GUIContent("physics:" + Triggers.Instance.PhysicsType(p)));
            GUILayout.Label(new GUIContent("animName:" + Triggers.Instance.AnimName(p)));
            GUILayout.Label(new GUIContent("animElem:" + Triggers.Instance.AnimFrame(p)));
            GUILayout.Label(new GUIContent("leftAnimElem:" + Triggers.Instance.LeftAnimFrame(p)));
            GUILayout.Label(new GUIContent("animTime:" + Triggers.Instance.AnimTime(p)));
            GUILayout.Label(new GUIContent("pos:" + Triggers.Instance.PosX(p) + "," + Triggers.Instance.PosY(p)));
            GUILayout.Label(new GUIContent("vel:" + Triggers.Instance.VelX(p) + "," + Triggers.Instance.VelY(p)));
            GUILayout.Label(new GUIContent("commands:" + p.cmdMgr.GetActiveCommandName()));
            GUILayout.Label(new GUIContent("ctrl:" + Triggers.Instance.Ctrl(p)));
            GUILayout.Label(new GUIContent("justOnGround:" + Triggers.Instance.JustOnGround(p)));
        }
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
        GUILayout.EndScrollView();
    }
     */

}

