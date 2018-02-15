using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D{

    public enum MoveType
    {
        Attack,
        Idle,
        Defence,
        BeingHitted,
    }

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(DecisionBoxManager))]
public class Player : Entity, Collideable {
    const int randomSeed = 123456789;
    public System.Random randomGenerater = new System.Random(randomSeed);
    public int AiLevel;
    public PlayerConfig config;
    [HideInInspector]
    public MoveType moveType = MoveType.Idle;
    [HideInInspector]
    public PlayerId id;
    public PlayerSetting setting;
    private Animation anim;
    [HideInInspector]
    public AnimationsDef animDef;
    [HideInInspector]
    public int facing = 1;
    [HideInInspector]
    public bool canCtrl = true;
    private int pauseTime = 0;
    [HideInInspector]
    public HitVars hitVars;
    [HideInInspector]
    public MoveCtr moveCtr;
    [HideInInspector]
    public CmdManager cmdMgr {  get; private set; }
    [HideInInspector]
    public AnimationController animCtr {  get; private set; }
    [HideInInspector]
    public StateManager stateMgr;
    private bool m_lockInput = false;
    public Dictionary<int, int> vars;

    public bool IsPause()
    {
        return pauseTime > 0;
    }

    public void Pause(int duration)
    {
        pauseTime = duration;
    }

    public void SetHitVars(HitVars hitvars)
    {
        this.hitVars = hitvars;
    }
  
    public void Init(PlayerSetting setting) { 
        //
        moveCtr = new MoveCtr(this.transform);
        moveCtr.owner = this;
        //
        cmdMgr = new CmdManager();
        cmdMgr.LoadCmdFile(setting.commandFile);   
        //
        anim = this.GetComponent<Animation>();
        animCtr = new AnimationController(anim, this, setting.animDef);
        //
        stateMgr = new StateManager(this);
        stateMgr.ReadStateDefFile(setting.stateFiles.ToArray());
        //
        config = new PlayerConfig(setting.playerConfig);
        moveCtr.gravity = new Vector3(0, config.GetConfig("Gravity"), 0);
        vars = new Dictionary<int, int>();
        
    }

    public void ChangeFacing(int facing)
    {
        if (this.facing != facing)
        {
            this.facing = facing;
            this.transform.localScale = new Vector3(1, 1, facing);
        }
    }

    private void UpdatePlayer()
    {
        moveCtr.Update();
        if(m_lockInput == false)
            cmdMgr.Update(InputHandler.GetInputKeycode(this.id, this.facing));
        animCtr.Update();
        stateMgr.Update();
    }

    public void OnUpdate()
    {
        if (pauseTime > 0)
        {
            pauseTime--;
            return;
        }
        UpdatePlayer();
    }

    public int GetVar(int id)
    {
        if (vars != null && vars.ContainsKey(id))
        {
            return vars[id];
        }
        return -1;
    }

    public void SetVar(int id, int value)
    {
        if (vars == null)
        {
            vars = new Dictionary<int, int>();
        }
        vars[id] = value;
    }

    public double CalcExpressionInRuntime(Expression ex)
    {
        VirtualMachine vm = new VirtualMachine();
        vm.SetOwner(this);
        return vm.Execute(ex);
    }

    public void LockInput()
    {
        this.m_lockInput = true;
    }

    public void UnlockInput()
    {
        this.m_lockInput = false;
    }

    public void Reset()
    {
        this.hp = this.MaxHP;
        this.stateMgr.ChangeState(0);
    }

    public Collider[] GetColliders()
    {
        return new Collider[] { this.GetComponent<DecisionBoxManager>().GetCollider()};
    }
}
}
