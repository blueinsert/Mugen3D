using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public enum TokenType
    {
        Str,
        VarName,
        Op,
        Op_Neg,
        Op_Sub,
        Num,
        NewLine,
    }

    public struct Token
    {
        public string value;
        public TokenType type;

        public Token(string v, TokenType t)
        {
            value = v;
            type = t; 
        }

        public override string ToString()
        {
            return value;
        }
        
    }

}
