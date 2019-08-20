using System;
using System.Collections;
using System.Collections.Generic;
using FixPointMath;
using Math = FixPointMath.Math;

namespace bluebean.Mugen3D.Core
{

    public class Status
    {
        public bool pushTest = true;
        public bool ctrl = true;
        public int pauseTime;
    }


    public class Unit : Entity, IHealth
    {
        
        private Status status = new Status();

        public Unit(UnitConfig config)
        {
            SetConfig(config);
            //moveCtr = new MoveCtrl(this);
            //animCtr = new AnimationController(config.actions, this);
            //fsmMgr = new FsmManager(config.fsm, this);   
        }

        protected Unit() { }

        public override void OnUpdate()
        {
            if (IsPause())
                return;
            if (hitBy != null)
                hitBy.Update();
            if (noHitBy != null)
                noHitBy.Update();
        }

        public override void SetPosition(Vector pos)
        {
            base.SetPosition(pos);
            this.moveCtr.PosSet(pos);
        }

        #region status get/set
        public Number GetFrontEdgeDist()
        {
            if (this.GetFacing() > 0)
            {
                return world.cameraController.viewPort.xMax - this.Position.x;
            }
            else
            {
                return this.Position.x - world.cameraController.viewPort.xMin;
            }
        }

        public Number GetBackEdgeDist()
        {
            if (this.GetFacing() > 0)
            {
                return this.Position.x - world.cameraController.viewPort.xMin;
            }
            else
            {
                return world.cameraController.viewPort.xMax - this.Position.x;
            }
        }

        public Number GetFrontStageDist()
        {
            if (this.GetFacing() > 0)
            {
                return world.config.stageConfig.borderXMax - this.Position.x;
            }
            else
            {
                return this.Position.x - world.config.stageConfig.borderXMin;
            }
        }

        public Number GetBackStageDist()
        {
            if (this.GetFacing() > 0)
            {
                return this.Position.x - world.config.stageConfig.borderXMin;  
            }
            else
            {
                return world.config.stageConfig.borderXMax - this.Position.x;
            }
        }

        public Vector GetP2Dist()
        {
            Unit enemy = this.world.teamInfo.GetEnemy(this);
            var dist = enemy.Position - this.Position;
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

        public void AddPauseTime(int delta)
        {
            status.pauseTime += delta;
        }

        public int GetFacing()
        {
            return this.status.facing;
        }

        public void ChangeFacing(int facing)
        {
            this.status.facing = facing;
            SetScale(new Vector(Math.Abs(scale.x) * facing, scale.y));
        }

        public void SetHitDefData(HitDef hitDef)
        {
            this.status.hitDefData = hitDef;
            this.status.hitDefData.owner = this;
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
            if ((hitFlag & (int)HitDef.HitFlag.H) != 0)
            {
                if (GetPhysicsType() == PhysicsType.S)
                    return true;
            }
            if ((hitFlag & (int)HitDef.HitFlag.L) != 0)
            {
                if (GetPhysicsType() == PhysicsType.C)
                    return true;
            }
            if ((hitFlag & (int)HitDef.HitFlag.A) != 0)
            {
                if (GetPhysicsType() == PhysicsType.A && (this.fsmMgr.stateNo != 5050))
                    return true;
            }
            if ((hitFlag & (int)HitDef.HitFlag.F) != 0)
            {
                if (GetPhysicsType() == PhysicsType.A && (fsmMgr.stateNo == 5050))
                    return true;
            }
            if ((hitFlag & (int)HitDef.HitFlag.D) != 0)
            {
            }
            return false;
        }

        public bool CanBeGuard(HitDef hitDef)
        {
            int guardFlag = hitDef.guardFlag;
            if ((guardFlag & (int)HitDef.GuardFlag.H) != 0)
            {
                if (GetPhysicsType() == PhysicsType.S)
                {
                    return true;
                }
            }
            if ((guardFlag & (int)HitDef.GuardFlag.L) != 0)
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

        public int GetMaxHP()
        {
            return m_maxHP;
        }

        public void SetMaxMP(int hp)
        {
            m_maxHP = hp;
        }

        public bool IsAlive()
        {
            return m_hp >= 0;
        }

        public int beHitCount { get; protected set; }

        protected void AddBeHitCount()
        {
            this.beHitCount++;
        }

        protected void ClearBeHitCount()
        {
            this.beHitCount = 0;
        }

        public int hitCount { get; private set; }
        public void SetHitCount(int hitCount)
        {
            this.hitCount = hitCount;
            SendEvent(new Event() { type = EventType.HitCountChange, data = this.hitCount });
        }

        public virtual void PrintDebugInfo()
        {
            Core.Debug.AddGUIDebugMsg(this.id, "roundState",this.world.RoundState.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "roundNo", this.world.RoundNo.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "matchNo", this.world.BattleNo.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "name", this.config.name);
            Core.Debug.AddGUIDebugMsg(this.id, "pos", this.Position.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "stateNo", this.fsmMgr.stateNo.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "stateTime", this.fsmMgr.stateTime.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "anim", this.animCtr.anim.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "animTime", this.animCtr.animTime.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "animElem", this.animCtr.animElem.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "animElemTime", this.animCtr.animElemTime.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "leftAnimTime", this.animCtr.leftAnimTime.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "facing", this.GetFacing().ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "vel", this.moveCtr.velocity.ToString());     
            Core.Debug.AddGUIDebugMsg(this.id, "isPause", this.IsPause().ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "pauseTime", this.GetPauseTime().ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "p2dist", this.GetP2Dist().ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "hp", this.GetHP().ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "beHitCount",this.beHitCount.ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "stageDist", this.GetBackStageDist().ToString() + "," + this.GetFrontStageDist().ToString());
            Core.Debug.AddGUIDebugMsg(this.id, "edgeDist", this.GetBackEdgeDist().ToString() + "," + this.GetFrontEdgeDist().ToString());
        } 
    }
}
