using System;
using System.Collections;
using System.Collections.Generic;
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
        public bool moveHit = false;
        public bool moveGuard = false;
        public bool moveContact { get { return moveHit || moveGuard; } }
    }

    public class HitDef
    {
        public int id;
        public int attackType;
        public int hitDamage;
        public int guardDamage;
        public int[] hitPauseTime;
        public int hitSlideTime;
        public int[] guardPauseTime;
        public int guardSlideTime;
        public Vector groundVel;
        public Vector airVel;
    }

    public abstract class Unit : Entity, IHealth
    {
        public MoveCtrl moveCtr { get; protected set; }
        public AnimationController animCtr { get; protected set; }  
        public FsmManager fsmMgr { get; protected set; }
       
        public Status status = new Status();
        public HitDef hitDefData {get; private set;}
        public HitDef beHitDefData { get; private set; }

        public int facing = 1;
        public int pauseTime { get; private set; } 

        public Unit(UnitConfig config)
        {
            SetConfig(config);
            moveCtr = new MoveCtrl(this);
            animCtr = new AnimationController(config.actions, this);
            fsmMgr = new FsmManager(config.fsm, this);
        }

        public override void OnUpdate(Number deltaTime)
        {
            if (IsPause())
            {
                pauseTime--;
                return;
            }
            moveCtr.Update(deltaTime);
            animCtr.Update();
            fsmMgr.Update();   
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

        public void SetHitDefData(HitDef hitDef)
        {
            this.hitDefData = hitDef;
        }

        public void SetBeHitDefData(HitDef hitDef)
        {
            this.beHitDefData = hitDef;
        }

        private int m_maxHP = 100;
        private int m_hp = 100;

        public int GetHP()
        {
            return m_hp;
        }

        public int GetMaxHP()
        {
            return m_maxHP;
        }

        public void AddHP(int hpAdd)
        {
            m_hp += hpAdd;
            if (m_hp <= 0)
            {
                SendEvent(new Event { type = EventType.Dead });
            }
        }

        public void SetHP(int hp)
        {
            m_hp = hp;
        }
    }
}
