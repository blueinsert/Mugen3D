using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D {
public class Environment {
    private Unit m_unit;

    public Environment(Unit unit)
    {
        m_unit = unit;
    }

    public Vector2 pos
    {
        get
        {
            return Triggers.Instance.Pos(m_unit);
        }
    }

    public Vector2 vel
    {
        get
        {
            return Triggers.Instance.Vel(m_unit);
        }
    }

    public string physics
    {
        get
        {
            return Triggers.Instance.Physics(m_unit);
        }
    }

    public string moveType
    {
        get
        {
            return Triggers.Instance.MoveType(m_unit);
        }
    }

    public int animNo
    {
        get
        {
            return Triggers.Instance.AnimNo(m_unit);
        }
    }

    public string animName
    {
        get
        {
            return Triggers.Instance.AnimName(m_unit);
        }
    }

    public int animFrame
    {
        get
        {
            return Triggers.Instance.AnimFrame(m_unit);
        }
    }

    public int leftAnimFrame
    {
        get
        {
            return Triggers.Instance.LeftAnimFrame(m_unit);
        }
    }

    public bool justOnGround
    {
        get
        {
            return Triggers.Instance.JustOnGround(m_unit);
        }
    }

    public bool ctrl
    {
        get
        {
            return Triggers.Instance.Ctrl(m_unit);
        }
    }

    public string commands
    {
        get
        {
            return Triggers.Instance.Commands(m_unit);
        }
    }

    public bool CommandTest(string commandName){
        return Triggers.Instance.CommandTest(m_unit, commandName);
    }
}
}
