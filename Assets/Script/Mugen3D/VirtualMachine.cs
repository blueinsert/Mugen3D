using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D
{
    public class VirtualMachine
    {
        private Stack<StackType> mStack = new Stack<StackType>();
        private StackType mPop;
        private Instruction mCurrIns;
        private Dictionary<OpCode, Action> pFuncTable = new Dictionary<OpCode, Action>();

        public VirtualMachine() {
            InitFuncTable();
        }

        public void InitFuncTable()
        {
            pFuncTable[OpCode.PushValue] = PushValue;
            pFuncTable[OpCode.PopValue] = PopValue;
            //+,-,*,/
            pFuncTable[OpCode.AddOP] = AddOP;
            pFuncTable[OpCode.SubOP] = SubOP;
            pFuncTable[OpCode.MulOP] = MulOP;
            pFuncTable[OpCode.DivOP] = DivOP;
            //==,!=
            pFuncTable[OpCode.EqualOP] = EqualOP;
            pFuncTable[OpCode.NotEqual] = NotEqual;
            //>,<,>=,<=
            pFuncTable[OpCode.Greater] = Greater;
            pFuncTable[OpCode.Less] = Less;
            pFuncTable[OpCode.GreaterEqual] = GreaterEqual;
            pFuncTable[OpCode.LessEqual] = LessEqual;

            //mugen sys trigger vars
            pFuncTable[OpCode.Trigger_command] = GetActiveCommand;
            pFuncTable[OpCode.Trigger_Anim] = GetAnim;
            pFuncTable[OpCode.Trigger_velx] = GetVelX;

            //todo
        }

        public double Execute(Instruction[] ins)
        {
            for (int i = 0; i < ins.Length; i++)
            {
                mCurrIns = ins[i];
                pFuncTable[ins[i].opCode]();
            }
            PopValue();
            mStack.Clear();
            return mPop.value;
        }

        public double Execute(Expression expression)
        {
            return Execute(expression.ints.ToArray());
        }

        #region op_functions
        void PushValue()
        {
            mStack.Push(new StackType(mCurrIns.value, mCurrIns.strValue));
        }

        void PopValue()
        {
            mPop = mStack.Pop();
        }

        void AddOP()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;
            mPop.value = tmp1 + tmp2;
            mStack.Push(new StackType(mPop.value, "#"));
        }

        void SubOP()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;
            mPop.value = tmp1 - tmp2;
            mStack.Push(new StackType(mPop.value, "#"));
        }

        void MulOP()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            mPop.value = tmp1 * tmp2;
            mStack.Push(new StackType(mPop.value, "#"));
        }

        void DivOP()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            mPop.value = tmp1 / tmp2;
            mStack.Push(new StackType(mPop.value, "#"));
        }

        //x==y
        void EqualOP()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;
            if (tmp1 == tmp2)
            {
                mPop.value = 1;
            }
            else
            {
                mPop.value = 0;
            }
            mStack.Push(new StackType(mPop.value, "#"));
        }

        //x!=y
        void NotEqual()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            if (tmp1 != tmp2)
            {
                mPop.value = 1;
            }
            else
            {
                mPop.value = 0;
            }
            mStack.Push(new StackType(mPop.value, "#"));
        }

        //x<y
        void Less()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            if (tmp1 < tmp2)
            {
                mPop.value = 1;
            }
            else
            {
                mPop.value = 0;
            }
            mStack.Push(new StackType(mPop.value, "#"));
        }

        //x>y
        void Greater()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            if (tmp1 > tmp2)
            {
                mPop.value = 1;
            }
            else
            {
                mPop.value = 0;
            }
            mStack.Push(new StackType(mPop.value, "#"));
        }

        //x<=y
        void LessEqual()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            if (tmp1 <= tmp2)
            {
                mPop.value = 1;
            }
            else
            {
                mPop.value = 0;
            }
            mStack.Push(new StackType(mPop.value, "#"));
        }

        //x>=y
        void GreaterEqual()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            if (tmp1 >= tmp2)
            {
                mPop.value = 1;
            }
            else
            {
                mPop.value = 0;
            }
            mStack.Push(new StackType(mPop.value, "#"));
        }

        //!x
        void LogNot()
        {
            PopValue();
            double tmp1 = mPop.value;
            if ((tmp1 == 0))
            {
                mPop.value = 1;
            }
            else
            {
                mPop.value = 0;
            }
            mStack.Push(new StackType(mPop.value, "#"));
        }

        //x && y
        void LogAnd()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            if (tmp1 != 0 && tmp2 != 0)
            {
                mPop.value = 1;
            }
            else
            {
                mPop.value = 0;
            }
            mStack.Push(new StackType(mPop.value, "#"));
        }

        void LogOr()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            if (tmp1 != 0 || tmp2 != 0)
            {
                mPop.value = 1;
            }
            else
            {
                mPop.value = 0;
            }
            mStack.Push(new StackType(mPop.value, "#"));
        }
        #endregion

        #region Mugen sys interface
        #region solve trigger vars
        void GetActiveCommand() { 

        }

        void GetAnim() {

        }

        void GetVelX() { 

        }
        #endregion
        #endregion

    }
}
