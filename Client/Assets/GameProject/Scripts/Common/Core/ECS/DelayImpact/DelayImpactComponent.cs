using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    
    public class DelayImpactComponent : ComponentBase
    {
        public int NewStateNo { get { return m_newStateNo; } }

        private int m_newStateNo;

        public void ChangeState(int stateNo)
        {
            m_newStateNo = stateNo;
        }

        public void Clear()
        {
            m_newStateNo = -1;
        }
    }
}
