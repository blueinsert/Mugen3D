﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayMode
{
    Training,
    SingleVS,
}

public class ClientGame : MonoBehaviour {
    public static ClientGame Instance;
    public Mugen3D.World world;
    public PlayMode playMode;
    public FightUI fightUI;
    public RoundMgr roundMgr;
    public Mugen3D.Character p1;
    public Mugen3D.Character p2;

    private float timer = 0;
    private bool isPause = false;
    private bool goNext = false;
    private bool isIntializeComplete = false;

    private static readonly List<Vector3> m_initPos = new List<Vector3> { 
        new Vector3(-1.5f, 0.3f, 0),
        new Vector3(1.5f, 0.3f, 0),
    }; 

    public void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        world = Mugen3D.World.Instance;
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    private void LoadStage(string stageName)
    {
        UnityEngine.Object prefab = Resources.Load<UnityEngine.Object>("Stage/" + stageName + "/" + stageName);
        GameObject goStage = GameObject.Instantiate(prefab, this.transform.Find("Stage")) as GameObject;
        foreach (var collider in goStage.GetComponentsInChildren<Mugen3D.Collider>())
        {
            world.collisionWorld.AddCollider(collider);
        }
    }

    private Mugen3D.Character LoadPlayer(int slot, string name)
    {
        var player = Mugen3D.EntityLoader.LoadPlayer(slot, name, this.transform.Find("Players"));
        player.transform.position = m_initPos[slot];
        Mugen3D.Controllers.Instance.CtrlSet(player, false);
        return player;
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
        
        LoadStage(stageName);
        p1 = LoadPlayer(0, p1CharacterName);
        p2 = LoadPlayer(1, p2CharacterName);
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
        fightUI.Init(this.p1, this.p2);
        mCameraController.SetFollowTarget(this.p1.transform, this.p2.transform);
        world.collisionWorld.AddCollider(mCameraController.leftCollider);
        world.collisionWorld.AddCollider(mCameraController.rightCollider);
    }

    /*
    public void Reset()
    {
        var p1 = Mugen3D.World.Instance.GetPlayer(Mugen3D.PlayerId.P1);
        var p2 = Mugen3D.World.Instance.GetPlayer(Mugen3D.PlayerId.P2);
       // p1.Reset();
        //p2.Reset();
        p1.transform.position = m_initPos[p1.id];
        p2.transform.position = m_initPos[p2.id];
    }

    public void ReloadPlayer(Mugen3D.Character p)
    {
        isIntializeComplete = false;
        LoadPlayer(p.id, p.name);
        Init();
        GameObject.Destroy(p.gameObject);
        isIntializeComplete = true;
    }

    public void ReloadAllPlayer()
    {
        isIntializeComplete = false;
        var id = Mugen3D.PlayerId.P1;
        Mugen3D.World.Instance.RemovePlayer(id);
        LoadPlayer(id, m_characterName[id]);
        id = Mugen3D.PlayerId.P2;
        Mugen3D.World.Instance.RemovePlayer(id);
        LoadPlayer(id, m_characterName[id]);
        Init();
        isIntializeComplete = true;
      
    }
*/

    void Update()
    {
        if (roundMgr != null)
        {
            roundMgr.Update();
        }
        //if (!isIntializeComplete)
         //   return;
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
