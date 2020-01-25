using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixPointMath;

namespace bluebean.Mugen3D.Core {
    /// <summary>
    /// 角色状态基类
    /// </summary>
    public class StateBase
    {
        protected Entity m_entity;

        public StateBase(Entity e)
        {
            m_entity = e;
        }

        #region controllers
        protected void ChangeAnim(int animNo,int animElem = 0)
        {
            var anim = m_entity.GetComponent<AnimationComponent>();
            if(anim!=null)
                anim.ChangeAnim(animNo, animElem);
        }

        protected void PosSet(Number x, Number y)
        {
            var transform = m_entity.GetComponent<TransformComponent>();
            if (transform != null)
            {
                transform.PosSet(x, y);
            }
        }

        protected void VelSet(Number x,Number y)
        {
            var move = m_entity.GetComponent<MoveComponent>();
            if (move != null)
                move.VelSet(x, y);
        }

        protected void CtrlSet(bool ctrl)
        {
            var basic = m_entity.GetComponent<BasicInfoComponent>();
            basic.SetCtrl(ctrl);
        }

        protected void PushStateLayer(int stateNo)
        {
            var fsm = m_entity.GetComponent<FSMComponent>();
            if (fsm != null)
                fsm.PushLayer(stateNo);
        }

        protected void PopStateLayer()
        {
            var fsm = m_entity.GetComponent<FSMComponent>();
            if (fsm != null)
                fsm.PopLayer();
        }

        protected void ChangeState(int stateNo)
        {
            var fsm = m_entity.GetComponent<FSMComponent>();
            if (fsm != null)
                fsm.ChangeState(stateNo);
        }

        protected void PhysicsSet(PhysicsType physicsType)
        {
            var physics = m_entity.GetComponent<PhysicsComponent>();
            if (physics != null)
            {
                physics.SetPhysicsType(physicsType);
            }
        }

        protected  void SetHitDefData(HitDefData hitdef, int duration)
        {
            var hit = m_entity.GetComponent<HitComponent>();
            if (hit != null)
            {
                hit.SetHitDef(hitdef, duration);
            }
        }

        protected void MoveTypeSet(MoveType moveType)
        {
            var basic = m_entity.GetComponent<BasicInfoComponent>();
            if (basic != null)
            {
                basic.MoveTypeSet(moveType);
            }
        }

        #endregion

        #region triggers
        
        protected bool CommandIsActive(string commandName)
        {
            var command = m_entity.GetComponent<CommandComponent>();
            if (command != null)
                return command.CommandIsActive(commandName);
            return false;
        }

        protected int StateNo { 
            get
            {
                var fsm = m_entity.GetComponent<FSMComponent>();
                if (fsm != null)
                    return fsm.StateNo;
                return 0;
            } 
        }

        protected int StateTime
        {
            get
            {
                var fsm = m_entity.GetComponent<FSMComponent>();
                if (fsm != null)
                    return fsm.StateTime;
                return 0;
            }
        }

        protected int Anim
        {
            get
            {
                var anim = m_entity.GetComponent<AnimationComponent>();
                if (anim != null)
                    return anim.Anim;
                return 0;
            }
        }

        protected int AnimElem
        {
            get
            {
                var anim = m_entity.GetComponent<AnimationComponent>();
                if (anim != null)
                    return anim.AnimElem;
                return 0;
            }
        }

        protected int LeftAnimTime
        {
            get
            {
                var anim = m_entity.GetComponent<AnimationComponent>();
                if (anim != null)
                    return anim.LeftAnimTime;
                return 0;
            }
        }

        protected Vector Pos
        {
            get
            {
                var transform = m_entity.GetComponent<TransformComponent>();
                if (transform != null)
                {
                    return transform.Position;
                }
                return Vector.zero;
            }
        }

        protected Vector Vel
        {
            get
            {
                var move = m_entity.GetComponent<MoveComponent>();
                if (move != null)
                    return move.Velocity;
                return Vector.zero;
            }
        }

        protected Vector Acceler
        {
            get
            {
                var move = m_entity.GetComponent<MoveComponent>();
                if (move != null)
                    return move.Acceler;
                return Vector.zero;
            }
        }

        protected HitDefData HitData
        {
            get
            {
                var hit = m_entity.GetComponent<HitComponent>();
                return hit.HitDef;   
            }
        }

        protected bool MoveContact
        {
            get
            {
                var hit = m_entity.GetComponent<HitComponent>();
                if (hit != null)
                    return hit.MoveContact;
                return false;
            }
        }

        protected bool MoveHit
        {
            get
            {
                var hit = m_entity.GetComponent<HitComponent>();
                if (hit != null)
                    return hit.MoveHit;
                return false;
            }
        }

        protected bool MoveGuarded
        {
            get
            {
                var hit = m_entity.GetComponent<HitComponent>();
                if (hit != null)
                    return hit.MoveGuarded;
                return false;
            }
        }

        protected HitDefData BeHitData
        {
            get
            {
                var hit = m_entity.GetComponent<HitComponent>();
                return hit.BeHitData;
            }
        }

        /// <summary>
        /// 打击震动时间
        /// </summary>
        /// <returns></returns>
        protected int HitShakeTime
        {
            get
            {
                var hit = m_entity.GetComponent<HitComponent>();
                if (hit != null)
                {
                    return hit.BeHitData.hitPauseTime[1];
                }
                return 0;
            }
        }

        /// <summary>
        /// 打击滑行时间
        /// </summary>
        protected int HitSlideTime
        {
            get
            {
                var hit = m_entity.GetComponent<HitComponent>();
                if (hit != null)
                {  
                    return hit.BeHitData.hitSlideTime;   
                }
                return 0;
            }
        }

        /// <summary>
        /// 打击震动时间
        /// </summary>
        /// <returns></returns>
        protected int GuardShakeTime
        {
            get
            {
                var hit = m_entity.GetComponent<HitComponent>();
                if (hit != null)
                {
                    return hit.BeHitData.guardPauseTime[1];
                }
                return 0;
            }
        }

        /// <summary>
        /// 打击滑行时间
        /// </summary>
        protected int GuardSlideTime
        {
            get
            {
                var hit = m_entity.GetComponent<HitComponent>();
                if (hit != null)
                {
                    return hit.BeHitData.guardSlideTime;
                }
                return 0;
            }
        }

        protected MoveType MoveType
        {
            get
            {
                var basic = m_entity.GetComponent<BasicInfoComponent>();
                if (basic != null)
                    return basic.MoveType;
                return MoveType.Idle;
            }
        }

        protected int P2StateNo
        {
            get
            {
                return UtilityFuncs.GetP2StateNo(m_entity);
            }
        }

        protected MoveType P2MoveType
        {
            get
            {
                return UtilityFuncs.GetP2MoveType(m_entity);
            }
        }

        protected Vector P2Dist
        {
            get
            {
                return UtilityFuncs.GetP2Dist(m_entity);
            }
        }

        #endregion
        public virtual void OnEnter() { 
        }

        public virtual void OnExit() { 
        }

        public virtual void OnUpdate()
        {

        }
    }
}
