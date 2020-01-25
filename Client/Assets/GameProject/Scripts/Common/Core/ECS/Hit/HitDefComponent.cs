using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 打击类型
    /// </summary>
    public enum HitType
    {
        /// <summary>
        /// 打击技
        /// </summary>
        Attack = 0,
        /// <summary>
        /// 抓取技
        /// </summary>
        Throw,
    }

    /// <summary>
    /// 指示攻击必须如何防御
    /// </summary>
    public enum GuardFlag
    {
        /// <summary>
        /// 不能被防御
        /// </summary>
        None,
        /// <summary>
        /// 正常，拉后防御
        /// </summary>
        Normal,
    }

    /// <summary>
    /// 指示攻击对于那些状态的敌人生效
    /// </summary>
    public class HitFlag
    {
        /// <summary>
        /// 攻击高位：需要站立防守
        /// </summary>
        public const int H = 1 << 0;
        /// <summary>
        /// 攻击低位：需要蹲下防守
        /// </summary>
        public const int L = 1 << 1;
        /// <summary>
        /// 攻击空中的敌人
        /// </summary>
        public const int A = 1 << 2;
        /// <summary>
        /// 攻击下落过程中的敌人
        /// </summary>
        public const int F = 1 << 3;
        /// <summary>
        /// 攻击躺在地上的敌人
        /// </summary>
        public const int D = 1 << 4;
    }

    /// <summary>
    /// 打击定义数据
    /// </summary>
    public class HitDefData
    {
        /// <summary>
        /// 打击类型：打击和抓取
        /// </summary>
        public HitType hitType;
        /// <summary>
        /// HitFlag的位组合，指示对哪些状态的敌人生效
        /// </summary>
        public int hitFlags;
        /// <summary>
        /// 指示如何进行防御
        /// </summary>
        public GuardFlag guardFlag;
        /// <summary>
        /// 击中伤害
        /// </summary>
        public int hitDamage;
        /// <summary>
        /// 击中暂停时间，自身和敌人，length = 2
        /// </summary>
        public int[] hitPauseTime;
        /// <summary>
        /// 暂定结束后，敌人的向后滑行的时间
        /// </summary>
        public int hitSlideTime;
        /// <summary>
        /// 敌人在地面时，给予敌人的速度
        /// </summary>
        public Vector groundVel;
        /// <summary>
        /// 敌人在空中时，给予敌人的速度
        /// </summary>
        public Vector airVel;

        /// <summary>
        /// 防御时造成的伤害
        /// </summary>
        public int guardDamage;
        /// <summary>
        /// 防御暂停时间
        /// </summary>
        public int[] guardPauseTime;
        /// <summary>
        /// 防御后的滑行时间
        /// </summary>
        public int guardSlideTime;
        /// <summary>
        /// 敌人在地面时给予的速度
        /// </summary>
        public Vector guardVel;

        /// <summary>
        /// 在角落击中(被防御)敌人给予自身的后退速度的计算系数
        /// </summary>
        public Number groundCornerPush;
        /// <summary>
        /// 在控制击中(被防御)敌人给予自身的后退速度的计算系数
        /// </summary>
        public Number airCornerPush;

        //params for throw
        /// <summary>
        /// 击中后自身转到的状态标号
        /// </summary>
        public int p1StateNo;
        /// <summary>
        /// 击中后敌人转到的状态编号
        /// </summary>
        public int p2StateNo;

    }

    /// <summary>
    /// 打击有关数据
    /// </summary>
    public class HitComponent : ComponentBase
    {
       public bool MoveContact { get { return m_moveContact; } }
        public bool MoveHit { get { return m_moveHit; } }
        public bool MoveGuarded { get { return m_moveGuarded; } }
        public HitDefData HitDef { get { return m_hitDefData; } }
        public HitDefData BeHitData { get { return m_beHitDefData; } }
        public int ContinueBeHitCount { get { return m_beHitCount; }}

        /// <summary>
        /// 连击计数
        /// </summary>
        private int m_beHitCount; 
        /// <summary>
        /// 打击定义数据
        /// </summary>
        private HitDefData m_hitDefData = new HitDefData();

        private bool m_moveHit = false;
        private bool m_moveGuarded = false;
        private bool m_moveContact = false;

        /// <summary>
        /// 计时器
        /// </summary>
        private int m_timer;//[0,duration]
        /// <summary>
        /// 被攻击时的对方的打击定义数据
        /// </summary>
        private HitDefData m_beHitDefData = new HitDefData();
        
        public bool IsActive()
        {
            return m_timer > 0 && !m_moveContact;
        }

        public void Update()
        {
            if (m_timer > 0)
            {
                m_timer--;
            }  
        }

        public void SetHitDef(HitDefData hitDef,int duration)
        {
            m_hitDefData = hitDef;
            m_timer = duration;
            m_moveContact = false;
            m_moveGuarded = false;
            m_moveHit = false;
        }

        public void SetBeHitDef(HitDefData hitDef)
        {
            m_beHitDefData = hitDef;
        }

        public void AddBeHitCount()
        {
            this.m_beHitCount++;
        }

        public void ClearBeHitCount()
        {
            this.m_beHitCount = 0;
        }

        public void OnMoveHit()
        {
            m_moveContact = true;
            m_moveGuarded = false;
            m_moveHit = true;
        }

        public void OnMoveGuarded()
        {
            m_moveContact = true;
            m_moveGuarded = true;
            m_moveHit = false;
        }
    }
}
