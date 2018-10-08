#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;


namespace XLua
{
    public partial class DelegateBridge : DelegateBridgeBase
    {
		
		public void __Gen_Delegate_Imp0(XLua.LuaTable p0)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, p0);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		public void __Gen_Delegate_Imp1(XLua.LuaTable p0, UnityEngine.Transform p1)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, p0);
                translator.Push(L, p1);
                
                int __gen_error = LuaAPI.lua_pcall(L, 2, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		public void __Gen_Delegate_Imp2()
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                
                
                LuaAPI.lua_getref(L, luaReference);
                
                
                int __gen_error = LuaAPI.lua_pcall(L, 0, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		public void __Gen_Delegate_Imp3(XLua.LuaTable p0, int p1)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, p0);
                LuaAPI.xlua_pushinteger(L, p1);
                
                int __gen_error = LuaAPI.lua_pcall(L, 2, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		public void __Gen_Delegate_Imp4(XLua.LuaTable p0, Mugen3D.Core.Unit p1)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, p0);
                translator.Push(L, p1);
                
                int __gen_error = LuaAPI.lua_pcall(L, 2, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
        
		static DelegateBridge()
		{
		    Gen_Flag = true;
		}
		
		public override Delegate GetDelegateByType(Type type)
		{
		
		    if (type == typeof(LuaMonoBehaviour.MonoBehaviourEvent))
			{
			    return new LuaMonoBehaviour.MonoBehaviourEvent(__Gen_Delegate_Imp0);
			}
		
		    if (type == typeof(System.Action<XLua.LuaTable>))
			{
			    return new System.Action<XLua.LuaTable>(__Gen_Delegate_Imp0);
			}
		
		    if (type == typeof(LuaMonoBehaviour.LuaBehaviourInit))
			{
			    return new LuaMonoBehaviour.LuaBehaviourInit(__Gen_Delegate_Imp1);
			}
		
		    if (type == typeof(UnityEngine.Events.UnityAction))
			{
			    return new UnityEngine.Events.UnityAction(__Gen_Delegate_Imp2);
			}
		
		    if (type == typeof(System.Action<XLua.LuaTable, int>))
			{
			    return new System.Action<XLua.LuaTable, int>(__Gen_Delegate_Imp3);
			}
		
		    if (type == typeof(System.Action<XLua.LuaTable, Mugen3D.Core.Unit>))
			{
			    return new System.Action<XLua.LuaTable, Mugen3D.Core.Unit>(__Gen_Delegate_Imp4);
			}
		
		    return null;
		}
	}
    
}