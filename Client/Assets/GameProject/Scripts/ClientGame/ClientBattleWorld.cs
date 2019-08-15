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

        public ClientBattleWorld(List<ConfigDataCommand> configDataCommand, ConfigDataStage stageConfig, ConfigDataCamera cameraConfig, ConfigDataCharacter p1Config, ConfigDataCharacter p2Config,IAssetProvider assetProvider) {
            m_battleWorld = new BattleWorld(configDataCommand, stageConfig, cameraConfig, p1Config, p2Config, this);
            m_assetProvider = assetProvider;
            m_gameDeltaTime = (1000 / 60) / 1000f;
            m_stageConfig = stageConfig;
        }

        public void Init(BattleSceneViewController battleSceneViewController)
        {
            m_battleSceneViewController = battleSceneViewController;
            m_battleSceneViewController.CreateStage(m_stageConfig, this);

            m_rootPlayers = m_battleSceneViewController.PlayerRoot;
            m_battleWorld.CreateCharacters();
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
            m_battleWorld.Step();
        }

        
    }
}
