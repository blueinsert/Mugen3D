using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateStand : StateBase
    {
        public StateStand(Entity e) : base(e)
        {

        }

        public override void OnEnter()
        {
            PhysicsSet(PhysicsType.Stand);
            ChangeAnim(0);
            VelSet(0, 0);
            CtrlSet(true);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            if (CommandIsActive("holdfwd"))
            {
                ChangeState(StateBase.StateNo_Walk);
            }else if (CommandIsActive("holdback"))
            {
                ChangeState(StateBase.StateNo_Walk);
            }else if (CommandIsActive("holdup"))
            {
                ChangeState(StateBase.StateNo_JumpStart);
            }
        }
    }
}
