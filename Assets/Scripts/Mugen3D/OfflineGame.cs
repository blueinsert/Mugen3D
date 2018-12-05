using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class OfflineGame : ClientGame
    {
        public Core.MatchInfo matchInfo;

        public int renderFPS = 60;
        public int logicFPS = 60;

        private float m_gameTimeResidual = 0;
        private float m_gameDeltaTime; //core update period

        private Core.Character p1;
        private Core.Character p2;

        public void Start()
        {      
            StartGame(matchInfo, renderFPS, logicFPS);
        }

        public void StartGame(Core.MatchInfo matchInfo, int renderFPS, int logicFPS)
        {
            Application.targetFrameRate = renderFPS;
            m_gameDeltaTime = (1000 / logicFPS) / 1000f;
            InitGame();
            CreateGame(matchInfo.stage, logicFPS);
            this.game.StartGame(matchInfo);
            p1 = (this.game.matchManager as Core.SingleVS).p1;
            p2 = (this.game.matchManager as Core.SingleVS).p2;
            viewWorld.CreateCamera(this.game.world.cameraController);
            UIManager.Instance.AddView("FightHud", this.transform);
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