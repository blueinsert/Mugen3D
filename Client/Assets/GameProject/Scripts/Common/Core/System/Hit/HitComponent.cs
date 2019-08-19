using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 当前的动作类型
    /// </summary>
    public enum MoveType
    {
        Attack = 1,
        Idle,
        Defence,
        BeingHitted,
    }

    /// <summary>
    /// 打击类型
    /// </summary>
    public enum HitType
    {
        Attack = 0,
        Throw,
    }

    /// <summary>
    /// 防御类型
    /// </summary>
    public enum GuardFlag
    {
        H = 1 << 0,
        L = 1 << 1,
    }

    public enum HitFlag
    {
        H = 1 << 0,
        L = 1 << 1,
        A = 1 << 2,
        F = 1 << 3,
        D = 1 << 4,
    }

    public enum GroundType
    {
        High = 0,
        Low,
    }

    /// <summary>
    /// 打击定义数据
    /// </summary>
    public class HitDef
    {
        //public Unit owner;
        //public Unit target;
        public int hitFlag;
        public int guardFlag;

        public int hitType;
        //params for attack, knock back
        public int forceLevel;
        public int groundType;
        //params for attack, knock away 
        public int knockAwayType = -1;

        public int hitDamage;
        public int[] hitPauseTime;
        public int hitSlideTime;
        public Number[] groundVel;
        public Number[] airVel;

        public int guardDamage;
        public int[] guardPauseTime;
        public int guardSlideTime;
        public Number[] guardVel;

        public Number groundCornerPush;
        public Number airCornerPush;

        //params for throw
        public int p1StateNo;
        public int p2StateNo;

        public string spark;
        public string guardSpark;
        public Number[] sparkPos;

        public string hitSound;
        public string guardSound;

        public bool moveHit = false;
        public bool moveGuarded = false;
        public bool moveContact = false;

        public HitDef()
        {
        }
    }

    public class HitComponent : ComponentBase
    {
        public MoveType MoveType{ get { return m_moveType; } }
        public HitDef HitDef { get { return m_hitDefData; } }
        public HitBy HitBy { get { return m_hitBy; } }
        public NoHitBy NoHitBy { get { return m_noHitBy; } }
        /// <summary>
        /// 当前的动作类型
        /// </summary>
        private MoveType m_moveType = MoveType.Idle;
        /// <summary>
        /// 打击定义数据
        /// </summary>
        private HitDef m_hitDefData;
        /// <summary>
        /// 被攻击时的对方的打击定义数据
        /// </summary>
        private HitDef m_beHitDefData;
        /// <summary>
        /// 有效打击的定义
        /// </summary>
        private HitBy m_hitBy;
        /// <summary>
        /// 免疫攻击的定义
        /// </summary>
        private NoHitBy m_noHitBy;

        public void Update()
        {
            if (m_hitBy != null)
                m_hitBy.Update();
            if (m_noHitBy != null)
                m_noHitBy.Update();
        }
    }
}
