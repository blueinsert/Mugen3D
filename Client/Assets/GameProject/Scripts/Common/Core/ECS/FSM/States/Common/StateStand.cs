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
            MoveTypeSet(MoveType.Idle);
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
                ChangeState(StateConst.StateNo_Walk);
            }else if (CommandIsActive("holdback"))
            {
                ChangeState(StateConst.StateNo_Walk);
            }else if (CommandIsActive("holdup"))
            {
                ChangeState(StateConst.StateNo_JumpStart);
            }else if (CommandIsActive("a"))
            {
                ChangeState(StateConst.StateNo_StandLightPunch);
            }
        }
    }
}
