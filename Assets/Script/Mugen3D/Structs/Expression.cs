using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Expression
    {
        public MyList<Instruction> ints = new MyList<Instruction>();

        public override string ToString()
        {
            string s = ints.ToString();
            return s;
        }

        public Expression(List<Token> tokens, bool isPostfix)
        {
            if (isPostfix == false)
            {
                tokens = Infix2PostFix(tokens);
            }
            ints = new MyList<Instruction>();
            for (int i = 0; i < tokens.Count; i++)
            {
                ints.Add(new Instruction(tokens[i]));
            }
        }

        public List<Token> Infix2PostFix(List<Token> tokens)
        {
            Stack<Token> opStack = new Stack<Token>();
            List<Token> result = new List<Token>();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.Num || tokens[i].type == TokenType.Str || tokens[i].type == TokenType.VarName)
                {
                    result.Add(tokens[i]);
                }
                else if (tokens[i].type == TokenType.Op)
                {
                    OpCode opcode = TokenConfig.GetOpCode(tokens[i].value);
                    if (opcode == OpCode.LeftBracket)
                    {
                        opStack.Push(tokens[i]);
                    }
                    else if (opcode == OpCode.RightBracket)
                    {
                        while (TokenConfig.GetOpCode(opStack.Peek().value) != OpCode.LeftBracket)
                        {
                            result.Add(opStack.Pop());
                        }
                        opStack.Pop();
                    }
                    else
                    {
                        while (opStack.Count != 0 && TokenConfig.GetOpCode(opStack.Peek().value) != OpCode.LeftBracket && TokenConfig.GetOpCodePriority(TokenConfig.GetOpCode(opStack.Peek().value)) < TokenConfig.GetOpCodePriority(opcode))
                        {
                            result.Add(opStack.Pop());
                        }
                        opStack.Push(tokens[i]);
                    }
                }
            }
            while (opStack.Count != 0)
            {
                result.Add(opStack.Pop());
            }
            return result;
        }
    }
}
