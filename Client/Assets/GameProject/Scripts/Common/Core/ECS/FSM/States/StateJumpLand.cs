using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateJumpLand:StateBase
    {

        public override void OnEnter(Entity e)
        {
            base.OnEnter(e);
            var physice = e.GetComponent<PhysicsComponent>();
            physice.SetPhysicsType(PhysicsType.Stand);
            var move = e.GetComponent<MoveComponent>();
            move.VelSet(move.Velocity.x, 0);
            move.PosSet(move.Position.x, 0);
            var anim = e.GetComponent<AnimationComponent>();
            anim.ChangeAnim(47);
        }

        public override void OnExit(Entity e)
        {
            base.OnExit(e);
        }

        public override void OnUpdate(Entity e)
        {
            base.OnUpdate(e);
            var anim = e.GetComponent<AnimationComponent>();
            if (anim.LeftAnimTime <= 0)
            {
                var fsm = e.GetComponent<FSMComponent>();
                fsm.ChangeState(e,StateBase.StateNo_Stand);
            }
        }
    }
}
