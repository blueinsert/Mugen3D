using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateStand : StateBase
    {

        public override void OnEnter(Entity e)
        {
            base.OnEnter(e);
            var physice = e.GetComponent<PhysicsComponent>();
            physice.SetPhysicsType(PhysicsType.Stand);
            var anim = e.GetComponent<AnimationComponent>();
            anim.ChangeAnim(0);
            var move = e.GetComponent<MoveComponent>();
            move.VelSet(0, 0);
            var basic = e.GetComponent<BasicInfoComponent>();
            basic.SetCtrl(true);
        }

        public override void OnExit(Entity e)
        {
            base.OnExit(e);
        }

        public override void OnUpdate(Entity e)
        {
            base.OnUpdate(e);
            var fsm = e.GetComponent<FSMComponent>();
            var command = e.GetComponent<CommandComponent>();
            if (command.CommandIsActive("holdfwd"))
            {
                fsm.ChangeState(e, StateBase.StateNo_Walk);
            }else if (command.CommandIsActive("holdback"))
            {
                fsm.ChangeState(e, StateBase.StateNo_Walk);
            }else if (command.CommandIsActive("holdup"))
            {
                fsm.ChangeState(e, StateBase.StateNo_JumpStart);
            }
        }
    }
}
