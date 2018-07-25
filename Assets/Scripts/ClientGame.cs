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
            world = new World(new StageConfig() { borderXMax = 15, borderXMin = -15, borderYMin = 0, borderYMax = 100 });
            viewWorld = new ViewWorld();
            viewWorld.CreateBattleScene(this.gameObject);
            world.onCreateEntity += viewWorld.OnCreateEntity;
        }

        public void OnDestroy()
        {
            Instance = null;
        }

        protected Character AddCharacter(string characterName, int slot)
        {
            string prefix = "Chars/" + characterName;
            CharacterConfig config = ConfigReader.Read<CharacterConfig>(ResourceLoader.LoadText(prefix + "/" + characterName + ".def"));
            Character p = new Character(characterName, config);
            p.SetSlot(slot);
            this.world.AddEntity(p);
            return p;
        }

        public void CreateGame(string p1CharacterName, string p2CharacterName, string stageName, PlayMode playMode = PlayMode.Training)
        {
            UnityEngine.Object prefabStage = Resources.Load<UnityEngine.Object>("Stage/" + stageName + "/" + stageName);
            GameObject goStage = GameObject.Instantiate(prefabStage, this.transform.Find("Stage")) as GameObject;
            p1 = AddCharacter(p1CharacterName, 0);
            p2 = AddCharacter(p2CharacterName, 1);
            var cameraController = new CameraController(new CameraConfig() { depth = -6, fieldOfView  = 34, yOffset = 1, aspect = new Number(4)/new Number(3)}, p1, p2);
            world.AddEntity(cameraController);
            world.camCtl = cameraController;
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
