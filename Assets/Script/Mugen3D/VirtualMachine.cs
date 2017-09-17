using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D
{
    public class VirtualMachine
    {
        private Player mOwner;
        private Stack<StackType> mStack = new Stack<StackType>();
        private StackType mPop;
        private Instruction mCurrIns;
        private Dictionary<OpCode, Action> pFuncTable = new Dictionary<OpCode, Action>();

        public void SetOwner(Player p){
            mOwner = p;
        }
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
            //logic : or, and,
            pFuncTable[OpCode.LogOr] = LogOr;
            pFuncTable[OpCode.LogNot] = LogNot;
            pFuncTable[OpCode.LogAnd] = LogAnd;

            //trigger vars
            pFuncTable[OpCode.Trigger_Anim] = GetAnim;
            pFuncTable[OpCode.Trigger_AnimElem] = GetAnimElem;
            pFuncTable[OpCode.Trigger_AnimTime] = GetAnimTime;
            pFuncTable[OpCode.Trigger_LeftAnimElem] = GetLeftAnimTime;
            pFuncTable[OpCode.Trigger_Command] = GetActiveCommand;
            pFuncTable[OpCode.Trigger_VelX] = GetVelX;
            pFuncTable[OpCode.Trigger_VelY] = GetVelY;
            pFuncTable[OpCode.Trigger_PosX] = GetPosX;
            pFuncTable[OpCode.Trigger_PosY] = GetPosY;
            pFuncTable[OpCode.Trigger_StateNo] = GetStateNo;
            pFuncTable[OpCode.Trigger_PrevStateNo] = GetPrevStateNo;
            pFuncTable[OpCode.Trigger_StateTime] = GetStateTime;
            pFuncTable[OpCode.Trigger_DeltaTime] = GetDeltaTime;
            //todo
        }

        public double Execute(Instruction[] ins)
        {
            for (int i = 0; i < ins.Length; i++)
            {
                mCurrIns = ins[i];
                if (pFuncTable.ContainsKey(ins[i].opCode))
                    pFuncTable[ins[i].opCode]();
                else
                {
                    Debug.LogError("pFuncTable do not has key:" + ins[i].opCode);
                    Application.Quit();
                }
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
            mStack.Push(new StackType(mCurrIns.value));
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
            mStack.Push(new StackType(mPop.value));
        }

        void SubOP()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;
            mPop.value = tmp1 - tmp2;
            mStack.Push(new StackType(mPop.value));
        }

        void MulOP()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            mPop.value = tmp1 * tmp2;
            mStack.Push(new StackType(mPop.value));
        }

        void DivOP()
        {
            PopValue();
            double tmp2 = mPop.value;
            PopValue();
            double tmp1 = mPop.value;

            mPop.value = tmp1 / tmp2;
            mStack.Push(new StackType(mPop.value));
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
            mStack.Push(new StackType(mPop.value));
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
            mStack.Push(new StackType(mPop.value));
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
            mStack.Push(new StackType(mPop.value));
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
            mStack.Push(new StackType(mPop.value));
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
            mStack.Push(new StackType(mPop.value));
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
            mStack.Push(new StackType(mPop.value));
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
            mStack.Push(new StackType(mPop.value));
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
            mStack.Push(new StackType(mPop.value));
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
            mStack.Push(new StackType(mPop.value));
        }
        #endregion

        #region Mugen sys interface
        #region solve trigger vars

        void GetAnim() {
            string anim = Triggers.Instance.AnimName(this.mOwner);
            mStack.Push(new StackType(anim.GetHashCode()));

        }

        void GetAnimElem() {
            int elem = Triggers.Instance.AnimElem(this.mOwner);
            mStack.Push(new StackType(elem));
        }

        void GetAnimTime()
        {
            int animTime = Triggers.Instance.AnimTime(this.mOwner);
            mStack.Push(new StackType(animTime));
        }

        void GetLeftAnimTime()
        {
            int leftTime = Triggers.Instance.LeftAnimElem(this.mOwner);
            mStack.Push(new StackType(leftTime));
        }

        void GetActiveCommand()
        {
            string command = Triggers.Instance.Command(this.mOwner);
            mStack.Push(new StackType(command.GetHashCode()));
        }

        void GetPosX()
        {
            float posX = Triggers.Instance.PosX(this.mOwner);
            mStack.Push(new StackType(posX));
        }

        void GetPosY()
        {
            float posY = Triggers.Instance.PosY(this.mOwner);
            mStack.Push(new StackType(posY));
        }

        void GetVelX() {
            float velX = Triggers.Instance.VelX(this.mOwner);
            mStack.Push(new StackType(velX));
        }

        void GetVelY()
        {
            float velY = Triggers.Instance.VelY(this.mOwner);
            mStack.Push(new StackType(velY));
        }

        void GetStateNo()
        {
            int no = Triggers.Instance.StateNo(this.mOwner);
            mStack.Push(new StackType(no));
        }

        void GetPrevStateNo()
        {
            int no = Triggers.Instance.PrevStateNo(this.mOwner);
            mStack.Push(new StackType(no));
        }

        void GetStateTime()
        {
            int time = Triggers.Instance.Time(this.mOwner);
            mStack.Push(new StackType(time));
        }

        void GetDeltaTime()
        {
            float time = Triggers.Instance.DeltaTime();
            mStack.Push(new StackType(time));
        }
        #endregion
        #endregion

    }
}
