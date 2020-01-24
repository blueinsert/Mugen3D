using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateJumpDown : StateBase
    {
        public StateJumpDown(Entity e) : base(e)
        {

        }

        public override void OnEnter()
        {
            ChangeAnim(44);
            CtrlSet(false);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            if (Pos.y + Vel.y*Time.deltaTime <= 0)
            {
                ChangeState(StateConst.StateNo_JumpLand);
            }
        }
    }
}
