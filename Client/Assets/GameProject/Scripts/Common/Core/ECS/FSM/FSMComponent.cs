using System;
using System.Collections;
using System.Collections.Generic;
using UniLua;

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

        /// <summary>
        /// lua中的update函数的索引
        /// </summary>
        private int m_refUpdate;

        public void Initialize(string luaScriptFileName, Entity owner)
        {
            m_stateDic.Add(StateBase.StateNo_Stand, new StateStand());
            m_stateDic.Add(StateBase.StateNo_Walk, new StateWalk());
            m_stateDic.Add(StateBase.StateNo_JumpStart, new StateJumpStart());
            m_stateDic.Add(StateBase.StateNo_JumpUp, new StateJumpUp());
            m_stateDic.Add(StateBase.StateNo_JumpDown, new StateJumpDown());
            m_stateDic.Add(StateBase.StateNo_JumpLand, new StateJumpLand());

            var env = LuaMgr.Instance.LuaState;
            var status = env.L_DoString("return (require('Lua_ABS/FsmScriptLoader'))");
            if (status != ThreadStatus.LUA_OK)
            {
                throw new Exception(env.ToString(-1));
            }
            if (!env.IsTable(-1))
            {
                throw new Exception("FsmScriptLoader's return value is not a table");
            }
            env.GetField(-1, "create");
            if (!env.IsFunction(-1))
            {
                throw new Exception(string.Format("method {0} not found!", env));
            }
            env.PushString(luaScriptFileName);
            env.PushLightUserData(owner);
            status = env.PCall(2, 1, 0);
            if (status != ThreadStatus.LUA_OK)
            {
                Debug.LogError(env.ToString(-1));
            }
            if (!env.IsTable(-1))
            {
                throw new Exception("createFSM's return value is not a table");
            }
            m_refUpdate = StoreMethod(env, "update");
        }

        private int StoreMethod(UniLua.ILuaState env, string name)
        {
            env.GetField(-1, name);
            if (!env.IsFunction(-1))
            {
                throw new Exception(string.Format("method {0} not found!", name));
            }
            return env.L_Ref(LuaDef.LUA_REGISTRYINDEX);
        }

        private void CallScript()
        {
            var env = LuaMgr.Instance.LuaState;
            env.RawGetI(LuaDef.LUA_REGISTRYINDEX, m_refUpdate);
            var status = env.PCall(0, 0, 0);
            if (status != ThreadStatus.LUA_OK)
            {
                Debug.LogError(env.ToString(-1));
            }
        }

        public void Update(Entity owner)
        {
            m_stateTime++;
            /*
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
            */
            ///*
            if (m_stateNo == -1)
            {
                m_stateNo = 0;
            }
            CallScript();
            //*/
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
