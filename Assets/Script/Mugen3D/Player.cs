using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D{
public class Player : MonoBehaviour {
    public PlayerId id;
    public PlayerSetting setting;
    public Animation anim;
    public int facing = 1;
    public bool canCtrl = true;
    [HideInInspector]
    public MoveCtr moveCtr;
    [HideInInspector]
    public CmdManager cmdMgr {  get; private set; }
    [HideInInspector]
    public AnimationController animCtr {  get; private set; }
    [HideInInspector]
    public StateManager stateMgr;

    public MyDictionary<int, int> vars;

    public void Init(PlayerSetting setting) { 
        moveCtr = new MoveCtr(this.transform);
        cmdMgr = new CmdManager();
        cmdMgr.LoadCmdFile(setting.commandFile);
        animCtr = new AnimationController(anim);
        stateMgr = new StateManager(this);
        stateMgr.ReadStateDefFile(setting.stateFiles.ToArray());
        vars = new MyDictionary<int, int>();
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
            vars = new MyDictionary<int, int>();
        }
        vars[id] = value;
    }

}
}
