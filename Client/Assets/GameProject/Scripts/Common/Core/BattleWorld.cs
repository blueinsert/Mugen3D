using System;
using System.Collections;
using System.Collections.Generic;
using Mugen3D;
using FixPointMath;
using bluebean.UGFramework.ConfigData;
using UniLua;

namespace bluebean.Mugen3D.Core
{
    
    public partial class BattleWorld : WorldBase
    {

        private List<ConfigDataCommand> m_commandConfigs;
        private ConfigDataStage m_stageConfig;
        private ConfigDataCamera m_cameraConfig;
        private ConfigDataCharacter[] m_characterConfigs;

        private MatchComponent m_matchComponent;
        private InputComponent m_inputComponent;

        public int[] m_cacheInputCodes;

        private IBattleWorldListener m_listener;

        public BattleWorld(List<ConfigDataCommand> configDataCommand,  IBattleWorldListener listener)
        {
            m_commandConfigs = configDataCommand;
            m_listener = listener;
            InitNecessarySystemAndComponets();
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
        public void StartSingleVSMatch(ConfigDataCharacter p1CharacterConfig, ConfigDataCharacter p2CharacterConfig, ConfigDataStage stageConfig, ConfigDataCamera cameraConfig)
        {
            //更新配置
            m_characterConfigs = new ConfigDataCharacter[2] { p1CharacterConfig, p2CharacterConfig };
            m_stageConfig = stageConfig;
            m_cameraConfig = cameraConfig;
            m_cacheInputCodes = new int[2];
            //创建单例组件
            AddSingletonComponent<StageComponent>().Init(m_stageConfig);
            var cameraComponent = AddSingletonComponent<CameraComponent>().Init(m_cameraConfig);
            m_listener.OnCameraCreate(cameraComponent);
            var character1 = AddCharacter(m_characterConfigs[0], 0);
            var character2 = AddCharacter(m_characterConfigs[1], 1);
            m_matchComponent.SetMatchMode(MatchMode.SingleVS);
            m_matchComponent.SetMatchState(MatchState.None);
            m_matchComponent.SetRoundState(RoundState.PreIntro);
            m_listener.OnCreateCharacter(character1);
            m_listener.OnCreateCharacter(character2);
            m_listener.OnMatchStart(0);
        }

        protected void StopMatch()
        {
           
        }

        #endregion

        protected override void OnStep()
        {
            m_inputComponent.Update(m_cacheInputCodes[0], m_cacheInputCodes[1]);
            base.OnStep();//变量所有系统更新组件状态
        }

    }
}
