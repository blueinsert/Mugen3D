using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class GameTest1 : MonoBehaviour {
    public Mugen3D.PlayMode playMode;

	void Start () {
        LuaMgr.Instance.Env.DoString(string.Format("return require('{0}')", "Lua/main"));
        GameObject go = new GameObject("Battle");
        var clientGame = go.AddComponent<ClientGame>();
        //clientGame.CreateGame("Origin", "Origin", "TrainingRoom", playMode);
	}
	
	void Update () {
		
	}
}
