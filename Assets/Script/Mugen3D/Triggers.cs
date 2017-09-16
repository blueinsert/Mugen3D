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
        return p.animCtr.totalFrame - p.animCtr.AnimElem;
    }

    public string Command(Player p)
    {
        return p.cmdMgr.GetActiveCommandName();
    }

    public bool Ctrl(Player p)
    {
        return p.canCtrl;
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


    #endregion

}
}
