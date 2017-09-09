using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class WordRunner : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerLoader.LoadPlayer(PlayerId.P1,"Mike", this.transform);
	}
	
	// Update is called once per frame
	void Update () {
        GameEngine.Instance.Update();
	}
}
