using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class TokenConfig
    {

        private static readonly Dictionary<OpCode, int> OpcodePriority = new Dictionary<OpCode, int>() { 
            {OpCode.AddOP,4},
            {OpCode.SubOP,4},
            {OpCode.MulOP,3},
            {OpCode.DivOP,3},
            {OpCode.EqualOP,7},
            {OpCode.NotEqual,7},
            {OpCode.Greater,6},
            {OpCode.GreaterEqual,6},
            {OpCode.Less,6},
            {OpCode.LessEqual,6},
            {OpCode.LogNot,2},
            {OpCode.LogAnd,11},
            {OpCode.LogOr,12},
            {OpCode.LeftBracket,1},
            {OpCode.RightBracket,1},
           
            {OpCode.Trigger_Anim,1},
            {OpCode.Trigger_AnimElem,1},
            {OpCode.Trigger_AnimTime,1},
            {OpCode.Trigger_Command,1},   
            {OpCode.Trigger_VelX,1},
            {OpCode.Trigger_VelY,1},
            {OpCode.Trigger_PosX,1},
            {OpCode.Trigger_PosY,1},
            {OpCode.Trigger_StateNo,1},
            {OpCode.Trigger_PrevStateNo,1},
            {OpCode.Trigger_StateTime,1},
            {OpCode.Trigger_DeltaTime,1},
            {OpCode.Trigger_Var,1},
            {OpCode.Trigger_Neg,1},
            {OpCode.Trigger_PhysicsType,1},
        };

        private static readonly Dictionary<string, OpCode> OpcodeStrIdMap = new Dictionary<string, OpCode>(){
            {"+",OpCode.AddOP},
            {"-",OpCode.SubOP},
            {"*",OpCode.MulOP},
            {"/",OpCode.DivOP},
            {"==",OpCode.EqualOP},
            {"!=",OpCode.NotEqual},
            {">",OpCode.Greater},
            {">=",OpCode.GreaterEqual},
            {"<",OpCode.Less},
            {"<=",OpCode.LessEqual},
            {"!",OpCode.LogNot},
            {"&&",OpCode.LogAnd},
            {"||",OpCode.LogOr},
            //{"-",OpCode.Op_Neg},
            {"(",OpCode.LeftBracket},
            {")",OpCode.RightBracket},
            {"Anim",OpCode.Trigger_Anim},
            {"AnimElem",OpCode.Trigger_AnimElem},
            {"AnimTime",OpCode.Trigger_AnimTime},
            {"LeftAnimElem",OpCode.Trigger_LeftAnimElem},
            {"Command",OpCode.Trigger_Command},
            {"PosX",OpCode.Trigger_PosX},
            {"PoxY",OpCode.Trigger_PosY},
            {"VelX",OpCode.Trigger_VelX},
            {"VelY",OpCode.Trigger_VelY},
            {"StateNo",OpCode.Trigger_StateNo},
            {"Time",OpCode.Trigger_StateTime},
            {"PrevStateNo",OpCode.Trigger_PrevStateNo},
            {"DeltaTime", OpCode.Trigger_DeltaTime},
            {"Physics", OpCode.Trigger_PhysicsType},
            {"Var",OpCode.Trigger_Var},
            {"Neg",OpCode.Trigger_Neg},
            // to do
        };

        public static bool IsTriggerFunc(Token t)
        {
            if (t.value == "Var" || t.value == "Neg" || t.value == "Command")
            {
                return true;
            }
            return false;
        }

        public static OpCode GetOpCode(string value) {
            if (OpcodeStrIdMap.ContainsKey(value))
            {
                return OpcodeStrIdMap[value];
            }
            else
            {
                Debug.LogWarning("can not find opcode, value:" + value);
                return OpCode.None;
            }
        }

        public static int GetOpCodePriority(OpCode code)
        {
            if (OpcodePriority.ContainsKey(code))
            {
                return OpcodePriority[code];
            }
            else
            {
                Debug.LogError("can not find opcode priority, code:" + code.ToString());
                Application.Quit();
                return 0;
            }
        }

    }
}