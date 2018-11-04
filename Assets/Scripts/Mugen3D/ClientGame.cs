using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Mugen3D.Core;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;
using System;

namespace Mugen3D
{
    public enum PlayMode
    {
        Training,
        SingleVS,
    }

    public abstract class ClientGame : MonoBehaviour
    {
        public static ClientGame Instance;
        public Core.World world;
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
            /*
            string path = Path.Combine(Path.Combine(Application.streamingAssetsPath, "LuaRoot"), fileName);
            var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(file, System.Text.Encoding.UTF8);
            var bytes = System.Text.Encoding.UTF8.GetBytes(reader.ReadToEnd());
            return bytes;
            */
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

        private void InitCore()
        {
            Core.Debug.Log = Log.Info;
            Core.Debug.LogWarn = Log.Warn;
            Core.Debug.LogError = Log.Error;
            Core.Debug.Assert = Log.Assert;
            Core.LuaMgr.AddLoader(LuaLoader);
            Core.FileReader.AddReader(FileRead);
            if (GUIDebug.Instance != null)
            {
                Core.Debug.AddGUIDebugMsg = GUIDebug.Instance.AddMsg;
            }
        }

        protected void InitGame()
        {
            InitCore();
        }

        protected Character CreateCharacter(string characterName, int slot, bool isLocal)
        {
            CharacterConfig config = ConfigHelper.ReadCharacterConfig(characterName);
            Character p = new Character(characterName, config, slot, isLocal);
            this.world.AddEntity(p);
            return p;
        }

        protected void CreateWorld(string stageName, int logicFPS)
        {
            StageConfig stageConfig = ConfigReader.Read<StageConfig>(ResourceLoader.LoadText("Config/Stage/" + stageName));
            InputConfig inputConfig = ConfigReader.Read<InputConfig>(ResourceLoader.LoadText("Config/Input"));
            WorldConfig worldConfig = new WorldConfig();
            worldConfig.SetStageConfig(stageConfig);
            worldConfig.SetInputConfig(inputConfig);
            world = new World(worldConfig, logicFPS);
            viewWorld = new ViewWorld(world);
            viewWorld.InitScene(this.gameObject);
            viewWorld.CreateStage(worldConfig.stageConfig.stage);
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
            this.world.Update();
        }

    }
}
