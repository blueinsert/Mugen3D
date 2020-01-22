using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateJumpDown : StateBase
    {

        public override void OnEnter(Entity e)
        {
            base.OnEnter(e);
            var anim = e.GetComponent<AnimationComponent>();
            anim.ChangeAnim(44);
            var basic = e.GetComponent<BasicInfoComponent>();
            basic.SetCtrl(false);
        }

        public override void OnExit(Entity e)
        {
            base.OnExit(e);
        }

        public override void OnUpdate(Entity e)
        {
            base.OnUpdate(e);
            var move = e.GetComponent<MoveComponent>();
            if(move.Position.y + move.Velocity.y*Time.deltaTime <= 0)
            {
                var fsm = e.GetComponent<FSMComponent>();
                fsm.ChangeState(e, StateBase.StateNo_JumpLand);
            }
        }
    }
}
