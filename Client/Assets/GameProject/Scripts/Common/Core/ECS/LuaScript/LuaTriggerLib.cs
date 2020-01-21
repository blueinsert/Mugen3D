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
                new NameFuncPair("NumHelper", NumHelper),
                new NameFuncPair("NumProj", NumProj),
                new NameFuncPair("NumProjID", NumProjID),
                new NameFuncPair("IsHelper", IsHelper),
                new NameFuncPair("Parent", GetParent),
                new NameFuncPair("Ctrl", IsCtrl),
                new NameFuncPair("CanAttack", CanAttack),
                new NameFuncPair("CommandTest", CommandTest),
                new NameFuncPair("Facing", Facing),
                new NameFuncPair("MoveType", GetMoveType),
                new NameFuncPair("PhysicsType", GetPhysicsType),
                new NameFuncPair("JustOnGround", IsJustOnGround),

                new NameFuncPair("Anim", GetAnim),
                new NameFuncPair("AnimExist", IsAnimExist),
                new NameFuncPair("AnimTime", GetAnimTime),
                new NameFuncPair("AnimElem", GetAnimElem),
                new NameFuncPair("AnimElemTime", GetAnimElemTime),
                new NameFuncPair("LeftAnimTime", GetLeftAnimTime),
                new NameFuncPair("Vel", GetVel),
                new NameFuncPair("Pos", GetPos),
                new NameFuncPair("P2Dist", P2Dist),
                new NameFuncPair("P2MoveType", P2MoveType),
                new NameFuncPair("GetHitVar", GetHitVar),
                new NameFuncPair("HitPauseTime", HitPauseTime),
                new NameFuncPair("FrontEdgeDist", FrontStageDist),
                new NameFuncPair("BackEdgeDist", BackStageDist),
                new NameFuncPair("FrontStageDist", FrontStageDist),
                new NameFuncPair("BackStageDist", BackStageDist),
                new NameFuncPair("MoveContact", MoveContact),
                new NameFuncPair("MoveGuarded", MoveGuarded),
                new NameFuncPair("MoveHit", MoveHit),
                new NameFuncPair("Life", GetHP),
                new NameFuncPair("LifeMax", GetMaxHP),
                new NameFuncPair("Alive", IsAlive),
                new NameFuncPair("MatchNo", MatchNo),
                new NameFuncPair("RoundNo", RoundNo),
            };
            lua.L_NewLib(define);
            return 1;
        }

        /// <summary>
        /// 获取角色拥有的helper的数量
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int NumHelper(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushInteger(0);
            return 1;
        }

        /// <summary>
        /// 获取角色拥有的飞行道具的数量
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int NumProj(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushInteger(0);
            return 1;
        }

        /// <summary>
        /// 获取角色拥有的指定id的飞行道具的数量
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int NumProjID(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushInteger(0);
            return 1;
        }

        /// <summary>
        /// 判断自身是否是Helper
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int IsHelper(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushBoolean(false);
            return 1;
        }


        public static int GetParent(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            lua.PushNil();
            return 1;
        }

        public static int FrontStageDist(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var dist = UtilityFuncs.GetFrontStageDist(c);
            lua.PushInteger(dist.AsInt());
            return 1;
        }

        public static int BackStageDist(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var dist = UtilityFuncs.GetBackStageDist(c);
            lua.PushInteger(dist.AsInt());
            return 1;
        }

        public static int IsCtrl(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushBoolean(true);
            return 1;
        }

        public static int CanAttack(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushBoolean(true);
            return 1;
        }

        public static int CommandTest(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            string command = lua.L_CheckString(2);
            bool res = false;
            var commandComponent = c.GetComponent<CommandComponent>();
            if (commandComponent != null)
            {
                res = commandComponent.CommandIsActive(command);
            }
            lua.PushBoolean(res);
            return 1;
        }

        public static int Facing(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var moveComponent = c.GetComponent<PlayerComponent>();
            int facing = moveComponent.Facing;
            lua.PushInteger(facing);
            return 1;
        }

        /// <summary>
        /// 获取角色的行为类型
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int GetMoveType(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            int moveType = 0;
            var hitComponent = c.GetComponent<HitComponent>();
            if (hitComponent != null)
            {
                moveType = (int)hitComponent.MoveType;
            }
            lua.PushInteger(moveType);
            return 1;
        }

        /// <summary>
        /// 获取对手的行为类型
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int P2MoveType(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushInteger(0);
            return 1;
        }

        /// <summary>
        /// 获取角色的物理类型
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int GetPhysicsType(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            int physicsType = 0;
            var moveComponent = c.GetComponent<PhysicsComponent>();
            if (moveComponent != null)
            {
                physicsType = (int)moveComponent.PhysicsType;
            }
            lua.PushInteger(physicsType);
            return 1;
        }

        /// <summary>
        /// 是否即将着陆
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int IsJustOnGround(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            //todo
            lua.PushBoolean(false);
            return 1;
        }

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

        /// <summary>
        /// 获取当前的动画编号
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int GetAnim(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var animComponent = c.GetComponent<AnimationComponent>();
            int anim = 0;
            if (animComponent != null)
            {
                anim = animComponent.Anim;
            }
            lua.PushInteger(anim);
            return 1;
        }

        /// <summary>
        /// 查询指定编号的动画是否存在
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int IsAnimExist(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var animComponent = c.GetComponent<AnimationComponent>();
            int animNo = lua.L_CheckInteger(2);
            var isExist = false;
            if (animComponent != null)
            {
                isExist = animComponent.IsAnimExist(animNo);
            }
            lua.PushBoolean(isExist);
            return 1;
        }

        /// <summary>
        /// 获取该动画的已持续时间
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int GetAnimTime(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var animComponent = c.GetComponent<AnimationComponent>();
            int animTime = 0;
            if (animComponent != null)
            {
                animTime = animComponent.AnimTime;
            }
            lua.PushInteger(animTime);
            return 1;
        }

        /// <summary>
        /// 获取当前动画帧编号
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int GetAnimElem(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var animComponent = c.GetComponent<AnimationComponent>();
            int animElem = 0;
            if (animComponent != null)
            {
                animElem = animComponent.AnimElem;
            }
            lua.PushInteger(animElem);
            return 1;
        }

        /// <summary>
        /// 获取该帧的已持续时间
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int GetAnimElemTime(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var animComponent = c.GetComponent<AnimationComponent>();
            var animElemTime = 0;
            if (animComponent != null)
            {
                animElemTime = animComponent.AnimElemTime;
            }
            lua.PushInteger(animElemTime);
            return 1;
        }

        /// <summary>
        /// 获得剩余动画时间
        /// </summary>
        /// <param name="lua"></param>
        /// <returns></returns>
        public static int GetLeftAnimTime(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            var animComponent = c.GetComponent<AnimationComponent>();
            int leftTime = 0;
            if (animComponent != null)
            {
                leftTime = animComponent.LeftAnimTime;
            }
            lua.PushInteger(leftTime);
            return 1;
        }

        public static int GetVel(ILuaState lua)
        {
            lua.L_CheckType(1, LuaType.LUA_TLIGHTUSERDATA);
            Entity c = (Entity)lua.ToUserData(1);
            MoveComponent moveComponent = c.GetComponent<MoveComponent>();
            Number x = 0;
            Number y = 0;
            if (moveComponent != null)
            {
                var pos = moveComponent.Velocity;
                x = pos.x;
                y = pos.y;
            }
            lua.PushInteger(x.AsInt());
            lua.PushInteger(y.AsInt());
            return 2;
        }

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
