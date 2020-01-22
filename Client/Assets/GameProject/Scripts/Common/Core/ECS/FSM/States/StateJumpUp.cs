using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateJumpUp : StateBase
    {
        public override void OnEnter(Entity e)
        {
            base.OnEnter(e);
           
            var anim = e.GetComponent<AnimationComponent>();
            anim.ChangeAnim(41);
        }

        public override void OnExit(Entity e)
        {
            base.OnExit(e);
        }

        public override void OnUpdate(Entity e)
        {
            base.OnUpdate(e);
            var fsm = e.GetComponent<FSMComponent>();
            var move = e.GetComponent<MoveComponent>();
            if(move.Velocity.y + move.Acceler.y*Time.deltaTime <= 0)
            {
                fsm.ChangeState(e, StateBase.StateNo_JumpDown);
            }
        }
    }
}
