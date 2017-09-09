using UnityEngine;
using System.Collections;

namespace Mugen3D{
public class Player : MonoBehaviour {
    public PlayerSetting setting;
    public Animation anim;

    private string cmdFile = "";
    private PhysicsSys physics;
    public CmdManager cmdMgr {  get; private set; }
    public AnimationController animCtr {  get; private set; }
    private StateManager stateMgr;

    
    public void Init(PlayerSetting setting) { 
        physics = new PhysicsSys(this.transform);
        cmdMgr = new CmdManager();
        cmdFile = Application.dataPath + "/" + "test.cmd";
        cmdMgr.LoadCmdFile(cmdFile);
        animCtr = new AnimationController(anim);
        stateMgr.ReadStateDefFile(setting.stateFiles.ToArray());
    }

    public void UpdatePlayer()
    {  
        physics.UpdatePhysics();
        cmdMgr.Update(InputHandler.GetInputKeycode());
        animCtr.UpdateSample();
        stateMgr.Update();
    }

    void Update()
    {
        UpdatePlayer();
        Debug.Log("command:" + Command);
    }

    #region 控制函数
    public void VelSet(float x,float y,float z) {
        Vector3 v = new Vector3(x, y, z);
        physics.Velocity = v;
    }

    public void ChangeAnim(string animName)
    {
        //animCtr.SetPlayAnim(animName);
    }

    public void SetPhysicsType(string type)
    {
        switch(type){
            case "Stand":
                physics.Physics = PhysicsType.Stand;
                break;
            case "Crouch":
                physics.Physics = PhysicsType.Croch;
                break;
            case "Air":
                physics.Physics = PhysicsType.Air;
                break;
            default:
                physics.Physics = PhysicsType.None;
                break;
        }
    }

    #endregion

    #region 触发器
    //public string AnimName { get { return animCtr.animName; } }
    //public int AnimTime { get { return animCtr.AnimTime; } }
    public Vector3 Vel { get { return physics.Velocity; } }
    public string Command {get {return cmdMgr.GetActiveCommandName();}}
    #endregion

}
}
