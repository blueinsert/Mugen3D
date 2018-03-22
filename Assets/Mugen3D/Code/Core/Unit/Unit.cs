using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Mugen3D
{
    [RequireComponent(typeof(DecisionBoxes))]
    public abstract class Unit : Entity
    {
        public TextAsset commandFile;

        public XLua.LuaTable fsm;
        public delegate void DegateFsmInit(XLua.LuaTable self, Unit u);
        public delegate void DelegateFsmUpdate(XLua.LuaTable self);
        public delegate void DelegateFsmChangeState(XLua.LuaTable self);
        private DegateFsmInit funFsmInit;
        private DelegateFsmUpdate funcFsmUpdate;
        private DelegateFsmChangeState funFsmChangeState;

        public AnimationController animCtr;
        public CmdManager cmdMgr;
        public MoveCtrl moveCtr;

        [HideInInspector]
        public DecisionBoxes decisionBoxes;

        public Status status = new Status();

        [HideInInspector]
        public int facing = 1;

        private int pauseTime = 0;

        public override void Init()
        {
            decisionBoxes = this.GetComponent<DecisionBoxes>();
            decisionBoxes.SetOwner(this);
            decisionBoxes.Init();
        }

        public void SetFSM(XLua.LuaTable fsm)
        {
            this.fsm = fsm;
            funcFsmUpdate = this.fsm.Get<string, DelegateFsmUpdate>("update");
            funFsmChangeState = this.fsm.Get<string, DelegateFsmChangeState>("changeState");
            funFsmInit = this.fsm.Get<string, DegateFsmInit>("init");
            funFsmInit(this.fsm, this);
        }

        public override Collider GetCollider()
        {
            return decisionBoxes.GetCollideBox();
        }

        public override void OnUpdate()
        {
            if (funcFsmUpdate != null)
            {
                funcFsmUpdate(this.fsm);
            }
        }
   
        public void ChangeFacing(int facing)
        {
            if (this.facing != facing)
            {
                this.facing = facing;
                var scale = this.transform.localScale;
                this.transform.localScale = new Vector3(scale.x, scale.y, Mathf.Abs(scale.z)*facing);
            }
        }

        public bool IsPause()
        {
            return pauseTime > 0;
        }

        public void Pause(int duration)
        {
            pauseTime = duration;
        }

        public void ChangeState(int stateNo, System.Action onExit)
        {

        }

    }
}
