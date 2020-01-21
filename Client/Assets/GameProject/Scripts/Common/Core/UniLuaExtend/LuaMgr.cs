using System.Collections;
using System.Collections.Generic;
using UniLua;
using System;

namespace bluebean.Mugen3D.Core
{
    public class LuaMgr
    {

        #region 单例模式
        public static LuaMgr Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new LuaMgr();
                }
                return m_instance;
            }
        }

        private static LuaMgr m_instance;

        private LuaMgr()
        {
            m_luaState = LuaAPI.NewState();
            m_luaState.L_OpenLibs();
        }

        #endregion

        public ILuaState LuaState { get { return m_luaState; } }

        private ILuaState m_luaState;
 
        public void OpenLibrary(string moduleName, CSharpFunctionDelegate openFunc, bool global)
        {
            m_luaState.L_RequireF(moduleName, openFunc, global);
        }
    }
}