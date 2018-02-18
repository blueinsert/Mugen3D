using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class GUIDebug : MonoBehaviour
{
    public static GUIDebug Instance;
    private Vector2 m_p1ScrollPosition = new Vector2(Screen.width / 2, 0);
    private Vector2 m_p2ScrollPosition = new Vector2(Screen.width / 2, 0);
    void Awake()
    {
        Instance = this;
    }

    void OnGUI() {
        foreach (var p in ClientGame.Instance.world.GetAllPlayers())
        {
            Draw(p.id, p);
        }  
    }

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
            GUILayout.Label(new GUIContent("stateNo:" + Triggers.Instance.StateNo(p)));
            GUILayout.Label(new GUIContent("stateTime:" + Triggers.Instance.Time(p)));
            GUILayout.Label(new GUIContent("anim:" + Triggers.Instance.Anim(p)));
            GUILayout.Label(new GUIContent("animName:" + Triggers.Instance.AnimName(p)));
            GUILayout.Label(new GUIContent("animElem:" + Triggers.Instance.AnimElem(p)));
            GUILayout.Label(new GUIContent("leftAnimElem:" + Triggers.Instance.LeftAnimElem(p)));
            GUILayout.Label(new GUIContent("animTime:" + Triggers.Instance.AnimTime(p)));
            GUILayout.Label(new GUIContent("pos:" + Triggers.Instance.PosX(p) + "," + Triggers.Instance.PosY(p)));
            GUILayout.Label(new GUIContent("vel:" + Triggers.Instance.VelX(p) + "," + Triggers.Instance.VelY(p)));
            GUILayout.Label(new GUIContent("commands:" + p.cmdMgr.GetActiveCommandName()));
            GUILayout.Label(new GUIContent("ctrl:" + Triggers.Instance.Ctrl(p)));
            GUILayout.Label(new GUIContent("justOnGround:" + Triggers.Instance.JustOnGround(p)));
            GUILayout.Label(new GUIContent("vars:" + Utility.DicToString<int, int>(p.vars)));
        }
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
        GUILayout.EndScrollView();
    }

}

