using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int Anim(Unit p)
    {
        return p.animCtr.animNo;
    }

    public string AnimName(Unit p)
    {
        return p.animCtr.animName;
    }

    public int AnimElem(Unit p)
    {
        return p.animCtr.AnimElem;
    }

    public int AnimTime(Unit p)
    {
        return p.animCtr.AnimTime;
    }

    public int LeftAnimElem(Unit p)
    {
        return p.animCtr.totalFrame - p.animCtr.AnimElem-1;
    }

    public int CommandTest(Unit p, int commandNameHashCode)
    {
        if (!p.status.ctrl)
            return 0;
        return p.cmdMgr.CommandIsActive(commandNameHashCode);
    }

    public int Ctrl(Unit p)
    {
        return p.status.ctrl == true ? 1 : 0;
    }

    public int StateNo(Unit p)
    {
        return p.stateMgr.currentState.stateId;
    }

    public int PrevStateNo(Unit p)
    {
        return p.stateMgr.GetPrevStateNo();
    }

    public int Time(Unit p)
    {
        return p.stateMgr.stateTime;

    }

    public float VelX(Unit p)
    {
        return p.moveCtr.velocity.x;
    }

    public float VelY(Unit p)
    {
        return p.moveCtr.velocity.y;
    }

    public float PosX(Unit p)
    {
        return p.transform.position.x;
    }

    public float PosY(Unit p)
    {
        return p.transform.position.y;
    }

    public string PhysicsType(Unit p)
    {
        return p.status.physicsType.ToString();
    }

    public float DeltaTime()
    {
        return World.Instance.deltaTime;
    }

    public int Var(Unit p, int id)
    {
        return p.GetVar(id);
    }

    public int HitVar(Unit p, int key)
    {
        return p.hitVars.GetHitVar(key);
    }

    public string MoveType(Unit p)
    {
        return p.status.moveType.ToString();
    }

    public float GetConfig(Unit p, int key)
    {
        return p.config.GetConfig(key);
    }

    public string EnemyMoveType(Unit p)
    {
        return p.enemy.status.moveType.ToString();
    }
    
    public float Random(Unit p)
    {
        //float r = p.randomGenerater.Next(100);
        //Log.Info("random:" + r);
        return 14;
    }
    public int JustOnGround(Unit p)
    {
        return p.moveCtr.justOnGround ? 1 : 0;
    }
    #endregion

}
}
