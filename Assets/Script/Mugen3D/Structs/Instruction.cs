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
        public string strValue;

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("opCode:").Append(opCode.ToString()).Append(",");
            sb.Append("value:").Append(value).Append(",");
            sb.Append("strValue:").Append(strValue);
            sb.Append("}");
            return sb.ToString();
        }

        public Instruction(OpCode code, double v, string sv)
        {
            opCode = code;
            value = v;
            strValue = sv;
        }

        public Instruction(OpCode code)
        {
            opCode = code;
            value = (int)code;
            strValue = code.ToString();
        }

        public Instruction(Token token)
        {
            if (token.type == TokenType.Num)
            {
                opCode = OpCode.PushValue;
                value = float.Parse(token.value);
                strValue = token.value;
            }
            else if (token.type == TokenType.Str)
            {
                opCode = OpCode.PushValue;
                value = token.value.GetHashCode();
                strValue = token.value;
            }
            else if(token.type == TokenType.Op)
            {
                opCode = TokenConfig.GetOpCode(token.value);
                value = (int)opCode;
                strValue = opCode.ToString();
            }
            else if (token.type == TokenType.VarName)
            {
                if ((opCode = TokenConfig.GetOpCode(token.value)) != OpCode.None)
                {
                    value = (int)opCode;
                    strValue = opCode.ToString();
                }
                else
                {

                }
            }
            else
            {
                Utility.Assert(false, "token type cannot be translated to Instruction");
            }
        }
    }
}
