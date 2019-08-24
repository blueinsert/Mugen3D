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

        private List<ConfigDataCommand> m_commandConfigs;
        private ConfigDataStage m_stageConfig;
        private ConfigDataCamera m_cameraConfig;
        private ConfigDataCharacter m_p1Config;
        private ConfigDataCharacter m_p2Config;

        private InputComponent m_inputComponent;

        private static bool m_hasStaticInit = false;

        public int[] m_cacheInputCodes;

        private IBattleWorldListener m_listener;

        private void InitSystemAndComponets()
        {
            CommandComponent.StaticInit(m_commandConfigs);
            //创建单例组件
            AddSingletonComponent<StageComponent>().Init(m_stageConfig);
            AddSingletonComponent<CameraComponent>().Init(m_cameraConfig);
            m_inputComponent = AddSingletonComponent<InputComponent>();
            //创建所有系统
            AddSystem<CommandSystem>();
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
            var entity = AddEntity();
            entity.AddComponent<MoveComponent>();
            entity.AddComponent<PlayerComponent>().Init(1);
            entity.AddComponent<CommandComponent>().Init();
            m_listener.OnCreateCharacter(entity);
            return entity;
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
            m_inputComponent.Update(m_cacheInputCodes[0], m_cacheInputCodes[1]);
            base.OnStep();
        }

    }
}
