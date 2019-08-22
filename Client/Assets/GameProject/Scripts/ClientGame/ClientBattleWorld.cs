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
        public int renderFPS = 60;
        public int logicFPS = 60;

        private float m_gameTimeResidual = 0;
        private float m_gameDeltaTime; //core update period

        private GameObject m_rootPlayers;

        private IAssetProvider m_assetProvider;

        protected BattleWorld m_battleWorld;

        private BattleSceneViewController m_battleSceneViewController;

        private ConfigDataStage m_stageConfig;

        private readonly Dictionary<int, CharacterActor> m_characterActorDic = new Dictionary<int, CharacterActor>();

        /// <summary>
        /// 玩家输入映射配置，key是玩家id,value是键名到键码的映射表
        /// </summary>
        private readonly Dictionary<int, Dictionary<KeyNames, KeyCode>> m_playerInputMapDic = new Dictionary<int, Dictionary<Core.KeyNames, KeyCode>>();
        private int[] m_playerInputCodes;

        public ClientBattleWorld(List<ConfigDataInputDefault> configDataInputDefaults, List<ConfigDataCommand> configDataCommand, ConfigDataStage stageConfig, ConfigDataCamera cameraConfig, ConfigDataCharacter p1Config, ConfigDataCharacter p2Config,IAssetProvider assetProvider) {
            m_battleWorld = new BattleWorld(configDataCommand, stageConfig, cameraConfig, p1Config, p2Config, this);
            m_assetProvider = assetProvider;
            m_gameDeltaTime = (1000 / 60) / 1000f;
            m_stageConfig = stageConfig;
        }

        /// <summary>
        /// 初始化玩家输入映射 todo 使用本地保存的设置进行初始化
        /// </summary>
        private void InitPlayerInputMapConfig(List<ConfigDataInputDefault> configs)
        {
           foreach(var config in configs)
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
                m_playerInputMapDic.Add(config.ID, mapping);
            }
            m_playerInputCodes = new int[m_playerInputMapDic.Count];
        }

        public void Init(BattleSceneViewController battleSceneViewController)
        {
            m_battleSceneViewController = battleSceneViewController;
            m_battleSceneViewController.CreateStage(m_stageConfig, this);

            m_rootPlayers = m_battleSceneViewController.PlayerRoot;
        }



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

        protected void InitCore()
        {
            Core.Debug.Log = Debug.Log;
            Core.Debug.LogWarn = Debug.LogWarning;
            Core.Debug.LogError = Debug.LogError;
            Core.Debug.Assert = Debug.Assert;
            Core.LuaMgr.AddLoader(LuaLoader);
            Core.FileReader.AddReader(FileRead);
            if (GUIDebug.Instance != null)
            {
                Core.Debug.AddGUIDebugMsg = GUIDebug.Instance.AddMsg;
            }
            //todo
            //Core.SystemConfig.Instance.Init(ResourceLoader.LoadText("Config/System.cfg"));
        }

        private void UpdatePlayerInputCodes()
        {
            foreach(var playerPair in m_playerInputMapDic)
            {
                var inputMapDic = playerPair.Value;
                int keycode = 0;
                foreach (var inputPair in inputMapDic)
                {
                    if (Input.GetKey(inputPair.Value))
                    {
                        keycode = keycode | Utility.GetKeycode(inputPair.Key);
                    }
                }
                m_playerInputCodes[playerPair.Key] = keycode;
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
            m_gameTimeResidual += UnityEngine.Time.deltaTime;
            while (m_gameTimeResidual > m_gameDeltaTime)
            {
                m_gameTimeResidual -= m_gameDeltaTime;
                Step();
            }
        }

        protected void Step()
        {
            UpdatePlayerInputCodes();
            m_battleWorld.UpdatePlayerInput(m_playerInputCodes);
            m_battleWorld.Step();
        }

        
    }
}
