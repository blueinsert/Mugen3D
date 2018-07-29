using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

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

        private Character p1;
        private Character p2;
        private bool isInited = false;

        public void Awake()
        {
            Instance = this;
            Application.targetFrameRate = 60;
            Init();    
        }

        public void Init()
        {         
            
        }

        public void OnDestroy()
        {
            Instance = null;
        }

        protected Character AddCharacter(string characterName, int slot)
        {
            string prefix = "Chars/" + characterName;
            CharacterConfig config = ConfigReader.Read<CharacterConfig>(ResourceLoader.LoadText(prefix + "/" + characterName + ".def"));
            ActionsConfig actionsConfig = ConfigReader.Read<ActionsConfig>(ResourceLoader.LoadText(prefix + config.actionConfigFile));
            string commands = ResourceLoader.LoadText(prefix + config.cmdConfigFile);
            config.SetActions(actionsConfig.actions.ToArray());
            config.SetCommand(commands);
            Character p = new Character(characterName, config);
            p.SetSlot(slot);
            this.world.AddEntity(p);
            return p;
        }

        protected void CreateWorld()
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
            world = new World(worldConfig);
            viewWorld = new ViewWorld();
            viewWorld.InitScene(this.gameObject);
            world.onCreateEntity += viewWorld.OnCreateEntity;
            world.onCreateWorld += viewWorld.OnCreateWorld;
            world.CreateWorld();
        }

        public void CreateGame(string p1CharacterName, string p2CharacterName, string stageName, PlayMode playMode = PlayMode.Training)
        {
            CreateWorld();
            p1 = AddCharacter(p1CharacterName, 0);
            p2 = AddCharacter(p2CharacterName, 1);
            world.CreateCamera();
            isInited = true;
        }

        void Update()
        {
            if (!isInited)
                return;
            p1.UpdateInput(InputHandler.Instance.GetInputKeycode(p1.slot, p1.facing));
            p2.UpdateInput(InputHandler.Instance.GetInputKeycode(p2.slot, p2.facing));
            this.world.Update(UnityEngine.Time.deltaTime.ToNumber());
        }
    }
}
