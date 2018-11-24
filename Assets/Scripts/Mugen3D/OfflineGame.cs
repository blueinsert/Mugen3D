using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;

namespace Mugen3D
{
    [System.Serializable]
    public class CharacterInfo
    {
        public string name;
    }

    public class OfflineGame : ClientGame
    {
        public List<CharacterInfo> chars = new List<CharacterInfo>(2);
        public string stage;  
        public int renderFPS = 60;
        public int logicFPS = 60;

        private float m_gameTimeResidual = 0;
        private float m_gameDeltaTime; //core update period

        private Character p1;
        private Character p2;

        public void Start()
        {      
            StartGame(chars[0].name, chars[1].name, stage, renderFPS, logicFPS);
        }

        public void StartGame(string p1CharacterName, string p2CharacterName, string stageName, int renderFPS, int logicFPS)
        {
            Application.targetFrameRate = renderFPS;
            m_gameDeltaTime = (1000 / logicFPS) / 1000f;
            InitGame();
            CreateWorld(stageName, logicFPS);
            p1 = CreateCharacter(p1CharacterName, 0, true);
            p2 = CreateCharacter(p2CharacterName, 1, false);
            viewWorld.CreateCamera(this.world.cameraController);
        }

        protected override void OnUpdate()
        {
            p1.UpdateInput(InputHandler.Instance.GetInputKeycode(p1.slot, p1.GetFacing()));
            p2.UpdateInput(InputHandler.Instance.GetInputKeycode(p2.slot, p2.GetFacing()));
            m_gameTimeResidual += UnityEngine.Time.deltaTime;
            while (m_gameTimeResidual > m_gameDeltaTime)
            {
                m_gameTimeResidual -= m_gameDeltaTime;
                Step();
            }

        }
    }
}