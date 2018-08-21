using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Net;

namespace Mugen3D
{

    public class MultiPlayerGame : ClientGame
    {
        BattleNetClient m_battleNetClient;

        public void StartGame(string p1CharacterName, string p2CharacterName, string stageName, BattleNetClient battleNetClient, int renderFPS = 60, int logicFPS = 60)
        {
            RegisterBattleNetClient(battleNetClient);
            Application.targetFrameRate = renderFPS;
            InitGame();
            CreateWorld(stageName, logicFPS);
            CreateCharacter(p1CharacterName, 0, true);
            CreateCharacter(p2CharacterName, 1, false);
            world.CreateCamera();
        }

        void RegisterBattleNetClient(BattleNetClient battleNetClient)
        {
            m_battleNetClient = battleNetClient;
            m_battleNetClient.onGameStart += OnGameStart;
            m_battleNetClient.onGameUpdate += OnGameUpdate;
            m_battleNetClient.onGameEnd += OnGameEnd;
        }


        void UnRegisterBattleNetClient(BattleNetClient battleNetClient)
        {
            m_battleNetClient.onGameStart -= OnGameStart;
            m_battleNetClient.onGameUpdate -= OnGameUpdate;
            m_battleNetClient.onGameEnd -= OnGameEnd;
            m_battleNetClient = null;
        }

        void OnGameStart()
        {

        }

        void OnGameUpdate(int frame, int[] commands)
        {
            //Debug.Log("OnGameUpdate");
            if (commands != null)
            {
                var players = world.characters;
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i] != null)
                    {
                        players[i].UpdateInput(commands[i]);
                    }
                }
            }
            Step();
        }

        void OnGameEnd() { 

        }

        protected override void OnUpdate()
        {
            m_battleNetClient.SendInput(Core.Time.frameCount, InputHandler.Instance.GetInputKeycode(world.localPlayer.slot, world.localPlayer.facing));
        }
    }
}
