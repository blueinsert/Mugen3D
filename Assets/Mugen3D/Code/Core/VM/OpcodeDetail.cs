using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public enum OpCode
    {
        None,
        PushValue,
        PopValue,
        AddOP,
        SubOP,
        Neg,
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
        Trigger_AnimName,
        Trigger_AnimElem,
        Trigger_AnimTime,
        Trigger_Ctrl,
        Trigger_LeftAnimElem,
        Trigger_CommandTest,
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
        Trigger_HitVar,
        //todo

    }

    public class OpcodeDetail
    {
        public OpCode opcode;
        public string strValue;
        public int priority;
        public int inputNum;
        public bool isMugenBuildIn;
      
    }
}