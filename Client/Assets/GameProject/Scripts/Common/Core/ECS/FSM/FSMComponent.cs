using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 角色状态机组件
    /// </summary>
    public class FSMComponent : ComponentBase
    {
        public int StateNo { get { return m_stateNo; } }
        public int StateTime { get { return m_stateTime; } }

        private int m_stateNo;
        private int m_stateTime; 

        public void Update()
        {
            m_stateTime++;
        }

        public void ChangeState(int stateNo)
        {
            m_stateNo = stateNo;
            m_stateTime = 0;
        }
    }
}
