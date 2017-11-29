using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class OpcodeConfig
    {
        private static List<OpcodeDetail> opcodeDetails = new List<OpcodeDetail> { 
            new OpcodeDetail{opcode = OpCode.PushValue,               strValue = "push",                priority = 1,     inputNum = 1,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.PopValue,                strValue = "pop",                priority = 1,     inputNum = 1,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.AddOP,                   strValue = "+",               priority = 4,     inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.SubOP,                   strValue = "-",               priority = 4,     inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.MulOP,                   strValue = "*",               priority = 3,     inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.DivOP,                   strValue = "/",               priority = 3,     inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.EqualOP,                 strValue = "==",              priority = 7,     inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.NotEqual,                strValue = "!=",              priority = 7,     inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.Less,                    strValue = "<",               priority = 6,     inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.LessEqual,               strValue = "<=",              priority = 6,     inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.Greater,                 strValue = ">",               priority = 6,     inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.GreaterEqual,            strValue = ">=",              priority = 6,     inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.LogAnd,                  strValue = "&&",              priority = 11,    inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.LogOr,                   strValue = "||",              priority = 12,    inputNum = 2,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.LogNot,                  strValue = "!",               priority = 2,     inputNum = 1,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.LeftBracket,             strValue = "(",               priority = 1,     inputNum = 0,    isMugenBuildIn = false},
            new OpcodeDetail{opcode = OpCode.RightBracket,            strValue = ")",               priority = 1,     inputNum = 0,    isMugenBuildIn = false},
            //Mugen3D runtime
            new OpcodeDetail{opcode = OpCode.Trigger_Anim,            strValue = "Anim",            priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_AnimName,        strValue = "AnimName",        priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_AnimElem,        strValue = "AnimElem",        priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_AnimTime,        strValue = "AnimTime",        priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_LeftAnimElem,    strValue = "LeftAnimElem",    priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_CommandTest,     strValue = "CommandTest",     priority = 1,     inputNum = 1,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_PosX,            strValue = "PosX",            priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_PosY,            strValue = "PosY",            priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_VelX,            strValue = "VelX",            priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_VelY,            strValue = "VelY",            priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_StateNo,         strValue = "StateNo",         priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_PrevStateNo,     strValue = "PrevStateNo",     priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_StateTime,       strValue = "Time",            priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_DeltaTime,       strValue = "DeltaTime",       priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_PhysicsType,     strValue = "Physics",         priority = 1,     inputNum = 0,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_Var,             strValue = "Var",             priority = 1,     inputNum = 1,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_Neg,             strValue = "Neg",             priority = 1,     inputNum = 1,    isMugenBuildIn = true},
            new OpcodeDetail{opcode = OpCode.Trigger_HitVar,          strValue = "HitVar",          priority = 1,     inputNum = 1,    isMugenBuildIn = true},
        };

        private static Dictionary<OpCode, OpcodeDetail> OpcodeDetailMap = new Dictionary<OpCode,OpcodeDetail>();
        private static Dictionary<string, OpcodeDetail> StrDetailMap = new Dictionary<string, OpcodeDetail>();

        public static void Init()
        {
            foreach (var detail in opcodeDetails)
            {
                OpcodeDetailMap[detail.opcode] = detail;
                StrDetailMap[detail.strValue] = detail;
            }
        }

        public static OpCode GetOpcodeByStr(string str)
        {
            if (StrDetailMap.ContainsKey(str))
            {
                return StrDetailMap[str].opcode;
            }
            else
            {
                Log.Error("can't translate " + str + " to  OpCode");
                return OpCode.None;
            }
        }

        public static OpcodeDetail GetDetail(OpCode op)
        {
            return OpcodeDetailMap[op];
        }
    }
}
