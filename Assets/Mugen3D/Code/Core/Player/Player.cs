using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D{

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(DecisionBoxManager))]
public class Player : MonoBehaviour {
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
        cmdMgr.Update(InputHandler.GetInputKeycode(this.id));
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

}
}
