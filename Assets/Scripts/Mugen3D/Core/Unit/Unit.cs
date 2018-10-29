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

    public enum GuardFlag { 
        H = 1 << 0,
        L = 1 << 1,
    }

    public enum HitFlag
    {
        H = 1 << 0,
        L = 1 << 1,
        A = 1 << 2,
        F = 1 << 3,
        D = 1 << 4,
    }

    public class HitDef
    {
        public int hitFlag;
        public int guardFlag;
        public int hitType;
        public int knockBackType;
        public int knockBackForceLevel;
        public int knockAwayType;
        public int hitDamage;
        public int guardDamage;
        public int[] hitPauseTime;
        public int hitSlideTime;
        public int[] guardPauseTime;
        public int guardSlideTime;
        public Vector groundVel;
        public Vector airVel;

        public HitDef()
        {
        }
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
        public int facing = 1;
        public int pauseTime;
        public HitDef hitDefData;
        public HitDef beHitDefData;

        public void Clear()
        {

        }
    }

    public class Unit : Entity, IHealth
    {
        public MoveCtrl moveCtr { get; protected set; }
        public AnimationController animCtr { get; protected set; }  
        public FsmManager fsmMgr { get; protected set; }

        private Status status = new Status();

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
                this.status.pauseTime--;
                return;
            }
            this.status.Clear();
            moveCtr.Update(deltaTime);
            animCtr.Update();
            fsmMgr.Update();   
        }        

        #region status get/set
        public Vector GetP2Dist()
        {
            Unit enemy = this.world.teamInfo.GetEnemy(this);
            var dist = enemy.position - this.position;
            return dist;
        }

        public bool IsPause()
        {
            return status.pauseTime > 0;
        }

        public void Pause(int duration)
        {
            status.pauseTime = duration;
        }

        public int GetPauseTime()
        {
            return status.pauseTime;
        }

        public int GetFacing()
        {
            return this.status.facing;
        }

        public void ChangeFacing(int facing)
        {
            this.status.facing = facing;
            SetScale(new Vector(Math.Abs(scale.x) * facing, scale.y, scale.z));
        }

        public void SetHitDefData(HitDef hitDef)
        {
            this.status.hitDefData = hitDef;
        }

        public HitDef GetHitDefData()
        {
            return this.status.hitDefData;
        }

        public void SetBeHitDefData(HitDef hitDef)
        {
            this.status.beHitDefData = hitDef;
        }

        public HitDef GetBeHitDefData()
        {
            return this.status.beHitDefData;
        }

        public void SetPhysicsType(PhysicsType type)
        {
            this.status.physicsType = type;
        }

        public PhysicsType GetPhysicsType()
        {
            return this.status.physicsType;
        }

        public void SetMoveType(MoveType type)
        {
            this.status.moveType = type;
        }

        public MoveType GetMoveType()
        {
            return this.status.moveType;
        }

        public void SetCtrl(bool ctrl)
        {
            this.status.ctrl = ctrl;
        }

        public bool CanCtrl()
        {
            return this.status.ctrl;
        }

        #endregion

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
