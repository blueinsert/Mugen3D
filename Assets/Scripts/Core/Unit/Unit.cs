using System;
using System.Collections;
using System.Collections.Generic;
using XLua;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public enum MoveType
    {
        Attack = 1,
        Idle,
        Defence,
        BeingHitted,
    }

    public enum PhysicsType
    {
        N = -1,
        S = 1,
        C,
        A,
    }

    public class Status
    {
        public MoveType moveType = MoveType.Idle;
        public PhysicsType physicsType = PhysicsType.S;
        public bool pushTest = true;
        public bool ctrl = true;
        public int animNo = -1;
    }

    public class HitDef
    {
        public int attackType;
        public int hitDamage;
        public int guardDamage;
        public int hitPauseTimeP1;
        public int hitPauseTimeP2;
        public int hitSlideTime;
        public int guardPauseTimeP1;
        public int guardPauseTimeP2;
        public int guardSlideTime;
        public Vector groundVel;
        public Vector airVel;
    }

    public abstract class Unit : Entity
    {
        
        public AnimationController animCtr;
        public CmdManager cmdMgr;
        public FsmManager fsmMgr;
        public MoveCtrl moveCtr;
       
        public Status status = new Status();
        public HitDef hitDefData {get; private set;}
        public int facing = 1;
        private int pauseTime = 0;
        private int input;

        public Unit()
        {
        }

        public override void OnUpdate(Number deltaTime)
        {
            if (IsPause())
            {
                pauseTime--;
                return;
            }
            fsmMgr.Update();
            moveCtr.Update(deltaTime);
            animCtr.Update();
            cmdMgr.Update(input);
        }

        public void UpdateInput(int input)
        {
            this.input = input;
        }
   
        public void ChangeFacing(int facing)
        {
            this.facing = facing;
            this.scale = new Vector(Math.Abs(scale.x)*facing, scale.y, scale.z);
        }

        public bool IsPause()
        {
            return pauseTime > 0;
        }

        public void Pause(int duration)
        {
            pauseTime = duration;
        }

        public void ChangeAnim(int animNo)
        {
           
            this.animCtr.ChangeAnim(animNo);
        }

        public void SetHitDefData(HitDef hitDef)
        {
            this.hitDefData = hitDef;
        }

    }
}
