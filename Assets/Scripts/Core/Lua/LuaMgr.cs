using System.Collections;
using System.Collections.Generic;
using UniLua;
using System;

namespace Mugen3D.Core
{
    public class LuaMgr
    {
        public static System.Func<string, string> pathHook;
        private static LuaMgr mInstance;

        private LuaMgr() { }

        public static LuaMgr Instance
        {
            get
            {
                if (mInstance == null)
                {
                    Init();
                }
                return mInstance;
            }
        }

        public UniLua.ILuaState Env { get; private set; }

        private static void Init()
        {
            mInstance = new LuaMgr();
            var Lua = LuaAPI.NewState();
            Lua.L_OpenLibs();
            mInstance.Env = Lua;
        }

        public static void Deinit()
        {

        }

        public static void SetPathHook(PathHook hook)
        {
            LuaFile.SetPathHook(hook);
        }
 
    }
}