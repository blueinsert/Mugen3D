﻿using System.Collections;
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