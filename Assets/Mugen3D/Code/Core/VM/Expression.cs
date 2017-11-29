using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Expression
    {
        public List<Instruction> ints = new List<Instruction>();

        public override string ToString()
        {
            string s = ints.ToString();
            return s;
        }

        public Expression(List<Token> tokens, bool isPostfix = false)
        {
            ints = new List<Instruction>();
            for (int i = 0; i < tokens.Count; i++)
            {
                ints.Add(new Instruction(tokens[i]));
            }
            if (isPostfix == false)
            {
                ints = Infix2PostFix(ints);
            }
            
        }

        private List<Instruction> Infix2PostFix(List<Instruction> ints)
        {
            Stack<Instruction> opStack = new Stack<Instruction>();
            List<Instruction> result = new List<Instruction>();
            for (int i = 0; i < ints.Count; i++)
            {
                var curInts = ints[i];
                var curOpDetail = OpcodeConfig.GetDetail(curInts.opCode);
                if (curOpDetail.opcode == OpCode.PushValue || (curOpDetail.isMugenBuildIn && curOpDetail.inputNum == 0))
                {
                    result.Add(curInts);
                }
                else
                {
                    OpCode opcode = curOpDetail.opcode;
                    if (opcode == OpCode.LeftBracket)
                    {
                        opStack.Push(curInts);
                    }
                    else if (opcode == OpCode.RightBracket)
                    {
                        while (opStack.Peek().opCode != OpCode.LeftBracket)
                        {
                            result.Add(opStack.Pop());
                        }
                        opStack.Pop();
                    }
                    else
                    {
                        while (opStack.Count != 0 && opStack.Peek().opCode != OpCode.LeftBracket && OpcodeConfig.GetDetail(opStack.Peek().opCode).priority < curOpDetail.priority)
                        {
                            result.Add(opStack.Pop());
                        }
                        opStack.Push(curInts);
                    }
                }
            }
            while (opStack.Count != 0)
            {
                result.Add(opStack.Pop());
            }
            return result;
        }

        public double CalcValueInRunTime()
        {
            VirtualMachine vm = new VirtualMachine();
            return vm.Execute(this);
        }
    }
}
