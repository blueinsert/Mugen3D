using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class GUIDebug : MonoBehaviour
{
    public static GUIDebug Instance;
 

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
        if (id == PlayerId.P1)
            GUILayout.BeginArea(new UnityEngine.Rect(0, 0, 100, Screen.height));
        else if (id == PlayerId.P2)
            GUILayout.BeginArea(new UnityEngine.Rect(Screen.width - 100, 0, 300, Screen.height));
        GUI.color = Color.red;
        GUILayout.Label(new GUIContent("playerId:" + id.ToString()));
        GUI.color = Color.black;
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
        GUILayout.Label(new GUIContent("justOnGround:"+Triggers.Instance.JustOnGround(p)));
        GUILayout.Label(new GUIContent("vars:" + Utility.DicToString<int,int>(p.vars)));

        GUILayout.EndArea();
    }

}

