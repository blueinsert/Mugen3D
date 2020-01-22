using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateJumpStart:StateBase
    {
        int m_moveDir = 0;

        public override void OnEnter(Entity e)
        {
            base.OnEnter(e);
           
            var anim = e.GetComponent<AnimationComponent>();
            anim.ChangeAnim(40);
            var command = e.GetComponent<CommandComponent>();
            if (command.CommandIsActive("holdfwd"))
            {
                m_moveDir = 1;
            }else if (command.CommandIsActive("holdback"))
            {
                m_moveDir = -1;
            }
            else
            {
                m_moveDir = 0;
            }
            var basic = e.GetComponent<BasicInfoComponent>();
            basic.SetCtrl(false);
        }

        public override void OnExit(Entity e)
        {
            base.OnExit(e);
            var physics = e.GetComponent<PhysicsComponent>();
            physics.SetPhysicsType(PhysicsType.Air);
            var move = e.GetComponent<MoveComponent>();
            move.VelSet(m_moveDir*3, 8);
        }

        public override void OnUpdate(Entity e)
        {
            base.OnUpdate(e);
            var anim = e.GetComponent<AnimationComponent>();
            if (anim.LeftAnimTime <= 0)
            {
                var fsm = e.GetComponent<FSMComponent>();
                fsm.ChangeState(e, StateBase.StateNo_JumpUp);
            }
        }
    }
}
