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
                throw new Exception("GetTableFieldInt: is not table");
            int result;
            lua.GetField(-1, key);
            result = lua.ToInteger(-1);
            lua.Pop(1);
            return result;
        }

        public static string GetTableFieldString(ILuaState lua, string key)
        {
            if (!lua.IsTable(-1))
                throw new Exception("GetTableFieldString: is not table");
            string result;
            lua.GetField(-1, key);
            result = lua.ToString(-1);
            lua.Pop(1);
            return result;
        }

        public static Vector GetTableFieldVector(ILuaState lua, string key)
        {
            if (!lua.IsTable(-1))
                throw new Exception("GetTableFieldVector: is not table");
            lua.GetField(-1, key);
            if (!lua.IsTable(-1))
                throw new Exception("Get Vector: is not table");
            int x, y;
            //get x
            lua.PushInteger(1);
            lua.GetTable(-2);
            x = lua.ToInteger(-1);
            lua.Pop(1);
            //get y
            lua.PushInteger(2);
            lua.GetTable(-2);
            y = lua.ToInteger(-1);
            lua.Pop(1);

            lua.Pop(1);
            return new Vector(x, y, 0);
        }
    }
}
