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

    public class ClientGame : MonoBehaviour
    {
        public static ClientGame Instance;
        public Core.World world;
        public ViewWorld viewWorld;

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

        private string LuaPathHook(string fileName)
        {
            return Path.Combine(Path.Combine(Application.streamingAssetsPath, "LuaRoot"), fileName);
        }

        private void InitCore()
        {
            Core.Debug.Log = Log.Info;
            Core.Debug.LogWarn = Log.Warn;
            Core.Debug.LogError = Log.Error;
            Core.Debug.Assert = Log.Assert;
            Core.LuaMgr.SetPathHook(this.LuaPathHook);
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
            viewWorld = new ViewWorld();
            viewWorld.InitScene(this.gameObject);
            world.onCreateEntity += viewWorld.OnCreateEntity;
            world.onCreateWorld += viewWorld.OnCreateWorld;
            world.CreateWorld();
        }

        protected virtual void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        protected void Step()
        {
            this.world.Update();
        }

    }
}
