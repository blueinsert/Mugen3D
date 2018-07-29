using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
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

    public class Token
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
