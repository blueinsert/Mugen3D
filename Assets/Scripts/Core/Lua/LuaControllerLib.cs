using System;
using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace Mugen3D.Core
{
    public static class LuaControllerLib 
    {
        public const string LIB_NAME = "controller.cs";

        public static int OpenLib(ILuaState lua)
        {
            var define = new NameFuncPair[] {
                new NameFuncPair("ChangeState", ChangeState),
                new NameFuncPair("ChangeAnim", ChangeAnim),
                new NameFuncPair("ChangeFacing", ChangeFacing),
                new NameFuncPair("HitDefSet", HitDefSet),
                new NameFuncPair("PhysicsSet", PhysicsSet),
                new NameFuncPair("MoveTypeSet", MoveTypeSet),
                new NameFuncPair("VelSet", VelSet),
                new NameFuncPair("CtrlSet", CtrlSet),
                new NameFuncPair("Pause", Pause),
            };
            lua.L_NewLib(define);
            return 1;
        }

        public static int ChangeState(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            int stateNo = lua.L_CheckInteger(2);
            c.fsmMgr.ChangeState(stateNo);
            return 0;
        }

        public static int ChangeFacing(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            int facing = lua.L_CheckInteger(2);
            c.ChangeFacing(facing);
            return 0;
        }

        public static int ChangeAnim(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            int anim = lua.L_CheckInteger(2);
            c.animCtr.ChangeAnim(anim);
            return 0;
        }

        static HitDef GetHitDef(ILuaState lua)
        {
            if(!lua.IsTable(-1))
                throw new Exception("get hitDef is not table");
            HitDef hitDef = new HitDef();
            try
            {
                hitDef.hitDamage = LuaUtil.GetTableFieldInt(lua, "hitDamage");
                hitDef.guardDamage = LuaUtil.GetTableFieldInt(lua, "guardDamage");
                hitDef.hitPauseTime = LuaUtil.GetTableFieldVector(lua, "hitPauseTime");
                hitDef.hitSlideTime = LuaUtil.GetTableFieldInt(lua, "hitSlideTime");
                hitDef.groundVel = LuaUtil.GetTableFieldVector(lua, "groundVel");
            }
            catch (Exception e)
            {
                Debug.LogError("get hitDef field error:" + e.Message);
            }
            return hitDef;
        }

        public static int HitDefSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            HitDef hitDef = GetHitDef(lua);
            c.SetHitDefData(hitDef);
            return 0;
        }

        public static int PhysicsSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            int physics = lua.L_CheckInteger(2);
            PhysicsType physicsType = (PhysicsType)physics;
            c.status.physicsType = physicsType;
            return 0;
        }

        public static int MoveTypeSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            int moveType = lua.L_CheckInteger(2);
            MoveType move = (MoveType)moveType;
            c.status.moveType = move;
            return 0;
        }

        private static Number ToNumber(double v)
        {
            return new Number((int)(v * 1000)) / new Number(1000);
        }

        public static int VelSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            double velX = lua.L_CheckNumber(2);
            double velY = lua.L_CheckNumber(3);
            c.moveCtr.VelSet(ToNumber(velX), ToNumber(velY));
            return 0;
        }

        public static int CtrlSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            bool ctrl = lua.ToBoolean(2);
            c.status.ctrl = ctrl;
            return 0;
        }

        public static int Pause(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Character c = (Character)lua.ToUserData(1);
            int duration = lua.L_CheckInteger(2);
            c.Pause(duration);
            return 0;
        }

        
    }
}
