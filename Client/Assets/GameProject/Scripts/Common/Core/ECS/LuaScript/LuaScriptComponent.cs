using System;
using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace bluebean.Mugen3D.Core
{
    public class LuaScriptComponent : ComponentBase
    {
        private int refUpdate;

        private int StoreMethod(UniLua.ILuaState env, string name)
        {
            env.GetField(-1, name);
            if (!env.IsFunction(-1))
            {
                throw new Exception(string.Format("method {0} not found!", name));
            }
            return env.L_Ref(LuaDef.LUA_REGISTRYINDEX);
        }

        private void AttachLuaScript(string luaScriptFileName)
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
            //env.PushString((m_owner.config as UnitConfig).fsm);//todo
            //env.PushLightUserData(m_owner);
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

        public void Init(string luaScriptFileName)
        {
            AttachLuaScript(luaScriptFileName);
        }

        private void CallScript(int stateNo, int stateTime)
        {
            var env = LuaMgr.Instance.Env;
            env.RawGetI(LuaDef.LUA_REGISTRYINDEX, this.refUpdate);
            env.PushInteger(stateNo);
            env.PushInteger(stateTime);
            var status = env.PCall(2, 0, 0);
            if (status != ThreadStatus.LUA_OK)
            {
                Debug.LogError(env.ToString(-1));
                Debug.LogError("update luaScript error StateNo:" + stateNo + " stateTime:" + stateTime);
            }
        }

        public void Update(int stateNo, int stateTime)
        {
            CallScript(stateNo, stateTime);
        }
    }
}
