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
        Application.targetFrameRate = 60;
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
        GameObject goStage = GameObject.Instantiate(o, this.transform) as GameObject;
        goStage.name = "stage_"+stageName;
        goStage.transform.parent = this.transform;
        //create characters
        var rootPlayers = goStage.transform.Find("Players");
        var p1 = PlayerLoader.LoadPlayer(PlayerId.P1, p1CharacterName, rootPlayers);
        var p2 = PlayerLoader.LoadPlayer(PlayerId.P2, p2CharacterName, rootPlayers);
        //init camera
        var cameraController = goStage.GetComponentInChildren<CameraController>();
        cameraController.SetFollowTarget(p1.transform, p2.transform);
        //init collision World
        var collisionWorld = CollisionWorld.Instance;
        collisionWorld.Clear();
        foreach (var colliderView in GetComponentsInChildren<RectColliderView>())
        {
            collisionWorld.AddCollider(colliderView.GetCollider());
        }
        Debug.Log("collider size:" + collisionWorld.GetColliderNum());
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
        {
            CollisionWorld.Instance.Update();
            world.Update(Time.deltaTime);
        }
        else
        {
            if (goNext)
            {
                CollisionWorld.Instance.Update();
                world.Update(Time.deltaTime);
            }
            goNext = false;
        }
    }
}
