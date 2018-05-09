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

    public string Physics(Unit p)
    {
        return p.status.physicsType.ToString();
    }

    public string MoveType(Unit p)
    {
        return p.status.moveType.ToString();
    }

    public Vector2 Pos(Unit p)
    {
        return p.transform.position;
    }

    public Vector2 Vel(Unit p)
    {
        return p.moveCtr.velocity;
    }

    public int AnimNo(Unit p)
    {
        return p.status.animNo;
    }

    public int AnimTime(Unit p)
    {
        return p.animCtr.animTime;
    }

    public int AnimElem(Unit p)
    {
        return p.animCtr.animElem;
    }

    public int AnimElemTime(Unit u)
    {
        return u.animCtr.animElemTime;
    }
    
    public int AnimLength(Unit p)
    {
        return p.animCtr.animLength;
    }
    
    public bool JustOnGround(Unit p)
    {
        return p.moveCtr.justOnGround;
    }

    public bool Ctrl(Unit p)
    {
        return p.status.ctrl;
    }

    public bool CommandTest(Unit p, string command)
    {
        return p.cmdMgr.CommandIsActive(command.GetHashCode());
    }

    public string Commands(Unit p)
    {
        return p.cmdMgr.GetActiveCommandName();
    }

    #endregion

}
}
