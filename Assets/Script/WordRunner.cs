using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class WordRunner : MonoBehaviour {
    int frameRate = 60;
    float timer = 0;
	// Use this for initialization
	void Start () {
        var p = PlayerLoader.LoadPlayer(PlayerId.P1,"Mike", this.transform.position, this.transform);
        PlayerLoader.LoadPlayer(PlayerId.P2, "Mike", this.transform.position, this.transform);
        CameraController.Instance.SetFollowTarget(p.transform);
        GUIDebug.Instance.SetPlayer(p);
	}
	
	// Update is called once per frame
	void Update () {
        float timeS = Time.time;
        GameEngine.Update(Time.deltaTime);
        float timeE = Time.time;
        //Debug.Log("take time:" + (timeE - timeS));
        /*
        float period = 1.0f / frameRate;
        timer += Time.deltaTime;
        if (timer >= period)
        {
            timer -= period;
            GameEngine.Update();
        }
        */
	}
}
