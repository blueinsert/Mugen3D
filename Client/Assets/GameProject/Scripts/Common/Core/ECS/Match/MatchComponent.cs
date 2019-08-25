using System.Collections;
using System.Collections.Generic;
using FixPointMath;

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
       
        //todo 写入配置表
        private readonly Number FADE_IN_TIME = new Number(1);
        private readonly Number FADE_OUT_TIME = new Number(1);
        private readonly Number ROUND_DECLARATION_TIME = new Number(3);
        private readonly Number PRE_OVER_TIME = new Number(3);
        private readonly Number OVER_TIME = new Number(3);

        public int MatchNo { get { return m_matchNo; } }
        public int RoundNo { get { return m_roundNo; } }
        public RoundState RoundState { get { return m_roundState; } }
        public MatchState MatchState { get { return m_matchState; } }
        public MatchMode MatchMode { get { return m_matchMode; } }

        /// <summary>
        /// 玩法
        /// </summary>
        private MatchMode m_matchMode;
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

        private Number m_roundStateTimer = Number.Zero;

        /// <summary>
        /// 胜利次数统计
        /// </summary>
        private Dictionary<int, int> winCount = new Dictionary<int, int>();
        private readonly int MAX_WIN_COUNT = 2;

        public void SetMatchMode(MatchMode matchMode)
        {
            m_matchMode = matchMode;
        }

        public void SetMatchState(MatchState matchState)
        {
            m_matchState = matchState;
        }

        public void SetRoundState(RoundState roundState)
        {
            m_roundState = roundState;
        }

        protected void ChangeRoundState(RoundState roundState)
        {
            if (m_roundState == roundState)
            {
                return;
            }
            m_roundStateTimer = 0;
            m_roundState = roundState;
            switch (m_roundState)
            {
                case RoundState.PreIntro:
                    //p1.fsmMgr.ChangeState(0, true);
                    //p2.fsmMgr.ChangeState(0, true);
                    break;
                case RoundState.Intro:
                    //p1.fsmMgr.ChangeState(5900);
                    //p2.fsmMgr.ChangeState(5900);
                    break;
                case RoundState.Fight:
                    //p1.SetCtrl(true);
                    //p2.SetCtrl(true);
                    break;
                case RoundState.PreOver:
                    //if (!p1.IsAlive() || !p2.IsAlive())
                    //{
                    //world.Pause(30);//pause 10 frame on ko
                    //}
                    break;
                case RoundState.Over:
                    /*
                    if (p1.IsAlive())
                    {
                        if (p1 == GetWiner())
                        {
                            p1.fsmMgr.ChangeState(180);
                        }
                        else
                        {
                            p1.fsmMgr.ChangeState(170);
                        }

                    }
                    if (p2.IsAlive())
                    {
                        if (p2 == GetWiner())
                        {
                            p2.fsmMgr.ChangeState(180);
                        }
                        else
                        {
                            p2.fsmMgr.ChangeState(170);
                        }

                    }
                    */
                    break;
            }
        }
        public void UpdateRoundState()
        {
            /*
            switch (m_roundState)
            {
                case RoundState.PreIntro:
                    m_roundStateTimer += Time.deltaTime;
                    if (m_roundStateTimer >= FADE_IN_TIME)
                        ChangeRoundState(RoundState.Intro);
                    break;
                case RoundState.Intro:
                    if (IsCharactersReady())
                    {
                        ChangeRoundState(RoundState.RoundDeclare);
                    }
                    break;
                case RoundState.RoundDeclare:
                    m_roundStateTimer += Time.deltaTime;
                    if (m_roundStateTimer >= ROUND_DECLARATION_TIME)
                    {
                        ChangeRoundState(RoundState.Fight);
                    }
                    break;
                case RoundState.Fight:
                    m_roundStateTimer -= Time.deltaTime;
                    if (m_roundStateTimer < 0)
                        m_roundStateTimer = 0;
                    if (IsRoundEnd())
                    {
                        ChangeRoundState(RoundState.PreOver);
                    }
                    break;
                case RoundState.PreOver:
                    m_roundStateTimer += Time.deltaTime;
                    if (m_roundStateTimer >= PRE_OVER_TIME && IsCharactersSteady())
                        ChangeRoundState(RoundState.Over);
                    break;
                case RoundState.Over:
                    m_roundStateTimer += Time.deltaTime;
                    if (m_roundStateTimer >= OVER_TIME)
                    {
                        ChangeRoundState(RoundState.PostOver);
                    }
                    break;
                case RoundState.PostOver:
                    m_roundStateTimer += Time.deltaTime;
                    if (m_roundStateTimer >= FADE_OUT_TIME)
                    {
                        StopRound();
                    }
                    break;
            }
            */
        }
    }
}
