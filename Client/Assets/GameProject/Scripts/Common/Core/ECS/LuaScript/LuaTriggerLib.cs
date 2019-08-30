using System.Collections;
using System.Collections.Generic;
using UniLua;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 提供给lua脚本查询游戏状态的Library
    /// </summary>
    public class LuaTriggerLib
    {

        public const string LIB_NAME = "trigger.cs";

        public static int OpenLib(ILuaState lua)
        {
            var define = new NameFuncPair[] {
                new NameFuncPair("StateNo", GetStateNo),
                new NameFuncPair("StateTime", GetStateTime),
                //new NameFuncPair("NumHelper", NumHelper),
                //new NameFuncPair("NumProj", NumProj),
                //new NameFuncPair("NumProjID", NumProjID),
                //new NameFuncPair("IsHelper", IsHelper),
                //new NameFuncPair("Parent", GetParent),
                //new NameFuncPair("Ctrl", IsCtrl),
                //new NameFuncPair("CanAttack", CanAttack),
                //new NameFuncPair("CommandTest", CommandTest),
                new NameFuncPair("Facing", Facing),
                //new NameFuncPair("MoveType", GetMoveType),
                //new NameFuncPair("PhysicsType", GetPhysicsType),
                //new NameFuncPair("JustOnGround", IsJustOnGround),
                
                //new NameFuncPair("Anim", GetAnim),
                //new NameFuncPair("AnimExist", IsAnimExist),
                //new NameFuncPair("AnimTime", GetAnimTime),
                //new NameFuncPair("AnimElem", GetAnimElem),
                //new NameFuncPair("AnimElemTime", GetAnimElemTime),
                //new NameFuncPair("LeftAnimTime", GetLeftAnimTime),
                //new NameFuncPair("Vel", GetVel),
                //new NameFuncPair("Pos", GetPos),
                //new NameFuncPair("P2Dist", P2Dist),
                //new NameFuncPair("P2MoveType", P2MoveType),
                //new NameFuncPair("GetHitVar", GetHitVar),
                //new NameFuncPair("HitPauseTime", HitPauseTime),
                //new NameFuncPair("FrontEdgeDist", FrontStageDist),
                //new NameFuncPair("BackEdgeDist", BackStageDist),
                //new NameFuncPair("FrontStageDist", FrontStageDist),
                //new NameFuncPair("BackStageDist", BackStageDist),
                //new NameFuncPair("MoveContact", MoveContact),
                //new NameFuncPair("MoveGuarded", MoveGuarded),
                //new NameFuncPair("MoveHit", MoveHit),
                //new NameFuncPair("Life", GetHP),
                //new NameFuncPair("LifeMax", GetMaxHP),
                //new NameFuncPair("Alive", IsAlive),
                //new NameFuncPair("MatchNo", MatchNo),
                //new NameFuncPair("RoundNo", RoundNo),
               
            };
            lua.L_NewLib(define);
            return 1;
        }


        //public static int NumHelper(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    if (u is Character)
        //    {
        //        var c = u as Character;
        //        int num = c.NumHelper();
        //        lua.PushInteger(num);
        //        return 1;
        //    }
        //    return 0;
        //}

        //public static int NumProj(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    if (u is Character)
        //    {
        //        var c = u as Character;
        //        int num = c.NumProj();
        //        lua.PushInteger(num);
        //        return 1;
        //    }
        //    return 0;
        //}

        //public static int NumProjID(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    if (u is Character)
        //    {
        //        var c = u as Character;
        //        int id = lua.L_CheckInteger(2);
        //        int num = c.NumProj(id);
        //        lua.PushInteger(num);
        //        return 1;
        //    }
        //    return 0;
        //}

        //public static int IsHelper(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    if (u is Helper)
        //    {
        //        lua.PushBoolean(true);
        //    }
        //    else
        //    {
        //        lua.PushBoolean(false);
        //    }
        //    return 1;
        //}

        //public static int GetParent(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    if (u is Helper)
        //    {
        //        var h = u as Helper;
        //        if (h.owner != null)
        //            lua.PushLightUserData(h.owner);
        //        else
        //            lua.PushNil();
        //    }
        //    else
        //    {
        //        lua.PushNil();
        //    }        
        //    return 1;
        //}

        //public static int FrontStageDist(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    var dist = u.GetFrontStageDist();
        //    lua.PushNumber(dist.AsDouble());
        //    return 1;
        //}

        //public static int BackStageDist(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    var dist = u.GetBackStageDist();
        //    lua.PushNumber(dist.AsDouble());
        //    return 1;
        //}

        //public static int IsCtrl(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    bool ctrl = u.CanCtrl();
        //    lua.PushBoolean(ctrl);
        //    return 1;
        //}

        //public static int CanAttack(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    if(u is Character)
        //    {
        //        var c = u as Character;
        //        var canAttack = c.CanAttack();
        //        lua.PushBoolean(canAttack);
        //    }
        //    else
        //    {
        //        lua.PushBoolean(true);
        //    }
        //    return 1;
        //}

        //public static int CommandTest(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    if (u is Character)
        //    {
        //        var c = u as Character;
        //        string command = lua.L_CheckString(2);
        //        bool res = c.cmdMgr.CommandIsActive(command);
        //        lua.PushBoolean(res);
        //    }
        //    else
        //    {
        //        lua.PushBoolean(false);
        //        Debug.LogError("when use CommandTest, target is not Character");
        //    } 
        //    return 1;
        //}

        //public static int ParentCommandTest(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit u = (Unit)lua.ToUserData(1);
        //    if (u is Helper)
        //    {
        //        var h = u as Helper;
        //        Character owner = h.owner;
        //        string command = lua.L_CheckString(2);
        //        bool res = owner.cmdMgr.CommandIsActive(command);
        //        lua.PushBoolean(res);
        //    }
        //    else
        //    {
        //        lua.PushBoolean(false);
        //        Debug.LogError("when use ParentCommandTest, target is not Helper");
        //    }
        //    return 1;
        //}

        public static int Facing(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var moveComponent = c.GetComponent<MoveComponent>();
            int facing = moveComponent.Facing;
            lua.PushInteger(facing);
            return 1;
        }

        //public static int GetMoveType(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    int res = (int)c.GetMoveType();
        //    lua.PushInteger(res);
        //    return 1;
        //}

        //public static int P2MoveType(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    var enemy = c.world.teamInfo.GetEnemy(c);
        //    int res = (int)enemy.GetMoveType();
        //    lua.PushInteger(res);
        //    return 1;
        //}

        //public static int GetPhysicsType(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    var physicsType = (int)c.GetPhysicsType();
        //    lua.PushInteger(physicsType);
        //    return 1;
        //}

        //public static int IsJustOnGround(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    var justOnGround = c.moveCtr.JustOnGround();
        //    lua.PushBoolean(justOnGround);
        //    return 1;
        //}

        public static int GetStateNo(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var fsmComponent = c.GetComponent<FSMComponent>();
            int stateNo = fsmComponent.StateNo;
            lua.PushInteger(stateNo);
            return 1;
        }

        public static int GetStateTime(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var fsmComponent = c.GetComponent<FSMComponent>();
            int stateTime = fsmComponent.StateTime;
            lua.PushInteger(stateTime);
            return 1;
        }

        //public static int GetAnim(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    var anim = c.animCtr.anim;
        //    lua.PushInteger(anim);
        //    return 1;
        //}

        //public static int IsAnimExist(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    int animNo = lua.L_CheckInteger(2);
        //    var isExist = c.animCtr.IsAnimExist(animNo);
        //    lua.PushBoolean(isExist);
        //    return 1;
        //}

        //public static int GetAnimTime(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    var animTime = c.animCtr.animTime;
        //    lua.PushInteger(animTime);
        //    return 1;
        //}

        //public static int GetAnimElem(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    var animElem = c.animCtr.animElem;
        //    lua.PushInteger(animElem);
        //    return 1;
        //}

        //public static int GetAnimElemTime(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    var animElemTime = c.animCtr.animElemTime;
        //    lua.PushInteger(animElemTime);
        //    return 1;
        //}

        //public static int GetLeftAnimTime(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    var leftTime = c.animCtr.animLength - c.animCtr.animTime;
        //    lua.PushInteger(leftTime);
        //    return 1;
        //}

        //public static int GetVel(ILuaState lua)
        //{
        //    lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
        //    Unit c = (Unit)lua.ToUserData(1);
        //    var vel = c.moveCtr.velocity;
        //    lua.PushNumber(vel.x.AsFloat()*c.GetFacing());
        //    lua.PushNumber(vel.y.AsFloat());
        //    return 2;
        //}

        public static int GetPos(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            MoveComponent moveComponent = c.GetComponent<MoveComponent>();
            Number x = 0;
            Number y = 0;
            if (moveComponent != null)
            {
                var pos = moveComponent.Position;
                x = pos.x;
                y = pos.y;
            }
            lua.PushInteger(x.AsInt());
            lua.PushInteger(y.AsInt());
            return 2;
        }

        /// <summary>
        /// 获取距离对手的相对位置
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int P2Dist(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushNumber(0);
            lua.PushNumber(0);
            return 2;
        }

        /// <summary>
        /// 获取打击参数
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int GetHitVar(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            string type = lua.L_CheckString(2);
            //todo
            int resNum = 0;
            return resNum;
        }

        /// <summary>
        /// 获取打击暂停时间
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int HitPauseTime(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushInteger(0);
            return 1;
        }

        /// <summary>
        /// 判断攻击是否对对手发生接触：攻击成功或被防御
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int MoveContact(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushBoolean(false);
            return 1;
        }

        /// <summary>
        /// 判断攻击是否被对手防御
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int MoveGuarded(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushBoolean(false);
            return 1;
        }

        /// <summary>
        /// 判断攻击是否击中了对手
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int MoveHit(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushBoolean(false);
            return 1;
        }

        /// <summary>
        /// 获取当前生命值
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int GetHP(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            lua.PushNumber(100);
            return 1;
        }

        /// <summary>
        /// 获取最大生命值
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int GetMaxHP(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushNumber(100);
            return 1;
        }

        /// <summary>
        /// 返回是否存活
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int IsAlive(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushBoolean(true);
            return 1;
        }

        /// <summary>
        /// 返回比赛编号
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int MatchNo(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushInteger(0);
            return 1;
        }

        /// <summary>
        /// 返回第几回合
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int RoundNo(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushInteger(0);
            return 1;
        }

    }
}
