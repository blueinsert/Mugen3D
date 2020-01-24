using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateJumpLand:StateBase
    {
        public StateJumpLand(Entity e) : base(e)
        {

        }

        public override void OnEnter()
        {
            PhysicsSet(PhysicsType.Stand);
            VelSet(Vel.x, 0);
            ChangeAnim(47);
            CtrlSet(false);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            if (LeftAnimTime <= 0)
            {
                ChangeState(StateConst.StateNo_Stand);
            }
        }
    }
}
