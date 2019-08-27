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

        public void Init(string luaScriptFileName,Entity owner)
        {
            var env = LuaMgr.Instance.LuaState;
            var status = env.L_DoString("return (require('Lua_ABS/FsmManager'))");
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
            refUpdate = StoreMethod(env, "update");
        }

        private void CallScript()
        {
            var env = LuaMgr.Instance.LuaState;
            env.RawGetI(LuaDef.LUA_REGISTRYINDEX, this.refUpdate);
            var status = env.PCall(0, 0, 0);
            if (status != ThreadStatus.LUA_OK)
            {
                Debug.LogError(env.ToString(-1));
            }
        }

        public void Update()
        {
            CallScript();
        }
    }
}
