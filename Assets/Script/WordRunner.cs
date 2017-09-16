using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class WordRunner : MonoBehaviour {
    int frameRate = 60;
    float timer = 0;
	// Use this for initialization
	void Start () {
        PlayerLoader.LoadPlayer(PlayerId.P1,"Mike", this.transform);
	}
	
	// Update is called once per frame
	void Update () {
        float period = 1.0f / frameRate;
        timer += Time.deltaTime;
        if (timer >= period)
        {
            timer -= period;
            GameEngine.Update();
        }
        
	}
}
