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
                new NameFuncPair("Facing", Facing),
                new NameFuncPair("MoveType", GetMoveType),
                new NameFuncPair("PhysicsType", GetPhysicsType),
                new NameFuncPair("JustOnGround", IsJustOnGround),
                new NameFuncPair("StateNo", GetStateNo),
                new NameFuncPair("StateTime", GetStateTime),
                new NameFuncPair("Anim", GetAnim),
                new NameFuncPair("AnimExist", IsAnimExist),
                new NameFuncPair("AnimTime", GetAnimTime),
                new NameFuncPair("AnimElem", GetAnimElem),
                new NameFuncPair("AnimElemTime", GetAnimElemTime),
                new NameFuncPair("LeftAnimTime", GetLeftAnimTime),
                new NameFuncPair("Vel", GetVel),
                new NameFuncPair("Pos", GetPos),
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

        public static int Facing(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            int facing = c.facing;
            lua.PushInteger(facing);
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
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            var physicsType = (int)c.status.moveType;
            lua.PushInteger(physicsType);
            return 1;
        }

        public static int IsJustOnGround(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            var justOnGround = c.moveCtr.justOnGround;
            lua.PushBoolean(justOnGround);
            return 1;
        }

        public static int GetStateNo(ILuaState lua)
        {
            //lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
           // Character c = (Character)lua.ToUserData(1);
            return 1;
        }

        public static int GetStateTime(ILuaState lua)
        {

            //lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //Character c = (Character)lua.ToUserData(1);
       
            return 1;
        }

        public static int GetAnim(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            var anim = c.animCtr.anim;
            lua.PushInteger(anim);
            return 1;
        }

        public static int IsAnimExist(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            int animNo = lua.L_CheckInteger(2);
            var isExist = c.animCtr.IsAnimExist(animNo);
            lua.PushBoolean(isExist);
            return 1;
        }

        public static int GetAnimTime(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            var animTime = c.animCtr.animTime;
            lua.PushInteger(animTime);
            return 1;
        }

        public static int GetAnimElem(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            var animElem = c.animCtr.animElem;
            lua.PushInteger(animElem);
            return 1;
        }

        public static int GetAnimElemTime(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            var animElemTime = c.animCtr.animElemTime;
            lua.PushInteger(animElemTime);
            return 1;
        }

        public static int GetLeftAnimTime(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            var leftTime = c.animCtr.animLength - c.animCtr.animTime;
            lua.PushInteger(leftTime);
            return 1;
        }

        public static int GetVel(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            var vel = c.moveCtr.velocity;
            lua.PushNumber(vel.x.AsFloat());
            lua.PushNumber(vel.y.AsFloat());
            return 2;
        }

        public static int GetPos(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            var pos = c.position;
            lua.PushNumber(pos.x.AsFloat());
            lua.PushNumber(pos.y.AsFloat());
            return 2;
        }

    }
}
