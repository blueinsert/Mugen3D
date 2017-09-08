using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D {
    public enum PlayerId
    {
        P1,
        P2,
        P3,
        P4,
    }

    public enum MoveType
    {
        Idle,
        Attack,
        Defence,
    }

    public enum StateEventType
    {
        VelSet,
        ChangeAnim,
        SetPhysicsType,
        ChangeState,
    }

    public enum TokenType
    {
        Str,
        VarName,
        Op,
        Num,
        NewLine,
        ReservedWord,
        Other
    }

    public enum OpCode
    {
        None,
        PushValue,
        PopValue,
        AddOP,
        SubOP,
        MulOP,
        DivOP,
        EqualOP,
        NotEqual,
        Less,
        Greater,
        LessEqual,
        GreaterEqual,
        LogNot,
        LogAnd,
        LogOr,
        LeftBracket,
        RightBracket,
        //Mugen Sys Related
        //trigger vars
        Trigger_command,
        Trigger_StateTime,
        Trigger_Anim,
        Trigger_velx,
        //trigger function
        //todo

    }

    public enum MugenSysVar
    {
        command,
        vel_x,
        vel_y,
        Anim
    }
}