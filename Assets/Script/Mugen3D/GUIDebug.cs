using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D{
public class GUIDebug : MonoBehaviour
{
    public static GUIDebug Instance;
    Player mPlayer;

    public void SetPlayer(Player p)
    {
        mPlayer = p;
    }

    void Awake()
    {
        Instance = this;
    }

    void OnGUI() {
       GUI.color = Color.black;
       if (mPlayer == null)
           return;
       GUILayout.Label(new GUIContent("stateNo:"+Triggers.Instance.StateNo(mPlayer)));
       GUILayout.Label(new GUIContent("stateTime:" +Triggers.Instance.Time(mPlayer)));
       GUILayout.Label(new GUIContent("anim:" + Triggers.Instance.AnimName(mPlayer)));
       GUILayout.Label(new GUIContent("animElem:" + Triggers.Instance.AnimElem(mPlayer)));
       GUILayout.Label(new GUIContent("leftAnimElem:" + Triggers.Instance.LeftAnimElem(mPlayer)));
       GUILayout.Label(new GUIContent("animTime:" + Triggers.Instance.AnimTime(mPlayer)));
       GUILayout.Label(new GUIContent("pos:" + Triggers.Instance.PosX(mPlayer)+","+Triggers.Instance.PosY(mPlayer)));
       GUILayout.Label(new GUIContent("vel:" + Triggers.Instance.VelX(mPlayer) + "," + Triggers.Instance.VelY(mPlayer)));
       GUILayout.Label(new GUIContent("command:" + Triggers.Instance.Command(mPlayer, 0)));
       GUILayout.Label(new GUIContent("ctrl:" + Triggers.Instance.Ctrl(mPlayer)));
       GUILayout.Label(new GUIContent("physics:" + Triggers.Instance.PhysicsType(mPlayer)));
       GUILayout.Label(new GUIContent("vars:" + mPlayer.vars.ToString()));
    }

}
}
