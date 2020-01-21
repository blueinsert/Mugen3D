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

        private PlayMode m_playMode;

        private IBattleWorldListener m_listener;

        public BattleWorld(ConfigDataStage stageConfig, ConfigDataCamera cameraConfig, ConfigDataCharacter[] characterConfigs, 
            List<ConfigDataCommand> configDataCommand, PlayMode playMode, IBattleWorldListener listener)
        {
            m_playMode = playMode;
            //更新配置
            m_characterConfigs = characterConfigs;
            m_stageConfig = stageConfig;
            m_cameraConfig = cameraConfig;
            m_cacheInputCodes = new int[m_characterConfigs.Length];
            m_commandConfigs = configDataCommand;
            m_listener = listener;
            InitializeBattleWorld();
            for(int i = 0; i < m_characterConfigs.Length; i++)
            {
                AddCharacter(m_characterConfigs[i], i);
            }  
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
        public void StartMatch()
        {
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
