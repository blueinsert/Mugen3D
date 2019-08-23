using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniLua;

namespace bluebean.Mugen3D.Core
{
    public class LuaDebugLib
    {

        public const string LIB_NAME = "debug.cs";

        public static int OpenLib(ILuaState lua)
        {
            var define = new NameFuncPair[] {
                new NameFuncPair("AddMsg", AddMsg),
            };
            lua.L_NewLib(define);
            return 1;
        }

        public static int AddMsg(ILuaState lua)
        {
            int slot = lua.L_CheckInteger(1);
            string key = lua.L_CheckString(2);
            string value = lua.L_CheckString(3);
            //Core.Debug.AddGUIDebugMsg(slot, key, value);
            return 0;
        }
    }
}
