using System;
using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace bluebean.Mugen3D.Core
{
    public static class LuaControllerLib 
    {
        public const string LIB_NAME = "controller.cs";

        public static int OpenLib(ILuaState lua)
        {
            var define = new NameFuncPair[] {
                new NameFuncPair("CreateHelper", CreateHelper),
                new NameFuncPair("CreateProjectile", CreateProjectile),
                new NameFuncPair("DestroySelf", DestroySelf),
                new NameFuncPair("ChangeState", ChangeState),
                new NameFuncPair("ChangeAnim", ChangeAnim),
                new NameFuncPair("ChangeFacing", ChangeFacing),
                new NameFuncPair("HitBy", HitBy),
                new NameFuncPair("NoHitBy", NoHitBy),
                new NameFuncPair("HitDefSet", HitDefSet),
                new NameFuncPair("PhysicsSet", PhysicsSet),
                new NameFuncPair("MoveTypeSet", MoveTypeSet),
                new NameFuncPair("VelSet", VelSet),
                new NameFuncPair("VelAdd", VelAdd),
                new NameFuncPair("CtrlSet", CtrlSet),
                new NameFuncPair("PosSet", PosSet),
                new NameFuncPair("PosAdd", PosAdd),
                new NameFuncPair("Pause", Pause),
                new NameFuncPair("TargetBind", TargetBind),
                new NameFuncPair("MakeEffect", MakeEffect),
                new NameFuncPair("PlaySound", PlaySound),
            };
            lua.L_NewLib(define);
            return 1;
        }

        public static int CreateProjectile(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit u = (Unit)lua.ToUserData(1);
            if (u is Character)
            {
                var c = u as Character;
                var projectileName = lua.L_CheckString(2);
                ProjectileDef def = new ProjectileDef();
                using (var t = new LuaTable(lua)) {
                    def.offset = t.GetNumberArray("offset", 2);
                    def.posType = t.GetString("posType");
                    def.projPriority = t.GetInt("projPriority");
                    def.projEdgeBound = t.GetNumber("projEdgeBound", 0);
                    def.projStageBound = t.GetNumber("projStageBound", 0);
                    def.facing = t.GetInt("facing", 1);
                    def.vel = t.GetNumberArray("vel", 2);
                    def.id = t.GetInt("id");
                }
                c.CreateProjectile(projectileName, def);
            }
            return 0;
        }

        public static int CreateHelper(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit u = (Unit)lua.ToUserData(1);
            if (u is Character)
            {
                var c = u as Character;
                var helperName = lua.L_CheckString(2);
                c.CreateHelper(helperName);
            }
            return 0;
        }

        public static int DestroySelf(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit u = (Unit)lua.ToUserData(1);
            if (u is Helper)
            {
                u.Destroy();
            }
            return 0;
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
                hitDef.forceLevel = t.GetInt("forceLevel");
                hitDef.groundType = t.GetInt("groundType");
                hitDef.knockAwayType = t.GetInt("knockAwayType", -1);
                hitDef.p1StateNo = t.GetInt("p1StateNo", 0);
                hitDef.p2StateNo = t.GetInt("p2StateNo", 0);

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

                hitDef.spark = t.GetString("spark");
                hitDef.guardSpark = t.GetString("guardSpark");
                hitDef.sparkPos = t.GetNumberArray("sparkPos", 2);

                hitDef.hitSound = t.GetString("hitSound");
                hitDef.guardSound = t.GetString("guardSound");
            }   
            return hitDef;
        }

        public static int HitDefSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            if (c.IsPause())
                return 0;
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

        public static int VelSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            double velX = lua.L_CheckNumber(2);
            double velY = lua.L_CheckNumber(3);
            c.moveCtr.VelSet(velX.ToNumber(), velY.ToNumber());
            return 0;
        }

        public static int VelAdd(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            double velX = lua.L_CheckNumber(2);
            double velY = lua.L_CheckNumber(3);
            c.moveCtr.VelAdd(velX.ToNumber(), velY.ToNumber());
            return 0;
        }

        public static int PosSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            double x = lua.L_CheckNumber(2);
            double y = lua.L_CheckNumber(3);
            c.moveCtr.PosSet(x.ToNumber(), y.ToNumber());
            return 0;
        }

        public static int PosAdd(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            double x = lua.L_CheckNumber(2);
            double y = lua.L_CheckNumber(3);
            double z = lua.L_CheckNumber(4);
            c.moveCtr.PosAdd(x.ToNumber(), y.ToNumber());
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

        public static int TargetBind(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            Unit target = c.GetHitDefData().target;
            if (target != null)
            {
                double x = lua.L_CheckNumber(2);
                double y = lua.L_CheckNumber(3);
                double z = lua.L_CheckNumber(4);
                target.moveCtr.PosSet(c.position.x + x.ToNumber()*c.GetFacing(), c.position.y + y.ToNumber());
            }
            return 0;
        }

        public static int BindToTarget(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            Unit target = c.GetHitDefData().target;
            if (target != null)
            {
                double x = lua.L_CheckNumber(2);
                double y = lua.L_CheckNumber(3);
                double z = lua.L_CheckNumber(4);
                c.moveCtr.PosSet(target.position.x + x.ToNumber() * target.GetFacing(), target.position.y + y.ToNumber());
            }
            return 0;
        }

        public static int MakeEffect(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            string effectName = lua.L_CheckString(2);
            EffectDef def = new EffectDef();
            using(var t = new LuaTable(lua))
            {
                def.posType = t.GetString("posType");
                def.pos = t.GetNumberArray("pos", 2);
                def.facing = t.GetInt("facing", 1);
                def.bindTime = t.GetInt("bindTime");
                def.vel = t.GetNumberArray("vel", 2);
                def.accel = t.GetNumberArray("accel", 2);
                def.removeTime = t.GetInt("removeTime");
                def.pauseMoveTime = t.GetInt("pauseMoveTime");
                def.superPauseMoveTime = t.GetInt("superPauseMoveTime");
            }
            def.name = effectName;
            c.SendEvent(new Event { type = EventType.PlayEffect, data = def });
            return 0;
        }

        public static int PlaySound(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Unit c = (Unit)lua.ToUserData(1);
            string soundName = lua.L_CheckString(2);
            SoundDef def = new SoundDef();
            using(var t = new LuaTable(lua))
            {
                def.delay = t.GetNumber("delay", 0);
                def.volume = t.GetNumber("volume", 1);
            }
            def.name = soundName;
            c.SendEvent(new Event() { type = EventType.PlaySound, data = def });
            return 0;
        }

    }
}
