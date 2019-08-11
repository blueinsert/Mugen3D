using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using bluebean.Mugen3D.Core;
using Debug = bluebean.UGFramework.Log.Debug;
using FixPointMath;
using System;

namespace bluebean.Mugen3D.ClientGame
{
    public enum PlayMode
    {
        Training,
        SingleVS,
    }

    public abstract class ClientGame : MonoBehaviour
    {
        public static ClientGame Instance;
        public Core.Game game;
        public ViewWorld viewWorld;
        private bool isPuase = false;

        private void Awake()
        {
            Instance = this;
        }

        private void DestroyWorld()
        {
            if (this.viewWorld != null)
            {
                //this.viewWorld.Destroy();
                //ResourceMgr.UnloadUnusedAssets();
            }
        }

        private void OnDestroy()
        {
            Instance = null;
            if (this.viewWorld != null)
            {
                DestroyWorld();
            }
        }

        private static byte[] LuaLoader(ref string fileName)
        {
            var code = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
            if (code != null)
            {
                return code.bytes;
            }
            return null;
        }

        private static string FileRead(ref string fileName)
        {
            return ResourceLoader.LoadText(fileName);
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
            Core.SystemConfig.Instance.Init(ResourceLoader.LoadText("Config/System.cfg"));
        }

        private void LoadFightHud() {
            UIManager.Instance.AddView("FightHud", this.transform);
        }

        protected void CreateGame(MatchInfo matchInfo, int logicFPS)
        {
            StageConfig stageConfig = ConfigReader.Parse<StageConfig>(ResourceLoader.LoadText("Stage/" + matchInfo.stage + "/" + matchInfo.stage + ".def"));
            WorldConfig worldConfig = new WorldConfig();
            worldConfig.SetStageConfig(stageConfig);
            this.game = new Game(matchInfo, worldConfig, logicFPS);

            LoadFightHud();

            viewWorld = new ViewWorld(game.world);
            viewWorld.InitScene(this.gameObject);
            viewWorld.CreateStage(worldConfig.stageConfig.stage);

            viewWorld.CreateCamera(this.game.world.cameraController);
        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                this.isPuase = !this.isPuase;
            }
            if (isPuase)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    OnUpdate();
                }
            }
            else
            {
                OnUpdate();
            }
            
        }

        protected virtual void OnUpdate() { }

        protected void Step()
        {
            this.game.Step();
        }

    }
}
