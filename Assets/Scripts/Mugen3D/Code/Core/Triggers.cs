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

    public string AnimName(Unit p)
    {
        return p.animCtr.animName;
    }

    public int AnimFrame(Unit p)
    {
        return p.animCtr.animFrame;
    }

    public int AnimTime(Unit p)
    {
        return p.animCtr.animTime;
    }

    public int LeftAnimFrame(Unit p)
    {
        return p.animCtr.totalFrame - p.animCtr.animFrame - 1;
    }

    public int CommandTest(Unit p, string command)
    {
        if (!p.status.ctrl)
            return 0;
        return p.cmdMgr.CommandIsActive(command.GetHashCode());
    }

    public int Ctrl(Unit p)
    {
        return p.status.ctrl == true ? 1 : 0;
    }

    public float VelX(Unit p)
    {
        return p.moveCtr.velocity.x;
    }

    public float VelY(Unit p)
    {
        return p.moveCtr.velocity.y;
    }

    public Vector2 Pos(Unit p)
    {
        return p.transform.position;
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

    public string MoveType(Unit p)
    {
        return p.status.moveType.ToString();
    }
    
    public int JustOnGround(Unit p)
    {
        return p.moveCtr.justOnGround ? 1 : 0;
    }

    #endregion

}
}
