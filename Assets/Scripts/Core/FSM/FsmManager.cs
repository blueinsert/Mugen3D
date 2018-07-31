using System;
using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace Mugen3D.Core
{
    public class FsmManager
    {
        private Character m_owner;

        ///*
        public XLua.LuaTable fsm;
        public delegate XLua.LuaTable DelegateFsmConstruct(Unit u);
        public delegate void DelegateFsmUpdate(XLua.LuaTable self);
        public delegate void DelegateFsmChangeState(XLua.LuaTable self, int stateNo);
        private DelegateFsmConstruct funcFsmConstruct;
        private DelegateFsmUpdate funcFsmUpdate;
        private DelegateFsmChangeState funcFsmChangeState;
        //*/


         
        public FsmManager(string fsmFile, Character owner)
        {
            m_owner = owner;
            /*
            XLua.LuaTable characterModule = LuaMgr.Instance.Env.DoString(string.Format("return (require('{0}'))", fsmFile))[0] as XLua.LuaTable;
            funcFsmConstruct = characterModule.Get<string, DelegateFsmConstruct>("new");
            this.fsm = funcFsmConstruct(owner);
            funcFsmUpdate = this.fsm.Get<string, DelegateFsmUpdate>("update");
            funcFsmChangeState = this.fsm.Get<string, DelegateFsmChangeState>("changeState"); 
             */
            var env = LuaMgr.Instance.Env;
            var status = env.L_DoString("return (require('Chars/CharFsmManager'))");
            if (status != ThreadStatus.LUA_OK)
            {
                throw new Exception(env.ToString(-1));
            }
            if (!env.IsTable(-1))
            {
                throw new Exception("framework main's return value is not a table");
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
                throw new Exception("framework main's return value is not a table");
            }
            refUpdate = StoreMethod(env, "update");
            env.Pop(env.GetTop());
        }

        int refUpdate;

        public void Update()
        {
            if (funcFsmUpdate != null)
            {
                funcFsmUpdate(this.fsm);
            }
            CallMethod(LuaMgr.Instance.Env, refUpdate);
        }

        public void ChangeState(int stateNo)
        {
            if (funcFsmChangeState != null)
            {
                funcFsmChangeState(fsm, stateNo);
            }
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

    }

}
