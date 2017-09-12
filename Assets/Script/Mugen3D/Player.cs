using UnityEngine;
using System.Collections;

namespace Mugen3D{
public class Player : MonoBehaviour {
    public PlayerId id;
    public PlayerSetting setting;
    public Animation anim;

    [HideInInspector]
    public PhysicsSys physics;
    [HideInInspector]
    public CmdManager cmdMgr {  get; private set; }
    [HideInInspector]
    public AnimationController animCtr {  get; private set; }
    [HideInInspector]
    public StateManager stateMgr;

    
    public void Init(PlayerSetting setting) { 
        physics = new PhysicsSys(this.transform);
        cmdMgr = new CmdManager();
        cmdMgr.LoadCmdFile(setting.commandFile);
        animCtr = new AnimationController(anim);
        stateMgr = new StateManager(this);
        stateMgr.ReadStateDefFile(setting.stateFiles.ToArray());
    }

    private void UpdatePlayer()
    {  
        physics.UpdatePhysics();
        cmdMgr.Update(InputHandler.GetInputKeycode());
        animCtr.UpdateSample();
        stateMgr.Update();
    }

    public void Update()
    {
        UpdatePlayer();
    }

}
}
