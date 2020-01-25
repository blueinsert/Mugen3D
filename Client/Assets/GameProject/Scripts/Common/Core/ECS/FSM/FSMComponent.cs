using System;
using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{

    public class FSMLayer
    {
        public int m_stateNo = -1;
        public int m_stateTime = -1;
    }

    /// <summary>
    /// 角色状态机组件
    /// </summary>
    public class FSMComponent : ComponentBase
    {
       
        public int StateNo { get { return CurrentLayer.m_stateNo; } }
        public int StateTime { get { return CurrentLayer.m_stateTime; } }
        public FSMLayer CurrentLayer { get { return m_layerStack.Peek(); } }

        private Stack<FSMLayer> m_layerStack = new Stack<FSMLayer>();

        private Dictionary<int, StateBase> m_stateDic = new Dictionary<int, StateBase>();

        public void Initialize(Entity owner)
        {
            m_layerStack.Clear();
            m_layerStack.Push(new FSMLayer());
            m_stateDic.Clear();
            m_stateDic.Add(StateConst.StateNo_Stand, new StateStand(owner));
            m_stateDic.Add(StateConst.StateNo_Walk, new StateWalk(owner));
            m_stateDic.Add(StateConst.StateNo_JumpStart, new StateJumpStart(owner));
            m_stateDic.Add(StateConst.StateNo_JumpUp, new StateJumpUp(owner));
            m_stateDic.Add(StateConst.StateNo_JumpDown, new StateJumpDown(owner));
            m_stateDic.Add(StateConst.StateNo_JumpLand, new StateJumpLand(owner));

            m_stateDic.Add(StateConst.StateNO_HitPause, new StateHitPause(owner));
            //guard
            m_stateDic.Add(StateConst.StateNo_GuardStart, new StateGuardStart(owner));
            m_stateDic.Add(StateConst.StateNo_GuardEnd, new StateGuardEnd(owner));
            m_stateDic.Add(StateConst.StateNo_Guarding, new StateGuarding(owner));
            m_stateDic.Add(StateConst.StateNo_GuardingShake, new StateGuardingShake(owner));
            m_stateDic.Add(StateConst.StateNo_GuardingSlide, new StateGuardingSlide(owner));
            //be hit       
            m_stateDic.Add(StateConst.StateNo_GetHitStandShake, new StateGetHitStandShake(owner));
            m_stateDic.Add(StateConst.StateNo_GetHitStandSlide, new StateGetHitStandSlide(owner));
            //attack
            m_stateDic.Add(StateConst.StateNo_StandLightPunch, new StateStandLightPunch(owner));
        }

        private void UpdateLayer(FSMLayer layer) {
            layer.m_stateTime++;
            ///*
            if (layer.m_stateNo == -1)
            {
                m_stateDic[0].OnEnter();
                layer.m_stateNo = 0;
            }
            if (m_stateDic.ContainsKey(layer.m_stateNo))
            {
                var state = m_stateDic[layer.m_stateNo];
                state.OnUpdate();
            }
        }

        public void Update(Entity owner)
        {
            var layer = CurrentLayer;
            UpdateLayer(layer);
        }

        public void ChangeState(int stateNo)
        {
            var layer = CurrentLayer;
            if (layer.m_stateNo != stateNo)
            {
                m_stateDic[layer.m_stateNo].OnExit();
            }
            layer.m_stateNo = stateNo;
            layer.m_stateTime = 0;
            m_stateDic[layer.m_stateNo].OnEnter();
        }

        public void PushLayer(int stateNo)
        {
            FSMLayer layer = new FSMLayer();
            layer.m_stateNo = stateNo;
            layer.m_stateTime = 0;
            m_layerStack.Push(layer);
            m_stateDic[layer.m_stateNo].OnEnter();
        }

        public void PopLayer()
        {
            m_layerStack.Pop();
        }
    }
}
