using System.Collections;
using System.Collections.Generic;
using UniLua;
using System;

namespace bluebean.Mugen3D.Core
{
    public class LuaMgr
    {
        private static LuaMgr mInstance;

        public static LuaMgr Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new LuaMgr();
                }
                return mInstance;
            }
        }

        public UniLua.ILuaState Env { get; private set; }

        private LuaMgr() {
            Env = LuaAPI.NewState();
            Env.L_OpenLibs();
            Env.L_RequireF(LuaTriggerLib.LIB_NAME, LuaTriggerLib.OpenLib, false);
            Env.L_RequireF(LuaControllerLib.LIB_NAME, LuaControllerLib.OpenLib, false);
            Env.L_RequireF(LuaDebugLib.LIB_NAME, LuaDebugLib.OpenLib, false);
        }
  
        public static void AddLoader(CustomFileLoader loader)
        {
            LuaFile.AddLoader(loader);
        }
 
    }
}