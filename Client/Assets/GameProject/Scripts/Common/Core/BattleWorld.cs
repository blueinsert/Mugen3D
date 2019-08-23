using System;
using System.Collections;
using System.Collections.Generic;
using Mugen3D;
using FixPointMath;
using bluebean.UGFramework.ConfigData;
using UniLua;

namespace bluebean.Mugen3D.Core
{
    
    public class BattleWorld : WorldBase
    {

        protected override List<SystemBase> AllSystem { get { return m_allSystems; } }

        private List<ConfigDataCommand> m_commandConfigs;
        private ConfigDataStage m_stageConfig;
        private ConfigDataCamera m_cameraConfig;
        private ConfigDataCharacter m_p1Config;
        private ConfigDataCharacter m_p2Config;

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

        #endregion
        private static bool m_hasStaticInit = false;

        public int[] m_cacheInputCodes;

        private IBattleWorldListener m_listener;

        private void InitSystemAndComponets()
        {
            //创建单例组件
            m_cameraComponent = CameraComponent.CreateInstance();
            m_cameraComponent.Init(m_cameraConfig);
            m_stageComponent = StageComponent.CreateInstance();
            m_stageComponent.Init(m_stageConfig);
            m_inputComponent = new InputComponent(this);
            CommandComponent.StaticInit(m_commandConfigs);
            //创建所有系统
            m_animSystem = new AnimSystem();
            m_commandSystem = new CommandSystem();
            m_moveSystem = new MoveSystem();
            m_collideSystem = new CollideSystem();
            m_cameraSystem = new CameraSystem();
            m_fsmSystem = new FSMSystem();
            m_luaScriptSystem = new LuaScriptSystem();
            //加入system list
            m_allSystems.Clear();
            m_allSystems.AddRange(new List<SystemBase>(){ m_commandSystem });
            //m_allSystems.Add(m_animSystem);
            //m_allSystems.Add(m_commandSystem);
            //m_allSystems.Add(m_moveSystem);
           // m_allSystems.Add(m_collideSystem);
            //m_allSystems.Add(m_cameraSystem);
            //m_allSystems.Add(m_fsmSystem);
            //m_allSystems.Add(m_luaScriptSystem);
        }

        public static void StaticInit(CustomLoader luaFileLoader, LogDelegate logDelegate, LogDelegate logWarn, LogDelegate logError)
        {
            if (m_hasStaticInit)
                return;
            Debug.m_Log = logDelegate;
            Debug.m_LogWarn = logWarn;
            Debug.m_LogError = logError;
            //Core.Debug.Assert = Debug.Assert;
            Core.LuaMgr.AddLoader(luaFileLoader);
            //Core.FileReader.AddReader(FileRead);
            m_hasStaticInit = true;
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
            CreateCharacters(m_p1Config);
        }


        public Entity CreateCharacters(ConfigDataCharacter configDataCharacter)
        {
            var character = new Entity(m_maxEntityId++);
            character.AddComponent<MoveComponent>();
            return character;
        }

        #region 改变世界的方法
       
        /// <summary>
        /// 更新玩家输入
        /// </summary>
        /// <param name="inputCodes"></param>
        public void UpdatePlayerInput(int[] inputCodes)
        {
            for(int i = 0; i < m_cacheInputCodes.Length; i++)
            {
                if (i < inputCodes.Length)
                {
                    m_cacheInputCodes[i] = inputCodes[i];
                }
            }
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        protected void StartBattle()
        {
           
        }

        protected void StopMatch()
        {
           
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
