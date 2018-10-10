using System;
using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace Mugen3D.Core
{
    public class FsmManager
    {
        private Character m_owner;
        private int refUpdate;
        private int refChangeState;
        private int m_stateNoToChange = -1;

        public int stateNo { get; private set; }
        public int stateTime { get; private set; }

        void CreateFSM()
        {
            var env = LuaMgr.Instance.Env;
            var status = env.L_DoString("return (require('Chars/FsmManager'))");
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
            env.PushString(m_owner.config.fsmConfigFile);
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
            refChangeState = StoreMethod(env, "changeState");
        }

        public FsmManager(string fsmFile, Character owner)
        {
            m_owner = owner;
            CreateFSM();
            Init();
        }

        private void Init()
        {
            stateNo = 0;
            stateTime = 0;
        }

        void CallMethod(int refFunc)
        {
            var env = LuaMgr.Instance.Env;
            env.RawGetI(LuaDef.LUA_REGISTRYINDEX, refFunc);
            env.PushInteger(this.stateNo);
            var status = env.PCall(1, 0, 0);
            if (status != ThreadStatus.LUA_OK)
            {
                Debug.LogError(env.ToString(-1));
            }
        }

        public void ChangeState(int stateNo)
        {
            if (this.m_stateNoToChange == -1)
            {
                this.m_stateNoToChange = stateNo;
            }
        }

        public void ProcessChangeState()
        {
            if (this.m_stateNoToChange != -1)
            {
                this.stateNo = this.m_stateNoToChange;
                this.stateTime = 0;
                CallMethod(this.refChangeState);
                this.m_stateNoToChange = -1;
            }
        }

        public void Update()
        {
            CallMethod(this.refUpdate);
            this.stateTime++;
        }

        public int StoreMethod(UniLua.ILuaState env, string name)
        {
            env.GetField(-1, name);
            if (!env.IsFunction(-1))
            {
                throw new Exception(string.Format("method {0} not found!", name));
            }
            return env.L_Ref(LuaDef.LUA_REGISTRYINDEX);
        }

        /*
        public void CallMethod(UniLua.ILuaState env, int funcRef)
        {
            env.RawGetI(LuaDef.LUA_REGISTRYINDEX, funcRef);

            // insert `traceback' function
            var b = env.GetTop();
            env.PushCSharpFunction(Traceback);
            env.Insert(b);

            var status = env.PCall(0, 0, b);
            if (status != ThreadStatus.LUA_OK)
            {
                Debug.LogError(env.ToString(-1));
            }

            // remove `traceback' function
            env.Remove(b);
        }

        private static int Traceback(ILuaState lua)
        {
            var msg = lua.ToString(1);
            if (msg != null)
            {
                lua.L_Traceback(lua, msg, 1);
            }
            // is there an error object?
            else if (!lua.IsNoneOrNil(1))
            {
                // try its `tostring' metamethod
                if (!lua.L_CallMeta(1, "__tostring"))
                {
                    lua.PushString("(no error message)");
                }
            }
            return 1;
        }
        */
    }

}
