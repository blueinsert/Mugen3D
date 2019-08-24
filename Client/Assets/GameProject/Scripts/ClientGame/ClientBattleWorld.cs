using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using bluebean.Mugen3D.Core;
using bluebean.UGFramework;
using bluebean.UGFramework.ConfigData;
using Debug = bluebean.UGFramework.Log.Debug;
using FixPointMath;

namespace bluebean.Mugen3D.ClientGame
{
    public enum PlayMode
    {
        Training,
        SingleVS,
    }

    public partial class ClientBattleWorld : IBattleWorldListener, IAssetProvider
    {

        private bool isPuase = false;
        private int m_renderFPS = 60;
        private int m_logicFPS = 60;

        private float m_gameTimeResidual = 0;
        private float m_gameDeltaTime; //core update period

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
        /// p1角色配置
        /// </summary>
        private ConfigDataCharacter m_p1Config;
        /// <summary>
        /// p2角色配置
        /// </summary>
        private ConfigDataCharacter m_p2Config;
        /// <summary>
        /// 资源加载接口
        /// </summary>
        private IAssetProvider m_assetProvider;
        /// <summary>
        /// 玩家输入映射配置，key是玩家id,value是键名到键码的映射表
        /// </summary>
        private readonly List<Dictionary<KeyNames, KeyCode>> m_playerInputMapConfigList = new List<Dictionary<Core.KeyNames, KeyCode>>();

        private GameObject m_playersRoot;

        protected BattleWorld m_battleWorld;

        private BattleSceneViewController m_battleSceneViewController;

       

        private readonly Dictionary<int, CharacterActor> m_characterActorDic = new Dictionary<int, CharacterActor>();

       
        private int[] m_playerInputCodes;


        public T GetAsset<T>(string path) where T : UnityEngine.Object
        {
            return m_assetProvider.GetAsset<T>(path);
        }

        private byte[] LuaLoader(ref string fileName)
        {
            var code = m_assetProvider.GetAsset<TextAsset>(fileName);
            if (code != null)
            {
                return code.bytes;
            }
            return null;
        }

        private string FileRead(ref string fileName)
        {
            return GetAsset<TextAsset>(fileName).text;
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

        public ClientBattleWorld(GameObject playersRoot, ConfigDataStage stageConfig, ConfigDataCharacter p1Config, ConfigDataCharacter p2Config,IAssetProvider assetProvider,int renderFPS = 60,int logicFPS=60) {
            //初始化GameObject节点
            m_playersRoot = playersRoot;
            //初始化配置信息
            var configLoader = ConfigDataLoader.Instance;
            m_inputConfigList = new List<ConfigDataInputDefault>(configLoader.GetAllConfigDataInputDefault().Values);
            m_inputConfigList.Sort((a, b) => { return a.ID - b.ID; });//从小到大，从id代表第几个玩家
            m_commandsConfig = new List<ConfigDataCommand>(ConfigDataLoader.Instance.GetAllConfigDataCommand().Values);
            m_stageConfig = stageConfig;
            m_cameraConfig = ConfigDataLoader.Instance.GetConfigDataCamera(m_stageConfig.CameraConfigID);
            m_p1Config = p1Config;
            m_p2Config = p2Config;
            m_assetProvider = assetProvider;//资源加载器
            m_renderFPS = renderFPS;
            m_logicFPS = logicFPS;
            Application.targetFrameRate = m_renderFPS;
            m_gameDeltaTime = (1000 / m_logicFPS) / 1000f;
            InitPlayerInputMapConfig(m_inputConfigList);//初始化输入配置
            // new BattleWorld
            BattleWorld.StaticInit(LuaLoader, Debug.Log, Debug.LogWarning, Debug.LogError);
            m_battleWorld = new BattleWorld(m_commandsConfig, m_stageConfig, m_cameraConfig, p1Config, p2Config, this);
        }


        public void Init(BattleSceneViewController battleSceneViewController)
        {
            m_battleSceneViewController = battleSceneViewController;
            m_battleSceneViewController.CreateStage(m_stageConfig, this);

            m_playersRoot = m_battleSceneViewController.PlayerRoot;
        }


        private void UpdatePlayerInputCodes()
        {
            for(int i = 0;i<m_playerInputMapConfigList.Count;i++)
            {
                var inputMapDic = m_playerInputMapConfigList[i];
                int keycode = 0;
                foreach (var inputPair in inputMapDic)
                {
                    if (Input.GetKey(inputPair.Value))
                    {
                        keycode = keycode | Utility.GetKeycode(inputPair.Key);
                    }
                }
                m_playerInputCodes[i] = keycode;
            }
        }

        public virtual void Tick()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                this.isPuase = !this.isPuase;
            }
            if (isPuase)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    OnTick();
                }
            }
            else
            {
                OnTick();
            }

        }

        protected virtual void OnTick() {
            UpdatePlayerInputCodes();
            m_gameTimeResidual += UnityEngine.Time.deltaTime;
            while (m_gameTimeResidual > m_gameDeltaTime)
            {
                m_gameTimeResidual -= m_gameDeltaTime;
                Step();
            }
            ActorsTick();
        }

        private void ActorsTick()
        {
            foreach(var pair in m_characterActorDic)
            {
                pair.Value.Tick();
            }
        }

        protected void Step()
        { 
            m_battleWorld.UpdatePlayerInput(m_playerInputCodes);
            m_battleWorld.Step();
        }

        
    }
}
