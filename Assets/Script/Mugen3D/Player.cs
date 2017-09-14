using UnityEngine;
using System.Collections;

namespace Mugen3D{
public class Player : MonoBehaviour {
    public PlayerId id;
    public PlayerSetting setting;
    public Animation anim;
    public bool canCtrl = true;
    [HideInInspector]
    public MoveCtr moveCtr;
    [HideInInspector]
    public CmdManager cmdMgr {  get; private set; }
    [HideInInspector]
    public AnimationController animCtr {  get; private set; }
    [HideInInspector]
    public StateManager stateMgr;

    
    public void Init(PlayerSetting setting) { 
        moveCtr = new MoveCtr(this.transform);
        cmdMgr = new CmdManager();
        cmdMgr.LoadCmdFile(setting.commandFile);
        animCtr = new AnimationController(anim);
        stateMgr = new StateManager(this);
        stateMgr.ReadStateDefFile(setting.stateFiles.ToArray());
    }

    private void UpdatePlayer()
    {  
        moveCtr.Update();
        cmdMgr.Update(InputHandler.GetInputKeycode());
        animCtr.Update();
        stateMgr.Update();
    }

    public void Update()
    {
        UpdatePlayer();
    }

}
}
