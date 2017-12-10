using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class WordRunner : MonoBehaviour {
    int frameRate = 60;
    float timer = 0;

    bool isPause = false;
    bool goNext = false;
	// Use this for initialization
	void Start () {
        OpcodeConfig.Init();
        var p1 = PlayerLoader.LoadPlayer(PlayerId.P1,"Origin", this.transform.position + new Vector3(0,0,-10), this.transform);
        World.Instance.AddPlayer(PlayerId.P1, p1);
        var p2 = PlayerLoader.LoadPlayer(PlayerId.P2, "Origin", this.transform.position + new Vector3(0, 0, +10), this.transform);
        p2.AiLevel = 1;
        World.Instance.AddPlayer(PlayerId.P2, p2);

        CameraController.Instance.SetFollowTarget(p1.transform);
        GUIDebug.Instance.AddPlayer(PlayerId.P1,p1);
        GUIDebug.Instance.AddPlayer(PlayerId.P2, p2);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPause = !isPause;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            goNext = true;
        }
        if(!isPause)
            GameEngine.Update(Time.deltaTime);
        else
        {
            if(goNext)
                GameEngine.Update(Time.deltaTime);
            goNext = false;
        }
	}
}
