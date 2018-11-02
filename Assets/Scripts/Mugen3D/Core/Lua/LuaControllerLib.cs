﻿using System;
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
                new NameFuncPair("HitBy", HitBy),
                new NameFuncPair("NoHitBy", NoHitBy),
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
            Unit c = (Unit)lua.ToUserData(1);
            int stateNo = lua.L_CheckInteger(2);
            c.fsmMgr.ChangeState(stateNo);
            return 0;
        }

        public static int ChangeFacing(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            int facing = lua.L_CheckInteger(2);
            c.ChangeFacing(facing);
            return 0;
        }

        public static int ChangeAnim(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            int anim = lua.L_CheckInteger(2);
            c.animCtr.ChangeAnim(anim);
            return 0;
        }

        static HitDef GetHitDef(ILuaState lua)
        {
            HitDef hitDef = new HitDef();
            using(var t = new LuaTable(lua)){
                hitDef.hitFlag = t.GetInt("hitFlag");
                hitDef.guardFlag = t.GetInt("guardFlag");
                hitDef.hitType = t.GetInt("hitType");
                hitDef.knockBackType = t.GetInt("knockBackType");
                hitDef.knockBackForceLevel = t.GetInt("knockBackForceLevel");
                hitDef.knockAwayType = t.GetInt("knockAwayType");

                hitDef.hitDamage = t.GetInt("hitDamage");
                hitDef.hitPauseTime = t.GetIntArray("hitPauseTime", 2);
                hitDef.hitSlideTime = t.GetInt("hitSlideTime");
                hitDef.groundVel = t.GetNumberArray("groundVel", 2);
                hitDef.airVel = t.GetNumberArray("airVel", 2);

                hitDef.guardDamage = t.GetInt("guardDamage");
                hitDef.guardPauseTime = t.GetIntArray("guardPauseTime", 2);
                hitDef.guardSlideTime = t.GetInt("guardSlideTime");
                hitDef.guardVel = t.GetNumberArray("guardVel", 2);

                hitDef.groundCornerPush = t.GetNumber("groundCornerPush", 1);
                hitDef.airCornerPush = t.GetNumber("airCornerPush", 1);
            }   
            return hitDef;
        }

        public static int HitDefSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            HitDef hitDef = GetHitDef(lua);
            c.SetHitDefData(hitDef);
            return 0;
        }

        public static int HitBy(ILuaState lua) {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            int duration = lua.L_CheckInteger(2);
            int num = lua.L_CheckInteger(3);
            List<HitInfo> hitInfos = new List<HitInfo>();
            int index = 4;
            for (int i = 0; i < num; i++)
            {
                int hitFlag = lua.L_CheckInteger(index++);
                int hitType = lua.L_CheckInteger(index++);
                HitInfo hitInfo = new HitInfo(hitType, hitFlag);
                hitInfos.Add(hitInfo);
            }
            c.SetHitBy(hitInfos, duration);
            return 0;
        }

        public static int NoHitBy(ILuaState lua) {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            int duration = lua.L_CheckInteger(2);
            int num = lua.L_CheckInteger(3);
            List<HitInfo> hitInfos = new List<HitInfo>();
            int index = 4;
            for (int i = 0; i < num; i++)
            {
                int hitFlag = lua.L_CheckInteger(index++);
                int hitType = lua.L_CheckInteger(index++);
                HitInfo hitInfo = new HitInfo(hitType, hitFlag);
                hitInfos.Add(hitInfo);
            }
            c.SetNoHitBy(hitInfos, duration);
            return 0;
        }

        public static int PhysicsSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            int physics = lua.L_CheckInteger(2);
            c.SetPhysicsType((PhysicsType)physics);
            return 0;
        }

        public static int MoveTypeSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            int moveType = lua.L_CheckInteger(2);
            c.SetMoveType((MoveType)moveType);
            return 0;
        }

        private static Number ToNumber(double v)
        {
            return new Number((int)(v * 1000)) / new Number(1000);
        }

        public static int VelSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            double velX = lua.L_CheckNumber(2);
            double velY = lua.L_CheckNumber(3);
            c.moveCtr.VelSet(ToNumber(velX), ToNumber(velY));
            return 0;
        }

        public static int CtrlSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            bool ctrl = lua.ToBoolean(2);
            c.SetCtrl(ctrl);
            return 0;
        }

        public static int Pause(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            int duration = lua.L_CheckInteger(2);
            c.Pause(duration);
            return 0;
        }
  
    }
}
