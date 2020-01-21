using System;
using System.Collections.Generic;
using UnityEngine;
using bluebean.Mugen3D.Core;
using bluebean.UGFramework;
using bluebean.UGFramework.ConfigData;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.Mugen3D.ClientGame
{
  
    public partial class ClientBattleWorld : IBattleWorldListener, IAssetProvider
    {

        private bool isPuase = false;

        private int m_renderFPS = 60;
        private int m_logicFPS = 60;

        private float m_gameTimeResidual = 0;
        private float m_gameDeltaTime; //core update period = 0.016s

        /// <summary>
        /// 玩家输入配置
        /// </summary>
        private List<ConfigDataInputDefault> m_inputConfigList;
        /// <summary>
        /// 角色指令配置
        /// </summary>
        private List<ConfigDataCommand> m_commandsConfig;
        /// <summary>
        /// 场景舞台配置
        /// </summary>
        private ConfigDataStage m_stageConfig;
        /// <summary>
        /// 摄像机配置
        /// </summary>
        private ConfigDataCamera m_cameraConfig;
        /// <summary>
        /// 玩家角色配置
        /// </summary>
        private ConfigDataCharacter[] m_characterConfigs;
       
        /// <summary>
        /// 资源加载接口
        /// </summary>
        private IAssetProvider m_assetProvider;

        /// <summary>
        /// 3D场景树根节点
        /// </summary>
        private GameObject m_sceneRoot;
        private GameObject m_playerRoot;
        private Camera m_battleCamera;

        /// <summary>
        /// 角色演员列表
        /// </summary>
        private readonly List<CharacterActor> m_characterActorList = new List<CharacterActor>();
        
        /// <summary>
        /// 摄像机控制器
        /// </summary>
        private CameraController m_cameraController;

        /// <summary>
        /// 纯逻辑战斗世界
        /// </summary>
        protected BattleWorld m_battleWorld;

        // <summary>
        /// 玩家输入映射配置，key是玩家id,value是键名到实际键码的映射表
        /// 比如攻击键a对应键盘"j"
        /// </summary>
        private readonly List<Dictionary<KeyNames, KeyCode>> m_playerInputMapConfigList = new List<Dictionary<Core.KeyNames, KeyCode>>();
        /// <summary>
        /// 当前帧的玩家输入
        /// </summary>
        private int[] m_playerInputCodes;


        public T GetAsset<T>(string path) where T : UnityEngine.Object
        {
            return m_assetProvider.GetAsset<T>(path);
        }

        private byte[] LuaLoader(ref string fileName)
        {
            if (!fileName.EndsWith(".txt"))
            {
                fileName = fileName + ".txt";
            }
            var code = m_assetProvider.GetAsset<TextAsset>(fileName);
            if (code != null)
            {
                return code.bytes;
            }
            return null;
        }

        /// <summary>
        /// 初始化玩家输入映射 todo 使用本地保存的设置进行初始化
        /// </summary>
        private void InitPlayerInputMapConfig(List<ConfigDataInputDefault> configs)
        {
            foreach (var config in configs)
            {
                var mapping = new Dictionary<Core.KeyNames, KeyCode>();
                mapping.Add(KeyNames.KEY_UP, (KeyCode)config.Up);
                mapping.Add(KeyNames.KEY_DOWN, (KeyCode)config.Down);
                mapping.Add(KeyNames.KEY_LEFT, (KeyCode)config.Left);
                mapping.Add(KeyNames.KEY_RIGHT, (KeyCode)config.Right);
                mapping.Add(KeyNames.KEY_BUTTON_A, (KeyCode)config.A);
                mapping.Add(KeyNames.KEY_BUTTON_B, (KeyCode)config.B);
                mapping.Add(KeyNames.KEY_BUTTON_C, (KeyCode)config.C);
                mapping.Add(KeyNames.KEY_BUTTON_X, (KeyCode)config.X);
                mapping.Add(KeyNames.KEY_BUTTON_Y, (KeyCode)config.Y);
                mapping.Add(KeyNames.KEY_BUTTON_Z, (KeyCode)config.Z);
                m_playerInputMapConfigList.Add(mapping);
            }
            m_playerInputCodes = new int[m_playerInputMapConfigList.Count];
        }

        public ClientBattleWorld(GameObject sceneRoot, ConfigDataStage stageConfig, ConfigDataCharacter[] characterConfig, Core.PlayMode playMode, IAssetProvider assetProvider,int renderFPS = 60,int logicFPS=60) {
            //初始化GameObject节点
            m_sceneRoot = sceneRoot;
            m_battleCamera = m_sceneRoot.transform.Find("BattleCamera").GetComponent<Camera>();
            m_playerRoot = m_sceneRoot.transform.Find("PlayerRoot").gameObject;
            //初始化BattleWorld静态环境
            BattleWorld.SetLogDelegate(Debug.Log, Debug.LogWarning, Debug.LogError);
            BattleWorld.SetLuaFileLoader(LuaLoader);
            //初始化配置信息
            var configLoader = ConfigDataLoader.Instance;
            m_inputConfigList = new List<ConfigDataInputDefault>(configLoader.GetAllConfigDataInputDefault().Values);
            m_inputConfigList.Sort((a, b) => { return a.ID - b.ID; });//从小到大，从id代表第几个玩家  
            m_commandsConfig = new List<ConfigDataCommand>(ConfigDataLoader.Instance.GetAllConfigDataCommand().Values);
            m_stageConfig = stageConfig;
            m_cameraConfig = ConfigDataLoader.Instance.GetConfigDataCamera(m_stageConfig.CameraConfigID);
            m_characterConfigs = characterConfig;
            //资源加载
            m_assetProvider = assetProvider;//资源加载器
            //初始化帧率信息
            m_renderFPS = renderFPS;
            m_logicFPS = logicFPS;
            Application.targetFrameRate = m_renderFPS;
            m_gameDeltaTime = (1000 / m_logicFPS) / 1000f;
            //初始化输入配置
            InitPlayerInputMapConfig(m_inputConfigList);
            //新建BattleWorld
            m_battleWorld = new BattleWorld(m_stageConfig,m_cameraConfig,m_characterConfigs,m_commandsConfig,playMode, this);
        }

        public void StartSingleVSMatch()
        {
            
            //m_battleWorld.StartSingleVSMatch(0);
        }


    }
}
