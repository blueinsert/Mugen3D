﻿using System;
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
        
        public int[] hitPauseTime;
        public int hitSlideTime;
        public Number[] groundVel;
        public Number[] airVel;

        public int guardDamage;
        public int[] guardPauseTime;
        public int guardSlideTime;
        public Number[] guardVel;

        public Number groundCornerPush;
        public Number airCornerPush;
        
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

    }

    public class HitInfo
    {
        public int hitType;
        public int hitFlag;

        public HitInfo(int hitType, int hitFlag)
        {
            this.hitType = hitType;
            this.hitFlag = hitFlag;
        }
    }

    public class HitChecker
    {
        protected List<HitInfo> m_infos;
        protected int m_duration;
        protected int m_lifeTime;

        public void Update()
        {
            m_lifeTime++;
        }

        public bool IsActive()
        {
            return m_lifeTime <= m_duration;
        }

        public HitChecker(List<HitInfo> infos, int duration)
        {
            this.m_infos = infos;
            this.m_duration = duration;
            this.m_lifeTime = 0;
        }

        public virtual bool Check(HitDef hitDef)
        {
            return false;
        }
    }

    public class HitBy : HitChecker
    {
        public HitBy(List<HitInfo> infos, int duration) : base(infos, duration) { 
        }

        //只有指定的Hit能够通过
        public override bool Check(HitDef hitDef)
        {
            if (!IsActive())
                return true;
            foreach (var hitInfo in this.m_infos)
            {
                if ((hitInfo.hitFlag & hitDef.hitFlag) != 0 && hitInfo.hitType == hitDef.hitType)
                    return true;
            }
            return false;
        }
    }

    public class NoHitBy : HitChecker
    {
        public NoHitBy(List<HitInfo> infos, int duration)
            : base(infos, duration)
        {

        }

        //对指定的hit进行拦截
        public override bool Check(HitDef hitDef)
        {
            if (!IsActive())
                return false;
            foreach (var hitInfo in this.m_infos)
            {
                if ((hitInfo.hitFlag & hitDef.hitFlag) != 0 && hitInfo.hitType == hitDef.hitType)
                    return true;
            }
            return false;
        }
    }

    public class Unit : Entity, IHealth
    {
        public MoveCtrl moveCtr { get; protected set; }
        public AnimationController animCtr { get; protected set; }  
        public FsmManager fsmMgr { get; protected set; }

        private Status status = new Status();
        private HitBy hitBy;
        private NoHitBy noHitBy;

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
            moveCtr.Update(deltaTime);
            animCtr.Update();
            fsmMgr.Update();
            if (hitBy != null)
                hitBy.Update();
            if (noHitBy != null)
                noHitBy.Update();
        }        

        #region status get/set
        public Number GetFrontEdgeDist()
        {
            if (this.GetFacing() > 0)
            {
                return world.config.stageConfig.borderXMax - this.position.x;
            }
            else
            {
                return this.position.x - world.config.stageConfig.borderXMin;
            }
        }

        public Number GetBackEdgeDist()
        {
            if (this.GetFacing() > 0)
            {
                return this.position.x - world.config.stageConfig.borderXMin;  
            }
            else
            {
                return world.config.stageConfig.borderXMax - this.position.x;
            }
        }

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

        public void SetHitBy(List<HitInfo> infos, int duration)
        {
            this.hitBy = new HitBy(infos, duration);
        }

        public void SetNoHitBy(List<HitInfo> infos, int duration)
        {
            this.noHitBy = new NoHitBy(infos, duration);
        }


        public bool CanBeHit(HitDef hitDef)
        {
            if (hitBy != null && !hitBy.Check(hitDef))
                return false;
            if (noHitBy != null && noHitBy.Check(hitDef))
                return false;
            int hitFlag = hitDef.hitFlag;
            if ((hitFlag & (int)HitFlag.H) != 0)
            {
                if (GetPhysicsType() == PhysicsType.S)
                    return true;
            }
            if ((hitFlag & (int)HitFlag.L) != 0)
            {
                if (GetPhysicsType() == PhysicsType.C)
                    return true;
            }
            if ((hitFlag & (int)HitFlag.A) != 0)
            {
                if (GetPhysicsType() == PhysicsType.A && (this.fsmMgr.stateNo != 5050))
                    return true;
            }
            if ((hitFlag & (int)HitFlag.F) != 0)
            {
                if (GetPhysicsType() == PhysicsType.A && (fsmMgr.stateNo == 5050))
                    return true;
            }
            if ((hitFlag & (int)HitFlag.D) != 0)
            {
            }
            return false;
        }

        public bool CanBeGuard(HitDef hitDef)
        {
            int guardFlag = hitDef.guardFlag;
            if ((guardFlag & (int)GuardFlag.H) != 0)
            {
                if (GetPhysicsType() == PhysicsType.S)
                {
                    return true;
                }
            }
            if ((guardFlag & (int)GuardFlag.L) != 0)
            {
                if (GetPhysicsType() == PhysicsType.C)
                {
                    return true;
                }
            }
            return false;
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
