using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTest1 : MonoBehaviour {
    public PlayMode playMode;
	// Use this for initialization
	void Start () {
        LuaMgr.Instance.Env.DoString(string.Format("return require('{0}')", "main"));
        GameObject prefab = Resources.Load("Prefabs/Scene/Scene_Fight") as GameObject;
        GameObject gameGo = GameObject.Instantiate(prefab);
        gameGo.name = "ClientGame";
        var clientGame = gameGo.AddComponent<ClientGame>();
        clientGame.CreateGame("Origin", "Origin", "TrainingRoom", playMode);
        clientGame.StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
