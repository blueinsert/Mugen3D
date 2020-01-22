using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateWalk:StateBase
    {

        public override void OnEnter(Entity e)
        {
            base.OnEnter(e);
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
            var move = e.GetComponent<MoveComponent>();
            var anim = e.GetComponent<AnimationComponent>();
            if (command.CommandIsActive("holdup"))
            {
                fsm.ChangeState(e, StateBase.StateNo_JumpStart);
            }else if (command.CommandIsActive("holdfwd"))
            {
                move.VelSet(3, 0);
                if(anim.Anim!=20)
                    anim.ChangeAnim(20);
            }
            else if (command.CommandIsActive("holdback"))
            {
                move.VelSet(-3, 0);
                if(anim.Anim!=21)
                    anim.ChangeAnim(21);
            }else
            {
                fsm.ChangeState(e, StateBase.StateNo_Stand);
            }
        }
    }
}
