using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Net;
using bluebean.Mugen3D.Core;
using Debug = bluebean.UGFramework.Log.Debug;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.ClientGame
{

    public class MultiPlayerGame : ClientBattleWorld
    {
        BattleNetClient m_battleNetClient;

        public MultiPlayerGame(ConfigDataStage stageConfig, ConfigDataCamera cameraConfig, ConfigDataCharacter p1Config, ConfigDataCharacter p2Config) : base(stageConfig, cameraConfig, p1Config, p2Config)
        {
        }

        IEnumerator SendProgress()
        {
            while (!m_battleNetClient.isBattleReady)
            {
                m_battleNetClient.SendLoadProgress(100);
                yield return new WaitForSeconds(0.5f);
            }      
        }

        public void StartGame(string p1CharacterName, string p2CharacterName, string stageName, BattleNetClient battleNetClient, int renderFPS = 60, int logicFPS = 60)
        {
            RegisterBattleNetClient(battleNetClient);
            Application.targetFrameRate = renderFPS;
            InitCore();
            //CreateGame(stageName, logicFPS);
            //CreateCharacter(p1CharacterName, 0, true);
            //CreateCharacter(p2CharacterName, 1, false);
            StartCoroutine(SendProgress());
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
            Debug.Log("OnGameStart");
        }

        void OnGameUpdate(int frame, int[] commands)
        {
            //Debug.Log("OnGameUpdate");
            if (commands != null)
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    m_battleWorld.UpdatePlayerInput(i, commands[i]);
                }
            }
            Step();
        }

        void OnGameEnd() {
            Debug.Log("OnGameEnd");
        }

        protected override void OnUpdate()
        {
            if (!m_battleNetClient.isBattleReady)
                return;
            //m_battleNetClient.SendInput(Core.Time.frameCount, InputHandler.Instance.GetInputKeycode(game.world.localPlayer.slot, game.world.localPlayer.GetFacing()));
        }

        void OnGUI()
        {
            if (GUILayout.Button("EndGame"))
            {
                m_battleNetClient.EndGame();
            }

        }

    }
}
