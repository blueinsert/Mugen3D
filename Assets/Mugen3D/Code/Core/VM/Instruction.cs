using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Mugen3D;

namespace Mugen3D
{
    public class Instruction
    {
        public OpCode opCode;
        public double value;

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("opCode:").Append(opCode.ToString()).Append(",");
            sb.Append("value:").Append(value);
            sb.Append("}");
            return sb.ToString();
        }

        public Instruction(OpCode code, double v)
        {
            opCode = code;
            value = v;
        }

        public Instruction(Token token)
        {
            switch (token.type)
            {
                case TokenType.Num:
                    opCode = OpCode.PushValue;
                    value = float.Parse(token.value);
                    break;
                case TokenType.Str:
                    opCode = OpCode.PushValue;
                    value = token.value.GetHashCode();
                    break;
                case TokenType.Op_Sub:
                    opCode = OpCode.SubOP;
                    value = 0;
                    break;
                case TokenType.Op_Neg:
                    opCode = OpCode.Neg;
                    value = 0;
                    break;
                default:
                    opCode = OpcodeConfig.GetOpcodeByStr(token.value);
                    value = 0;
                    break;
            }    
        }

    }//class
}//namespace
