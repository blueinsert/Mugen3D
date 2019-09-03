using System;
using System.Collections;
using System.Collections.Generic;
using UniLua;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 提供给lua脚本修改游戏状态的Library
    /// </summary>
    public static class LuaControllerLib 
    {
        public const string LIB_NAME = "controller.cs";

        public static int OpenLib(ILuaState lua)
        {
            var define = new NameFuncPair[] {
                new NameFuncPair("ChangeState", ChangeState),
                new NameFuncPair("ChangeFacing", ChangeFacing),
                new NameFuncPair("ChangeAnim", ChangeAnim),
                new NameFuncPair("VelSet", VelSet),
                new NameFuncPair("VelAdd", VelAdd),
                new NameFuncPair("PosSet", PosSet),
                new NameFuncPair("PosAdd", PosAdd),

                new NameFuncPair("CreateHelper", CreateHelper),
                new NameFuncPair("CreateProjectile", CreateProjectile),
                new NameFuncPair("DestroySelf", DestroySelf),


                new NameFuncPair("HitBy", HitBy),
                new NameFuncPair("NoHitBy", NoHitBy),
                new NameFuncPair("HitDefSet", HitDefSet),
                new NameFuncPair("PhysicsSet", PhysicsSet),
                new NameFuncPair("MoveTypeSet", MoveTypeSet),

                new NameFuncPair("CtrlSet", CtrlSet),

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
            //todo
            return 0;
        }

        public static int CreateHelper(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        public static int DestroySelf(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        public static int ChangeState(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            int stateNo = lua.L_CheckInteger(2);
            var delayImpactComponent = c.GetComponent<DelayImpactComponent>();
            if (delayImpactComponent != null)
            {
                delayImpactComponent.ChangeState(stateNo);
            }
            return 0;
        }

        public static int ChangeFacing(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            int facing = lua.L_CheckInteger(2);
            DelayImpactComponent delayImpact = c.GetComponent<DelayImpactComponent>();
            if (delayImpact != null)
            {
                delayImpact.ChangeFacing(facing);
            }
            return 0;
        }

        public static int ChangeAnim(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            int anim = lua.L_CheckInteger(2);
            DelayImpactComponent delayImpact = c.GetComponent<DelayImpactComponent>();
            if (delayImpact != null)
            {
                delayImpact.ChangeAnim(anim);
            }
            return 0;
        }

        //static HitDef GetHitDef(ILuaState lua)
        //{
        //    HitDef hitDef = new HitDef();
        //    using(var t = new LuaTable(lua)){
        //        hitDef.hitFlag = t.GetInt("hitFlag");
        //        hitDef.guardFlag = t.GetInt("guardFlag");

        //        hitDef.hitType = t.GetInt("hitType");
        //        hitDef.forceLevel = t.GetInt("forceLevel");
        //        hitDef.groundType = t.GetInt("groundType");
        //        hitDef.knockAwayType = t.GetInt("knockAwayType", -1);
        //        hitDef.p1StateNo = t.GetInt("p1StateNo", 0);
        //        hitDef.p2StateNo = t.GetInt("p2StateNo", 0);

        //        hitDef.hitDamage = t.GetInt("hitDamage");
        //        hitDef.hitPauseTime = t.GetIntArray("hitPauseTime", 2);
        //        hitDef.hitSlideTime = t.GetInt("hitSlideTime");
        //        hitDef.groundVel = t.GetNumberArray("groundVel", 2);
        //        hitDef.airVel = t.GetNumberArray("airVel", 2);

        //        hitDef.guardDamage = t.GetInt("guardDamage");
        //        hitDef.guardPauseTime = t.GetIntArray("guardPauseTime", 2);
        //        hitDef.guardSlideTime = t.GetInt("guardSlideTime");
        //        hitDef.guardVel = t.GetNumberArray("guardVel", 2);

        //        hitDef.groundCornerPush = t.GetNumber("groundCornerPush", 1);
        //        hitDef.airCornerPush = t.GetNumber("airCornerPush", 1);

        //        hitDef.spark = t.GetString("spark");
        //        hitDef.guardSpark = t.GetString("guardSpark");
        //        hitDef.sparkPos = t.GetNumberArray("sparkPos", 2);

        //        hitDef.hitSound = t.GetString("hitSound");
        //        hitDef.guardSound = t.GetString("guardSound");
        //    }   
        //    return hitDef;
        //}

        public static int HitDefSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        public static int HitBy(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        public static int NoHitBy(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        public static int PhysicsSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        public static int MoveTypeSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        public static int VelSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            DelayImpactComponent delayImpact = c.GetComponent<DelayImpactComponent>();
            if (delayImpact != null)
            {
                int velX = lua.L_CheckInteger(2);
                int velY = lua.L_CheckInteger(3);
                delayImpact.VelSet(new Vector(Number.EN4 * velX, Number.EN4 * velY));
            }
            return 0;
        }

        public static int VelAdd(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            DelayImpactComponent delayImpact = c.GetComponent<DelayImpactComponent>();
            if (delayImpact != null)
            {
                int velX = lua.L_CheckInteger(2);
                int velY = lua.L_CheckInteger(3);
                delayImpact.VelAdd(new Vector(Number.EN4 * velX, Number.EN4 * velY));
            }
            return 0;
        }

        public static int PosSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            DelayImpactComponent delayImpact = c.GetComponent<DelayImpactComponent>();
            if (delayImpact != null)
            {
                int x = lua.L_CheckInteger(2);
                int y = lua.L_CheckInteger(3);
                delayImpact.PosSet(new Vector(Number.EN4 * x, Number.EN4 * y));
            }
            return 0;
        }

        public static int PosAdd(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            DelayImpactComponent delayImpact = c.GetComponent<DelayImpactComponent>();
            if (delayImpact != null)
            {
                int x = lua.L_CheckInteger(2);
                int y = lua.L_CheckInteger(3);
                delayImpact.PosAdd(new Vector(Number.EN4 * x, Number.EN4 * y));
            }
            return 0;
        }

        public static int CtrlSet(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        public static int Pause(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        public static int TargetBind(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        public static int BindToTarget(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int MakeEffect(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int PlaySound(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            return 0;
        }

    }
}
