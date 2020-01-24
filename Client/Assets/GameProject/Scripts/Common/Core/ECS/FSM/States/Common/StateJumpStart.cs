using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateJumpStart:StateBase
    {
        int m_moveDir = 0;

        public StateJumpStart(Entity e):base(e)
        {
            
        }

        public override void OnEnter()
        {
            ChangeAnim(40);
            if (CommandIsActive("holdfwd"))
            {
                m_moveDir = 1;
            }else if (CommandIsActive("holdback"))
            {
                m_moveDir = -1;
            }
            else
            {
                m_moveDir = 0;
            }
            CtrlSet(false);
        }

        public override void OnExit()
        {  
            PhysicsSet(PhysicsType.Air);
            VelSet(m_moveDir*3, 8);
        }

        public override void OnUpdate()
        {
            if (LeftAnimTime <= 0)
            {
                ChangeState(StateConst.StateNo_JumpUp);
            }
        }
    }
}
