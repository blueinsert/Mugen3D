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

    private CameraController mCameraController;

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
       
        OpcodeConfig.Init();

        LoadStagePlayers(stageName, p1CharacterName, p2CharacterName);
        LoadFightUI();
        InitCamera();
        InitCollisionWorld();

        GUIDebug.Instance.AddPlayer(PlayerId.P1, World.Instance.GetPlayer(PlayerId.P1));
        GUIDebug.Instance.AddPlayer(PlayerId.P2, World.Instance.GetPlayer(PlayerId.P2));

        isIntializeComplete = true;
    }

    private void LoadStagePlayers(string stageName, string p1CharacterName, string p2CharacterName)
    {
        //create stage
        UnityEngine.Object o = Resources.Load<UnityEngine.Object>("Stage/" + stageName + "/" + stageName);
        GameObject goStage = GameObject.Instantiate(o, this.transform) as GameObject;
        goStage.name = "stage_" + stageName;
        goStage.transform.parent = this.transform;
        //create characters
        var rootPlayers = goStage.transform.Find("Players");
        var p1 = PlayerLoader.LoadPlayer(PlayerId.P1, p1CharacterName, rootPlayers);
        var p2 = PlayerLoader.LoadPlayer(PlayerId.P2, p2CharacterName, rootPlayers);

        world = World.Instance;
        world.AddPlayer(PlayerId.P1, p1);
        //p2.AiLevel = 1;
        world.AddPlayer(PlayerId.P2, p2);
    }

    private void LoadFightUI()
    {
        GameObject prefabFightUI = Resources.Load<UnityEngine.GameObject>("Prefabs/UI/FightUIRoot");
        GameObject fightUI = GameObject.Instantiate(prefabFightUI, this.transform) as GameObject;
        FightUIManager uiScript = fightUI.GetComponent<FightUIManager>();
        uiScript.SetPlayer(World.Instance.GetPlayer(PlayerId.P1), World.Instance.GetPlayer(PlayerId.P2));
    }

    private void InitCamera()
    {
        mCameraController = GetComponentInChildren<CameraController>();
        mCameraController.SetFollowTarget(World.Instance.GetPlayer(PlayerId.P1).transform, World.Instance.GetPlayer(PlayerId.P2).transform);
    }

    private void InitCollisionWorld()
    {
        var collisionWorld = CollisionWorld.Instance;
        collisionWorld.Clear();
        foreach (var colliderView in GetComponentsInChildren<RectColliderView>())
        {
            collisionWorld.AddCollideable(colliderView);
        }
        collisionWorld.AddCollideable(mCameraController);
        collisionWorld.AddCollideable(World.Instance.GetPlayer(PlayerId.P1));
        collisionWorld.AddCollideable(World.Instance.GetPlayer(PlayerId.P2));
        Debug.Log("collider size:" + collisionWorld.GetCollideableNum());
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
            world.Update(Time.deltaTime);
        }
        else
        {
            if (goNext)
            {
                world.Update(Time.deltaTime);
            }
            goNext = false;
        }
    }
}
