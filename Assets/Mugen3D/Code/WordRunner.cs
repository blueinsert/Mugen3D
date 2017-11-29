using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class WordRunner : MonoBehaviour {
    int frameRate = 60;
    float timer = 0;
	// Use this for initialization
	void Start () {
        OpcodeConfig.Init();
        var p1 = PlayerLoader.LoadPlayer(PlayerId.P1,"Origin", this.transform.position + new Vector3(0,0,-10), this.transform);
        World.Instance.AddPlayer(PlayerId.P1, p1);
        var p2 = PlayerLoader.LoadPlayer(PlayerId.P2, "Origin", this.transform.position + new Vector3(0, 0, +10), this.transform);
        World.Instance.AddPlayer(PlayerId.P2, p2);

        CameraController.Instance.SetFollowTarget(p1.transform);
        GUIDebug.Instance.AddPlayer(PlayerId.P1,p1);
        GUIDebug.Instance.AddPlayer(PlayerId.P2, p2);
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
