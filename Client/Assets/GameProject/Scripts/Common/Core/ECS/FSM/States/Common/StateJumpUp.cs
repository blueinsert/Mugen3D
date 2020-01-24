using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateJumpUp : StateBase
    {
        public StateJumpUp(Entity e) : base(e) { }

        public override void OnEnter()
        { 
            ChangeAnim(41);
            CtrlSet(false);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            var vel = Vel;
            if(vel.y + Acceler.y*Time.deltaTime <= 0)
            {
                ChangeState(StateConst.StateNo_JumpDown);
            }
        }
    }
}
