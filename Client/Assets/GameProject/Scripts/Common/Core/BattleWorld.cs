using System;
using System.Collections;
using System.Collections.Generic;
using Mugen3D;
using FixPointMath;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.Core
{
    
    public class BattleWorld : WorldBase
    {
        #region 单例模式
        public static BattleWorld Instance { get { return m_instance; } }
        private static BattleWorld m_instance;
        //public static BattleWorld CreateInstance() { m_instance = new BattleWorld(); return m_instance; }
        #endregion

        public int BattleNo { get { return m_battleNo; } }
        public MatchState BattleState {get { return m_battleState; } }
        public int RoundNo { get { return m_roundNo; } }
        public RoundState RoundState { get { return m_roundState; } }

      

      

        private bool isPause = false;
        private int m_pauseTime = 0;

        /// <summary>
        /// 胜利次数统计
        /// </summary>
        private Dictionary<int, int> winCount = new Dictionary<int, int>();
        private readonly int MAX_WIN_COUNT = 2;


        #region system和单例组件
        /// <summary>
        /// 运动系统
        /// </summary>
        private MoveSystem m_moveSystem;
        /// <summary>
        /// 碰撞系统
        /// </summary>
        private CollideSystem m_collideSystem;
        /// <summary>
        /// 动画系统
        /// </summary>
        private AnimSystem m_animSystem;
        /// <summary>
        /// 指令系统
        /// </summary>
        private CommandSystem m_commandSystem;
        /// <summary>
        /// 摄像机系统
        /// </summary>
        private CameraSystem m_cameraSystem;
        /// <summary>
        /// 状态机系统
        /// </summary>
        private FSMSystem m_fsmSystem;
        /// <summary>
        /// 脚本系统
        /// </summary>
        private LuaScriptSystem m_luaScriptSystem;
        /// <summary>
        /// 系统列表
        /// </summary>
        private readonly List<SystemBase> m_allSystems = new List<SystemBase>();
        /// <summary>
        /// 摄像机单例组件
        /// </summary>
        private CameraComponent m_cameraComponent;
        /// <summary>
        /// 舞台单例组件
        /// </summary>
        private StageComponent m_stageComponent;
        /// <summary>
        /// 输入单例组件
        /// </summary>
        private InputComponent m_inputComponent;

        protected override List<SystemBase> AllSystem { get { return m_allSystems; } }
        #endregion

        private ConfigDataStage m_stageConfig;
        private ConfigDataCamera m_cameraConfig;
        private ConfigDataCharacter m_p1Config;
        private ConfigDataCharacter m_p2Config;
        List<ConfigDataCommand> m_commandConfigs;

        /// <summary>
        /// 战斗，比赛场次标号
        /// </summary>
        private int m_battleNo;
        /// <summary>
        /// 战斗模式
        /// </summary>
        private MatchMode m_battleMode;
        /// <summary>
        /// 战斗状态
        /// </summary>
        private MatchState m_battleState;
        /// <summary>
        /// 第几回合
        /// </summary>
        private int m_roundNo;
        /// <summary>
        /// 回合状态
        /// </summary>
        private RoundState m_roundState;

        public int[] m_cacheInputCodes;

        private IBattleWorldListener m_listener;

        private void InitSystemAndComponets()
        {
            //创建单例组件
            m_cameraComponent = CameraComponent.CreateInstance();
            m_cameraComponent.Init(m_cameraConfig);
            m_stageComponent = StageComponent.CreateInstance();
            m_stageComponent.Init(m_stageConfig);
            m_inputComponent = InputComponent.CreateInstance();
            CommandComponent.StaticInit(m_commandConfigs);
            //创建所有系统
            //m_scriptEngine = new ScriptSystem(this);
            m_animSystem = new AnimSystem();
            m_commandSystem = new CommandSystem();
            m_moveSystem = new MoveSystem();
            m_collideSystem = new CollideSystem();
            m_cameraSystem = new CameraSystem();
            m_fsmSystem = new FSMSystem();
            m_luaScriptSystem = new LuaScriptSystem();
            //加入system list
            m_allSystems.Clear();
            m_allSystems.Add(m_animSystem);
            m_allSystems.Add(m_commandSystem);
            m_allSystems.Add(m_moveSystem);
            m_allSystems.Add(m_collideSystem);
            m_allSystems.Add(m_cameraSystem);
            m_allSystems.Add(m_fsmSystem);
            m_allSystems.Add(m_luaScriptSystem);
        }

       

        public BattleWorld(List<ConfigDataCommand> configDataCommand, ConfigDataStage stageConfig, ConfigDataCamera cameraConfig, ConfigDataCharacter p1Config, ConfigDataCharacter p2Config, IBattleWorldListener listener)
        {
            m_commandConfigs = configDataCommand;
            m_stageConfig = stageConfig;
            m_cameraConfig = cameraConfig;
            m_p1Config = p1Config;
            m_p2Config = p2Config;
            m_cacheInputCodes = new int[2];
            m_listener = listener;
            InitSystemAndComponets();
        }

        public void CreateCharacters()
        {
           
        }

        #region 改变世界的方法
        /// <summary>
        /// 更新玩家输入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="inputCode"></param>
        public void UpdatePlayerInput(int index, int inputCode)
        {
            m_cacheInputCodes[index] = inputCode;
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        protected void StartBattle()
        {
            m_roundNo = 1;
            m_battleState = MatchState.Starting;
            ChangeRoundState(RoundState.PreIntro);
            m_p1 = new Character(m_p1Config,0,true,null, "","", this);
            m_listener.OnCreateCharacter(m_p1);
            m_p2 = new Character(m_p2Config,1, false, null, "", "", this);
            m_listener.OnCreateCharacter(m_p2);
            AddCharacter(m_p1);
            AddCharacter(m_p2);
            winCount[m_p1.slot] = 0;
            winCount[m_p2.slot] = 0;
            m_listener.OnBattleStart(m_battleNo);
        }

        protected void StopMatch()
        {
            m_battleState = MatchState.Stoping;
            m_listener.OnBattleEnd(m_battleNo);
        }

        #endregion

        #region roundState 更新

        //todo 写入配置表
        private readonly Number FADE_IN_TIME = new Number(1);
        private readonly Number FADE_OUT_TIME = new Number(1);
        private readonly Number ROUND_DECLARATION_TIME = new Number(3);
        private readonly Number PRE_OVER_TIME = new Number(3);
        private readonly Number OVER_TIME = new Number(3);

        private Number m_roundStateTimer = Number.Zero;

        private bool IsCharactersReady()
        {
            return m_p1.fsmMgr.stateNo == 0 && m_p2.fsmMgr.stateNo == 0;
        }

        private bool IsCharactersSteady()
        {
            return (!m_p1.IsAlive() || (m_p1.IsAlive() && m_p1.fsmMgr.stateNo == 0)) && (!m_p2.IsAlive() || (m_p2.IsAlive() && m_p2.fsmMgr.stateNo == 0));
        }

        private bool IsRoundEnd()
        {
            if (!m_p1.IsAlive() || !m_p2.IsAlive())
                return true;
            if (m_roundStateTimer <= 0)
                return true;
            return false;
        }

        private void UpdateRoundState()
        {
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
        }

        protected void StopRound()
        {
            m_listener.OnRoundEnd(m_roundNo);
        }

        protected void ChangeRoundState(RoundState roundState)
        {
            if(m_roundState == roundState)
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

        #endregion


        public void AddCharacter(ConfigDataCharacter configDataCharacter)
        {
            
        }

        protected override void OnStep()
        {
            base.OnStep();
        }

    }
}
