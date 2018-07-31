using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace Mugen3D.Core
{
    public class LuaTriggerLib
    {

        public const string LIB_NAME = "trigger.cs";

        public static int OpenLib(ILuaState lua)
        {
            var define = new NameFuncPair[] {
                new NameFuncPair("CommandTest", CommandTest),
                new NameFuncPair("MoveType", GetMoveType),
                new NameFuncPair("PhysicsType", GetPhysicsType),
                new NameFuncPair("JustOnGround", IsJustOnGround),
                new NameFuncPair("StateNo", GetStateNo),
                new NameFuncPair("StateTime", GetStateTime),
                new NameFuncPair("Anim", GetAnim),
                new NameFuncPair("AnimTime", GetAnimTime),
                new NameFuncPair("AnimElem", GetAnimElem),
                new NameFuncPair("AnimElemTime", GetAnimElemTime),
                new NameFuncPair("LeftAnimTime", GetLeftAnimTime),
                new NameFuncPair("Vel", GetVel),

            };
            lua.L_NewLib(define);
            return 1;
        }

        public static int CommandTest(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            string command = lua.L_CheckString(2);
            bool res = c.cmdMgr.CommandIsActive(command);
            lua.PushBoolean(res);
            return 1;
        }

        public static int GetMoveType(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            int res = (int)c.status.moveType;
            lua.PushInteger(res);
            return 1;
        }

        public static int GetPhysicsType(ILuaState lua)
        {
            return 1;
        }

        public static int IsJustOnGround(ILuaState lua)
        {
            return 1;
        }

        public static int GetStateNo(ILuaState lua)
        {
            return 1;
        }

        public static int GetStateTime(ILuaState lua)
        {
            return 1;
        }

        public static int GetAnim(ILuaState lua)
        {
            return 1;
        }

        public static int GetAnimTime(ILuaState lua)
        {
            return 1;
        }

        public static int GetAnimElem(ILuaState lua)
        {
            return 1;
        }

        public static int GetAnimElemTime(ILuaState lua)
        {
            return 1;
        }

        public static int GetLeftAnimTime(ILuaState lua)
        {
            return 1;
        }

        public static int GetVel(ILuaState lua)
        {
            return 1;
        }

       

    }
}
