using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class ClientGame : MonoBehaviour {
    public World world;
    int frameRate = 60;
    float timer = 0;

    bool isPause = false;
    bool goNext = false;
    private bool isIntializeComplete = false;

    public static ClientGame Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    public void StartGame(string p1CharacterName, string p2CharacterName, string stageName)
    {
        world = World.Instance;
        OpcodeConfig.Init();
        //create stage
        UnityEngine.Object o = Resources.Load<UnityEngine.Object>("Stage/" + stageName + "/" + stageName);
        GameObject go = GameObject.Instantiate(o, this.transform) as GameObject;
        go.name = "stage_"+stageName;
        //var playerRoot = go.transform.Find("Players");
        var p1Pos = go.transform.Find("Players/P1");
        var p2Pos = go.transform.Find("Players/P2");
        //create characters
        var p1 = PlayerLoader.LoadPlayer(PlayerId.P1, p1CharacterName, p1Pos); 
        var p2 = PlayerLoader.LoadPlayer(PlayerId.P2, p2CharacterName, p2Pos);
        //init camera
        var cameraController = go.GetComponentInChildren<CameraController>();
        cameraController.SetFollowTarget(p1.transform);
        world.AddPlayer(PlayerId.P1, p1);
        //p2.AiLevel = 1;
        world.AddPlayer(PlayerId.P2, p2);

        GUIDebug.Instance.AddPlayer(PlayerId.P1, p1);
        GUIDebug.Instance.AddPlayer(PlayerId.P2, p2);
        isIntializeComplete = true;
    }

    void Update()
    {
        if (!isIntializeComplete)
            return;
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPause = !isPause;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            goNext = true;
        }
        if (!isPause)
            world.Update(Time.deltaTime);
        else
        {
            if (goNext)
                world.Update(Time.deltaTime);
            goNext = false;
        }
    }
}
