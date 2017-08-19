using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class TokenConfig
    {
        public static readonly List<string> ReservedWords = new List<string> { 
            "State", 
            "Statedef",
            "Command"
        };

        public static readonly Dictionary<OpCode, int> OpcodePriority = new Dictionary<OpCode, int>() { 
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
            {OpCode.Trigger_StateTime,1},
            {OpCode.Trigger_Anim,1},
            {OpCode.Trigger_command,1},
            {OpCode.Trigger_velx,1},
        };

        public static readonly Dictionary<string, OpCode> OpcodeStrIdMap = new Dictionary<string, OpCode>(){
            {"+",OpCode.AddOP},
            {"-",OpCode.SubOP},
            {"*",OpCode.MulOP},
            {"/",OpCode.DivOP},
            {"==",OpCode.EqualOP},
            {"!=",OpCode.NotEqual},
            {">",OpCode.GreaterEqual},
            {">=",OpCode.GreaterEqual},
            {"<",OpCode.Less},
            {"<=",OpCode.LessEqual},
            {"!",OpCode.LogNot},
            {"&&",OpCode.LogAnd},
            {"||",OpCode.LogOr},
            //{"-",OpCode.Op_Neg},
            {"(",OpCode.LeftBracket},
            {")",OpCode.RightBracket},
            {"command",OpCode.Trigger_command},
            {"Anim",OpCode.Trigger_Anim},
            {"velx",OpCode.Trigger_velx},
            {"Time",OpCode.Trigger_StateTime},
            // to do
        };

        public static OpCode GetOpCode(string value) {
            if (OpcodeStrIdMap.ContainsKey(value))
            {
                return OpcodeStrIdMap[value];
            }
            else
            {
                return OpCode.None;
            }
        }

    }
}