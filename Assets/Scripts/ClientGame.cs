using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayMode
{
    Training,
    SingleVS,
}

public class ClientGame : MonoBehaviour {
    public static ClientGame Instance;

    public PlayMode playMode;
    public FightUI fightUI;
    public RoundMgr roundMgr;

    private float timer = 0;
    private bool isPause = false;
    private bool goNext = false;
    private bool isIntializeComplete = false;

    private static readonly Dictionary<Mugen3D.PlayerId, Vector3> m_initPos = new Dictionary<Mugen3D.PlayerId, Vector3> { 
        {Mugen3D.PlayerId.P1, new Vector3(-1.5f, 0, 0)},
        {Mugen3D.PlayerId.P2, new Vector3(1.5f, 0, 0)},
    }; 

    public void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    private void LoadStage(string stageName)
    {
        UnityEngine.Object prefab = Resources.Load<UnityEngine.Object>("Stage/" + stageName + "/" + stageName);
        GameObject goStage = GameObject.Instantiate(prefab, this.transform.Find("Stage")) as GameObject;
        foreach (var colliderView in goStage.GetComponentsInChildren<Mugen3D.RectColliderView>())
        {
            Mugen3D.CollisionWorld.Instance.AddCollideable(colliderView);
        }
    }

    private void LoadPlayer(Mugen3D.PlayerId id, string name)
    {
        var player = PlayerLoader.LoadPlayer(id, name, this.transform.Find("Players"));
        player.transform.position = m_initPos[id];
        player.LockInput();
        Mugen3D.World.Instance.AddPlayer(player);
        Mugen3D.CollisionWorld.Instance.AddCollideable(player);
    }

    private void LoadFightUI()
    {
        GameObject prefabFightUI = Resources.Load<UnityEngine.GameObject>("Prefabs/UI/Fight/FightUIRoot");
        GameObject goFightUI = GameObject.Instantiate(prefabFightUI, this.transform) as GameObject;
        fightUI = goFightUI.GetComponent<FightUI>();
    }

    public void CreateGame(string p1CharacterName, string p2CharacterName, string stageName, PlayMode playMode = PlayMode.Training)
    {
        this.playMode = playMode;
        Mugen3D.OpcodeConfig.Init();
        
        Mugen3D.World.Instance.Clear();
        Mugen3D.CollisionWorld.Instance.Clear();

        LoadStage(stageName);
        LoadPlayer(Mugen3D.PlayerId.P1, p1CharacterName);
        LoadPlayer(Mugen3D.PlayerId.P2, p2CharacterName);
        LoadFightUI();

        Init();

        isIntializeComplete = true;
    }

    public void StartGame()
    {
        roundMgr.StartRound(1);
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

    private void Init()
    {
        roundMgr = GetRoundMgr(playMode);
        Mugen3D.CameraController mCameraController = GetComponentInChildren<Mugen3D.CameraController>();
        fightUI.Init(Mugen3D.World.Instance.GetPlayer(Mugen3D.PlayerId.P1), Mugen3D.World.Instance.GetPlayer(Mugen3D.PlayerId.P2));
        mCameraController.SetFollowTarget(Mugen3D.World.Instance.GetPlayer(Mugen3D.PlayerId.P1).transform, Mugen3D.World.Instance.GetPlayer(Mugen3D.PlayerId.P2).transform);
        Mugen3D.CollisionWorld.Instance.AddCollideable(mCameraController);
    }

    public void Reset()
    {
        var p1 = Mugen3D.World.Instance.GetPlayer(Mugen3D.PlayerId.P1);
        var p2 = Mugen3D.World.Instance.GetPlayer(Mugen3D.PlayerId.P2);
        p1.Reset();
        p2.Reset();
        p1.transform.position = m_initPos[p1.id];
        p2.transform.position = m_initPos[p2.id];
    }

    public void ReloadPlayer(Mugen3D.Player p)
    {
        isIntializeComplete = false;
        Mugen3D.CollisionWorld.Instance.RemoveCollideable(p);
        Mugen3D.World.Instance.RemovePlayer(p);
        LoadPlayer(p.id, p.name);
        Init();
        isIntializeComplete = true;
    }

    public void ReloadAllPlayer()
    {/*
        isIntializeComplete = false;
        var id = Mugen3D.PlayerId.P1;
        Mugen3D.World.Instance.RemovePlayer(id);
        LoadPlayer(id, m_characterName[id]);
        id = Mugen3D.PlayerId.P2;
        Mugen3D.World.Instance.RemovePlayer(id);
        LoadPlayer(id, m_characterName[id]);
        Init();
        isIntializeComplete = true;
      */
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
            Mugen3D.World.Instance.Update(Time.deltaTime);
        }
        else
        {
            if (goNext)
            {
                Mugen3D.World.Instance.Update(Time.deltaTime);
            }
            goNext = false;
        }
    }
}
