using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    
    public class DelayImpactComponent : ComponentBase
    {
        public int NewStateNo { get { return m_newStateNo; } }
        public int NewFacing { get { return m_newFacing; } }
        public int NewAnimNo { get { return m_newAnimNo; } }
        public Vector? NewVel { get { return m_newVel; } }
        public Vector? NewVelDelta { get { return m_newVelDelta; } }
        public Vector? NewPos { get { return m_newPos; } }
        public Vector? NewPosDelta { get { return m_newPosDelta; } }

        private int m_newStateNo;
        private int m_newFacing;
        private int m_newAnimNo;
        private Vector? m_newVel;
        private Vector? m_newVelDelta;
        private Vector? m_newPos;
        private Vector? m_newPosDelta;

        public void ChangeState(int stateNo)
        {
            m_newStateNo = stateNo;
        }

        public void ChangeFacing(int facing)
        {
            m_newFacing = facing;
        }

        public void ChangeAnim(int animNo)
        {
            m_newAnimNo = animNo;
        }

        public void VelSet(Vector vel)
        {
            m_newVel = vel;
        }

        public void VelAdd(Vector velDelta)
        {
            m_newVelDelta = velDelta;
        }

        public void PosSet(Vector pos)
        {
            m_newPos = pos;
        }

        public void PosAdd(Vector posDelta)
        {
            m_newPosDelta = posDelta;
        }

        public void Clear()
        {
            m_newStateNo = -1;
            m_newFacing = 0;
            m_newAnimNo = -1;
            m_newVel = null;
            m_newVelDelta = null;
            m_newPos = null;
            m_newPosDelta = null;
        }
    }
}
