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
        }

        protected void InitGame()
        {
            InitCore();
        }

        protected Character CreateCharacter(string characterName, int slot, bool isLocal)
        {
            string prefix = "Chars/" + characterName;
            CharacterConfig config = ConfigReader.Read<CharacterConfig>(ResourceLoader.LoadText(prefix + "/" + characterName + ".def"));
            ActionsConfig actionsConfig = ConfigReader.Read<ActionsConfig>(ResourceLoader.LoadText(prefix + config.actionConfigFile));
            string commands = ResourceLoader.LoadText(prefix + config.cmdConfigFile);
            config.SetActions(actionsConfig.actions.ToArray());
            config.SetCommand(commands);
            Character p = new Character(characterName, config, isLocal);
            p.SetSlot(slot);
            this.world.AddEntity(p);
            return p;
        }

        protected void CreateWorld(string stageName, int logicFPS)
        {
            CameraConfig cameraConfig = new CameraConfig() { depth = -6, fieldOfView = 34, yOffset = 1, aspect = new Number(4) / new Number(3) };
            var stageConfig = new StageConfig() { borderXMax = 15, borderXMin = -15, borderYMin = 0, borderYMax = 100, cameraConfig = cameraConfig, stage = "TrainingRoom", initPos = new Vector[] { new Vector(-10, 0), new Vector(10, 0) } };
            InputConfig inputConfig = new InputConfig();
            inputConfig.inputConfig = new PlayerInputConfig[] { 
                new PlayerInputConfig(){
                    slot = 0,
                    up = 119,
                    down = 115,
                    left = 97,
                    right = 100,
                    a = 106,
                    b = 107,
                    c = 108,
                    x = 117,
                    y = 105,
                    z = 122,
                },
                new PlayerInputConfig(){
                    slot = 1,
                    up = 273,
                    down = 274,
                    left = 276,
                    right = 275,
                    a = 256,
                    b = 257,
                    c = 258,
                    x = 259,
                    y = 260,
                    z = 261,
                },
            };
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
