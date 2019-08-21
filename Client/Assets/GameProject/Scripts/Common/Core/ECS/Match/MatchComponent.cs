using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 回合状态
    /// </summary>
    public enum RoundState
    {
        PreIntro = 0,
        Intro,//第一回合的出场白
        RoundDeclare,//回合开始
        Fight,//开始战斗
        PreOver,
        Over,
        PostOver,
    }

    /// <summary>
    /// 战斗、比赛状态
    /// </summary>
    public enum MatchState
    {
        None,
        Starting,
        Running,
        Stoping,
        Stoped
    }

    /// <summary>
    /// 战斗模式，玩法
    /// </summary>
    public enum MatchMode
    {
        SinglePlay,
        SingleVS,
        TeamPlay,
        TeamVS,
    }

    public class MatchComponent:ComponentBase
    {
        #region 单例模式
        public static MatchComponent Instance { get { return m_instance; } }
        private static MatchComponent m_instance;
        public static MatchComponent CreateInstance() { m_instance = new MatchComponent(); return m_instance; }
        #endregion

        public int MatchNo { get { return m_matchNo; } }
        public int RoundNo { get { return m_roundNo; } }
        public RoundState RoundState { get { return m_roundState; } }
       
        /// <summary>
        /// 比赛状态
        /// </summary>
        private MatchState m_matchState;
        /// <summary>
        /// 比赛场次标号
        /// </summary>
        private int m_matchNo;
        /// <summary>
        /// 第几回合
        /// </summary>
        private int m_roundNo;
        /// <summary>
        /// 回合状态
        /// </summary>
        private RoundState m_roundState;

        public void SetRoundState(RoundState roundState)
        {
            m_roundState = roundState;
        }
    }
}
