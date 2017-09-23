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
        ChangeState,
        PhysicsSet,
        PosSet,
        VarSet,
    }

    public enum TokenType
    {
        Str,
        VarName,
        Op,
        Num,
        NewLine,
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
        Trigger_Anim,
        Trigger_AnimElem,
        Trigger_AnimTime,
        Trigger_LeftAnimElem,
        Trigger_Command,
        Trigger_PosX,
        Trigger_PosY,
        Trigger_VelX,
        Trigger_VelY,
        Trigger_StateNo,
        Trigger_PrevStateNo,
        Trigger_StateTime,       
        Trigger_DeltaTime,
        Trigger_PhysicsType,
        //trigger functions
        Trigger_Var,
        Trigger_Neg,
        //todo

    }

}