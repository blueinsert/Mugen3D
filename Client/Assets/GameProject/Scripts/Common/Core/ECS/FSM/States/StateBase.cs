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
        public const int StateNo_Stand = 0;
        public const int StateNo_Walk = 1;
        public const int StateNo_JumpStart = 2;
        public const int StateNo_JumpUp = 3;
        public const int StateNo_JumpDown = 4;
        public const int StateNo_JumpLand = 5;

        protected Entity m_entity;

        public StateBase(Entity e)
        {
            m_entity = e;
        }

        #region controllers
        protected void ChangeAnim(int animNo)
        {
            var anim = m_entity.GetComponent<AnimationComponent>();
            if(anim!=null)
                anim.ChangeAnim(animNo);
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
