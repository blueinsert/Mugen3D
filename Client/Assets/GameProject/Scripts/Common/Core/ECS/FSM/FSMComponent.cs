using System;
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
        private int m_stateTime= -1;

        private Dictionary<int, StateBase> m_stateDic = new Dictionary<int, StateBase>();

        public void Initialize(Entity owner)
        {
            m_stateDic.Clear();
            m_stateDic.Add(StateConst.StateNo_Stand, new StateStand(owner));
            m_stateDic.Add(StateConst.StateNo_Walk, new StateWalk(owner));
            m_stateDic.Add(StateConst.StateNo_JumpStart, new StateJumpStart(owner));
            m_stateDic.Add(StateConst.StateNo_JumpUp, new StateJumpUp(owner));
            m_stateDic.Add(StateConst.StateNo_JumpDown, new StateJumpDown(owner));
            m_stateDic.Add(StateConst.StateNo_JumpLand, new StateJumpLand(owner));
            m_stateDic.Add(StateConst.StateNo_StandLightPunch, new StateStandLightPunch(owner));
            m_stateDic.Add(StateConst.StateNo_GetHitStandShake, new StateGetHitStandShake(owner));
            m_stateDic.Add(StateConst.StateNo_GetHitStandSlide, new StateGetHitStandSlide(owner));
        }

        public void Update(Entity owner)
        {
            m_stateTime++;
            ///*
            if(m_stateNo == -1)
            {
                m_stateDic[0].OnEnter();
                m_stateNo = 0;
            }
            if (m_stateDic.ContainsKey(m_stateNo))
            {
                var state = m_stateDic[m_stateNo];
                state.OnUpdate();
            }
        }

        public void ChangeState(int stateNo)
        {
            if(m_stateNo != stateNo)
            {
                m_stateDic[m_stateNo].OnExit();
            }
            m_stateNo = stateNo;
            m_stateTime = 0;
            m_stateDic[m_stateNo].OnEnter();
        }
    }
}
