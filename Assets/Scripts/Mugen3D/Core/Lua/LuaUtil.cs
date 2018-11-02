using System;
using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace Mugen3D.Core
{
    public class LuaTable : IDisposable{
        private ILuaState m_lua;

        public LuaTable(ILuaState lua){
            if (!lua.IsTable(-1))
                throw new Exception("lua top: is not a table");
            m_lua = lua;  
        }

        public int GetInt(string key, int defaultValue = 0){
            int value = defaultValue;
            m_lua.GetField(-1, key);
            if (!m_lua.IsNoneOrNil(-1))
                value = m_lua.ToInteger(-1);
            m_lua.Pop(1);
            return value;
        }

        public Number GetNumber(string key, Number defaultValue){
            Number value = defaultValue;
            m_lua.GetField(-1, key);
            if (!m_lua.IsNoneOrNil(-1)){
                var v = m_lua.ToNumber(-1);
                value = v.ToNumber();
            }
            m_lua.Pop(1);
            return value;
        }

        public string GetString(string key){ 
            string result = "";
            m_lua.GetField(-1, key);
            if(!m_lua.IsNoneOrNil(-1))
                result = m_lua.ToString(-1);
            m_lua.Pop(1);
            return result;
        }

        public Number[] GetNumberArray(string key, int arrayLength)
        {
            m_lua.GetField(-1, key);
            if (!m_lua.IsTable(-1))
                throw new Exception("GetNumberArray: is not table");
            Number[] res = new Number[arrayLength];
            for (int i = 1; i <= arrayLength; i++)
            {
                m_lua.PushInteger(i);
                m_lua.GetTable(-2);
                var value = m_lua.ToNumber(-1);
                m_lua.Pop(1);
                res[i-1] = value.ToNumber();
            }
            m_lua.Pop(1);
            return res;
        }

        public int[] GetIntArray(string key, int arrayLength)
        {
            m_lua.GetField(-1, key);
            if (!m_lua.IsTable(-1))
                throw new Exception("GetIntArray: is not table");
            int[] res = new int[arrayLength];
            for (int i = 1; i <= arrayLength; i++)
            {
                m_lua.PushInteger(i);
                m_lua.GetTable(-2);
                var value = m_lua.ToInteger(-1);
                m_lua.Pop(1);
                res[i - 1] = value;
            }
            m_lua.Pop(1);
            return res;
        }

        public LuaTable GetTable(string key) {
            m_lua.GetField(-1, key);
            if (!m_lua.IsTable(-1))
                throw new Exception("GetTable: is not table");
            return new LuaTable(m_lua);
        }

        public void Dispose() {
            m_lua.Pop(1);
        }
    }

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

        public static int[] GetTableFieldIntArray(ILuaState lua, string key, int arrayLength)
        {
            if (!lua.IsTable(-1))
                throw new Exception("GetTableFieldVector: is not table");
            lua.GetField(-1, key);
            if (!lua.IsTable(-1))
                throw new Exception("Get Vector: is not table");
            int[] res = new int[arrayLength];
            for (int i = 1; i <= arrayLength; i++)
            {
                lua.PushInteger(1);
                lua.GetTable(-2);
                var value = lua.ToInteger(-1);
                lua.Pop(1);
                res[i-1] = value;
            }
            lua.Pop(1);
            return res;
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
