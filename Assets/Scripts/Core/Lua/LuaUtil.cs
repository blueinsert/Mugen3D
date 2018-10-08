using System;
using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace Mugen3D.Core
{
    public class LuaUtil
    {
        public static int GetTableFieldInt(ILuaState lua, string key)
        {
            if (!lua.IsTable(-1))
                throw new Exception("GetTableField: is not table");
            int result;
            lua.GetField(-1, key);
            result = lua.ToInteger(-1);
            lua.Pop(1);
            return result;
        }

        public static string GetTableFieldString(ILuaState lua, string key)
        {
            if (!lua.IsTable(-1))
                throw new Exception("GetTableField: is not table");
            string result;
            lua.GetField(-1, key);
            result = lua.ToString(-1);
            lua.Pop(1);
            return result;
        }
    }
}
