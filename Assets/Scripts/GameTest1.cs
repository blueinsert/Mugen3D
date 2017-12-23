using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTest1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject gameGo = new GameObject();
        gameGo.name = "ClientGame";
        var clientGame = gameGo.AddComponent<ClientGame>();
        clientGame.StartGame("Origin", "Origin", "TrainingRoom");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
