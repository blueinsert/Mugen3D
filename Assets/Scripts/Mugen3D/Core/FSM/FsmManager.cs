using System;
using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace Mugen3D.Core
{
    public class FsmManager
    {
        private Unit m_owner;
        private int refUpdate;

        private int m_stateNoToChange = -1;

        public int stateNo { get; private set; }
        public int stateTime { get; private set; }

        void CreateFSM()
        {
            var env = LuaMgr.Instance.Env;
            var status = env.L_DoString("return (require('System/FsmManager'))");
            if (status != ThreadStatus.LUA_OK)
            {
                throw new Exception(env.ToString(-1));
            }
            if (!env.IsTable(-1))
            {
                throw new Exception("FsmManager's return value is not a table");
            }
            env.GetField(-1, "create");
            if (!env.IsFunction(-1))
            {
                throw new Exception(string.Format("method {0} not found!", env));
            }
            env.PushString((m_owner.config as UnitConfig).fsm);
            env.PushLightUserData(m_owner);
            status = env.PCall(2, 1, 0);
            if (status != ThreadStatus.LUA_OK)
            {
                Debug.LogError(env.ToString(-1));
            }
            if (!env.IsTable(-1))
            {
                throw new Exception("createFSM's return value is not a table");
            }
            refUpdate = StoreMethod(env, "update");
        }

        public FsmManager(string fsmFile, Unit owner)
        {
            m_owner = owner;
            CreateFSM();
            Init();
        }

        private void Init()
        {
            stateNo = 0;
            stateTime = -1;
            m_stateNoToChange = 0;
        }

        public void ChangeState(int stateNo)
        {
            if (this.m_stateNoToChange == -1)
            {
                Debug.Log("curStateNo:" + this.stateNo + " changeState:" + stateNo);
                this.m_stateNoToChange = stateNo;
            }
        }

        public void ProcessChangeState()
        {
            if (this.m_stateNoToChange != -1)
            {
                this.stateNo = this.m_stateNoToChange;
                this.stateTime = -1;
                this.m_stateNoToChange = -1;
                CallScript(this.stateNo, this.stateTime);
            }
        }

        private void CallScript(int stateNo, int stateTime) {
            var env = LuaMgr.Instance.Env;
            env.RawGetI(LuaDef.LUA_REGISTRYINDEX, this.refUpdate);
            env.PushInteger(stateNo);
            env.PushInteger(stateTime);
            var status = env.PCall(2, 0, 0);
            if (status != ThreadStatus.LUA_OK)
            {
                Debug.LogError(env.ToString(-1));
                Debug.LogError("update State No:" + this.stateNo + " stateTime:" + this.stateTime + " error");
            }
        }

        public void Update()
        {
            this.stateTime++;
            CallScript(this.stateNo, this.stateTime);  
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

    }

}
