using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateWalk:StateBase
    {
        public StateWalk(Entity e) : base(e) { }

        public override void OnEnter()
        {
            MoveTypeSet(MoveType.Idle);
            CtrlSet(true);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            if (CommandIsActive("holdup"))
            {
                ChangeState(StateConst.StateNo_JumpStart);
            }else if (CommandIsActive("holdfwd"))
            {
                VelSet(3, 0);
                if(Anim!=20)
                    ChangeAnim(20);
            }
            else if (CommandIsActive("holdback"))
            {
                VelSet(-3, 0);
                if(Anim!=21)
                    ChangeAnim(21);
            }else
            {
                ChangeState(StateConst.StateNo_Stand);
            }
        }
    }
}
