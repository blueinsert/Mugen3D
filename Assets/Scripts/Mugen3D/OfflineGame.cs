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
            InitCore();
            CreateGame(matchInfo, logicFPS);
            this.game.StartGame();

            p1 = (this.game.matchManager).p1;
            p2 = (this.game.matchManager).p2;

            
        }

        protected override void OnUpdate()
        {
            this.game.UpdateInput(InputHandler.Instance.GetInputKeycode(0), InputHandler.Instance.GetInputKeycode(1));
            m_gameTimeResidual += UnityEngine.Time.deltaTime;
            while (m_gameTimeResidual > m_gameDeltaTime)
            {
                m_gameTimeResidual -= m_gameDeltaTime;
                Step();
            }

        }
    }
}