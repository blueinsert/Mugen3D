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
            if (token.type == TokenType.Num)
            {
                opCode = OpCode.PushValue;
                value = float.Parse(token.value);
            }
            else if (token.type == TokenType.Str)
            {
                opCode = OpCode.PushValue;
                value = token.value.GetHashCode();
            }
            else if(token.type == TokenType.Op)
            {
                opCode = TokenConfig.GetOpCode(token.value);
                value = 0;
            }
            else if (token.type == TokenType.VarName)
            {
                if ((opCode = TokenConfig.GetOpCode(token.value)) != OpCode.None)
                {
                    value = (int)opCode;
                }
                else
                {
                    Utility.Assert(false, token.value +":token varname cannot be translated to Instruction");
                }
            }
            else
            {
                Utility.Assert(false, token.type + ":token type cannot be translated to Instruction");
            }
        }
    }
}
