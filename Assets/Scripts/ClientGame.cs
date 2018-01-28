using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public enum PlayMode
{
    Training,
    SingleVS,
}

public class ClientGame : MonoBehaviour {
    public World world;
    int frameRate = 60;
    float timer = 0;

    bool isPause = false;
    bool goNext = false;
   
    private CameraController mCameraController;
    public PlayMode playMode;
    public static ClientGame Instance;
    public FightUI fightUI;
    public RoundMgr roundMgr;
   
    private bool isIntializeComplete = false;
    private static readonly Dictionary<PlayerId, Vector3> mInitPos = new Dictionary<PlayerId, Vector3> { 
        {PlayerId.P1, new Vector3(-1.5f, 0, 0)},
        {PlayerId.P2, new Vector3(1.5f, 0, 0)},
    }; 
    private Transform mPlayerRoot;
    private Dictionary<PlayerId, string> mCharacterName = new Dictionary<PlayerId, string>();

    public void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    private RoundMgr GetRoundMgr(PlayMode playMode)
    {
        RoundMgr mgr = null;
        if (playMode == PlayMode.Training)
        {
            mgr = new RoundMgrTrain();
        }
        else if (playMode == PlayMode.SingleVS)
        {
            mgr = new RoundMgrSingleVs();
        }
        if (mgr != null)
        {
            mgr.Init(this);
        }
        return mgr;
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
        //Debug.Log("collider size:" + collisionWorld.GetCollideableNum());
    }

    private void Init()
    {
        fightUI.Init(World.Instance.GetPlayer(PlayerId.P1), World.Instance.GetPlayer(PlayerId.P2));
        mCameraController.SetFollowTarget(World.Instance.GetPlayer(PlayerId.P1).transform, World.Instance.GetPlayer(PlayerId.P2).transform);
        InitCollisionWorld();
    }

    public void CreateGame(string p1CharacterName, string p2CharacterName, string stageName, PlayMode playMode = PlayMode.Training)
    {
        this.playMode = playMode;
        OpcodeConfig.Init();

        LoadStage(stageName);
        this.world = World.Instance;
        mCharacterName.Add(PlayerId.P1, p1CharacterName);
        mCharacterName.Add(PlayerId.P2, p2CharacterName);
        LoadPlayer(PlayerId.P1, p1CharacterName);
        LoadPlayer(PlayerId.P2, p2CharacterName);
        LoadFightUI();
        mCameraController = GetComponentInChildren<CameraController>();
        Init();
        roundMgr = GetRoundMgr(playMode);
        isIntializeComplete = true;
    }

    public void StartGame()
    {
        roundMgr.StartRound(1);
    }

    private void LoadStage(string stageName)
    {
        //create stage
        UnityEngine.Object o = Resources.Load<UnityEngine.Object>("Stage/" + stageName + "/" + stageName);
        GameObject goStage = GameObject.Instantiate(o, this.transform) as GameObject;
        goStage.name = "stage_" + stageName;
        goStage.transform.parent = this.transform;
        //create characters
        mPlayerRoot = goStage.transform.Find("Players"); 
    }

    private void LoadPlayer(PlayerId id, string name)
    {
        var p = PlayerLoader.LoadPlayer(id, name, mPlayerRoot, new Vector3(0, 0, 0));
        p.transform.position = mInitPos[id];
        p.LockInput();
        world.AddPlayer(id,p);
    }

    private void LoadFightUI()
    {
        GameObject prefabFightUI = Resources.Load<UnityEngine.GameObject>("Prefabs/UI/Fight/FightUIRoot");
        GameObject goFightUI = GameObject.Instantiate(prefabFightUI, this.transform) as GameObject;
        fightUI = goFightUI.GetComponent<FightUI>();
    }

    public void Reset()
    {
        var p1 = World.Instance.GetPlayer(PlayerId.P1);
        var p2 = World.Instance.GetPlayer(PlayerId.P2);
        p1.Reset();
        p2.Reset();
        p1.transform.position = mInitPos[p1.id];
        p2.transform.position = mInitPos[p2.id];
    }

    public void ReloadPlayer(PlayerId id)
    {
        isIntializeComplete = false;
        world.RemovePlayer(id);
        LoadPlayer(id, mCharacterName[id]);
        Init();
        isIntializeComplete = true;
    }

    public void ReloadAllPlayer()
    {
        isIntializeComplete = false;
        var id = PlayerId.P1;
        world.RemovePlayer(id);
        LoadPlayer(id, mCharacterName[id]);
        id = PlayerId.P2;
        world.RemovePlayer(id);
        LoadPlayer(id, mCharacterName[id]);
        Init();
        isIntializeComplete = true;
    }

    void Update()
    {
        if (roundMgr != null)
        {
            roundMgr.Update();
        }
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
