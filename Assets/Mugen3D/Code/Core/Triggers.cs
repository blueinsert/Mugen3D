using System.Collections;
using System.Collections.Generic;
namespace Mugen3D{
public class Triggers
{

    private Triggers() { }

    private static Triggers mInstance;
    public static Triggers Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new Triggers();
                mInstance.Init();
            }
            return mInstance;
        }
    }

    private void Init() {

    }

    #region trigger function

    public int Anim(Player p)
    {
        return p.animCtr.animNo;
    }

    public string AnimName(Player p)
    {
        return p.animCtr.animName;
    }

    public int AnimElem(Player p)
    {
        return p.animCtr.AnimElem;
    }

    public int AnimTime(Player p)
    {
        return p.animCtr.AnimTime;
    }

    public int LeftAnimElem(Player p)
    {
        return p.animCtr.totalFrame - p.animCtr.AnimElem-1;
    }

    public int CommandTest(Player p, int commandNameHashCode)
    {
        return p.cmdMgr.CommandIsActive(commandNameHashCode);
    }

    public int Ctrl(Player p)
    {
        return p.canCtrl?1:0;
    }

    public int StateNo(Player p)
    {
        return p.stateMgr.currentState.stateId;
    }

    public int PrevStateNo(Player p)
    {
        return p.stateMgr.GetPrevStateNo();
    }

    public int Time(Player p)
    {
        return p.stateMgr.stateTime;

    }

    public float VelX(Player p)
    {
        return p.moveCtr.velocity.z;
    }

    public float VelY(Player p)
    {
        return p.moveCtr.velocity.y;
    }

    public float PosX(Player p)
    {
        return p.transform.position.z;
    }

    public float PosY(Player p)
    {
        return p.transform.position.y;
    }

    public string PhysicsType(Player p)
    {
        return p.moveCtr.type.ToString();
    }

    public float DeltaTime()
    {
        return GameEngine.deltaTime;
    }

    public int Var(Player p, int id)
    {
        return p.GetVar(id);
    }

    public int HitVar(Player p, int key)
    {
        return p.hitVars.GetHitVar(key);
    }

    public string MoveType(Player p)
    {
        return p.moveType.ToString();
    }

    public int GetConfig(Player p, int key)
    {
        return p.config.GetConfig(key);
    }

    public string EnemyMoveType(Player p)
    {
        return TeamMgr.GetEnemy(p).moveType.ToString();
    }

    public int AiLevel(Player p)
    {
        return p.AiLevel;
    }
    #endregion

}
}
