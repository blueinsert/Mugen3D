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

        private int m_stateNo = -1;
        private int m_stateTime;

        private Dictionary<int, StateBase> m_stateDic = new Dictionary<int, StateBase>();

        public void Initialize()
        {
            m_stateDic.Add(StateBase.StateNo_Stand, new StateStand());
            m_stateDic.Add(StateBase.StateNo_Walk, new StateWalk());
            m_stateDic.Add(StateBase.StateNo_JumpStart, new StateJumpStart());
            m_stateDic.Add(StateBase.StateNo_JumpUp, new StateJumpUp());
            m_stateDic.Add(StateBase.StateNo_JumpDown, new StateJumpDown());
            m_stateDic.Add(StateBase.StateNo_JumpLand, new StateJumpLand());
        }

        public void Update(Entity owner)
        {
            m_stateTime++;
            if(m_stateNo == -1)
            {
                m_stateDic[0].OnEnter(owner);
                m_stateNo = 0;
            }
            if (m_stateDic.ContainsKey(m_stateNo))
            {
                var state = m_stateDic[m_stateNo];
                state.OnUpdate(owner);
            }
           
        }

        public void ChangeState(Entity owner, int stateNo)
        {
            if(m_stateNo != stateNo)
            {
                m_stateDic[m_stateNo].OnExit(owner);
            }
            m_stateNo = stateNo;
            m_stateTime = 0;
            m_stateDic[m_stateNo].OnEnter(owner);
        }
    }
}
