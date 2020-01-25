using System;
using System.Collections;
using System.Collections.Generic;
using Mugen3D;
using FixPointMath;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.Core
{
    public delegate string CustomFileReader(string filepath);

    public partial class BattleWorld : WorldBase
    {
        /// <summary>
        /// 文件内容读取，用于读取动画定义等自定义数据内容（不适合表格化的数据），在客户端和服务器具有不同的设置
        /// </summary>
        private static CustomFileReader sFileReader;

        private List<ConfigDataCommand> m_commandConfigs;
        private ConfigDataStage m_stageConfig;
        private ConfigDataCamera m_cameraConfig;
        private ConfigDataCharacter[] m_characterConfigs;

        private MatchComponent m_matchComponent;
        private InputComponent m_inputComponent;

        private Entity[] m_characterArray = new Entity[2];

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
       
        public Entity GetEnemy(Entity e)
        {
            var basic = e.GetComponent<BasicInfoComponent>();
            return m_characterArray[1 - basic.Index];
        }
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

        protected override void OnStep()
        {
            Time.Update(Number.D60);
            m_inputComponent.Update(m_cacheInputCodes[0], m_cacheInputCodes[1]);
            base.OnStep();//变量所有系统更新组件状态
        }

    }
}
