﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using bluebean.Mugen3D.Core;
using bluebean.UGFramework.ConfigData;
using Debug = bluebean.UGFramework.Log.Debug;
using FixPointMath;

namespace bluebean.Mugen3D.ClientGame
{
    public enum PlayMode
    {
        Training,
        SingleVS,
    }

    public partial class ClientBattleWorld : MonoBehaviour, IBattleWorldListener
    {

        #region toRemoved
        public ViewWorld viewWorld;
        private bool isPuase = false;

        private void OnDestroy()
        {
          
        }

        private static byte[] LuaLoader(ref string fileName)
        {
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

        protected void InitCore()
        {
            Core.Debug.Log = Debug.Log;
            Core.Debug.LogWarn = Debug.LogWarning;
            Core.Debug.LogError = Debug.LogError;
            Core.Debug.Assert = Debug.Assert;
            Core.LuaMgr.AddLoader(LuaLoader);
            Core.FileReader.AddReader(FileRead);
            if (GUIDebug.Instance != null)
            {
                Core.Debug.AddGUIDebugMsg = GUIDebug.Instance.AddMsg;
            }
            Core.SystemConfig.Instance.Init(ResourceLoader.LoadText("Config/System.cfg"));
        }

        #endregion

        public int renderFPS = 60;
        public int logicFPS = 60;

        private float m_gameTimeResidual = 0;
        private float m_gameDeltaTime; //core update period

        protected BattleWorld m_battleWorld;

        public ClientBattleWorld(ConfigDataStage stageConfig, ConfigDataCamera cameraConfig, ConfigDataCharacter p1Config, ConfigDataCharacter p2Config) {
            m_battleWorld = new BattleWorld(stageConfig, cameraConfig, p1Config, p2Config, this);
            m_gameDeltaTime = (1000 / 60) / 1000f;
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

        protected virtual void OnUpdate() {
            m_gameTimeResidual += UnityEngine.Time.deltaTime;
            while (m_gameTimeResidual > m_gameDeltaTime)
            {
                m_gameTimeResidual -= m_gameDeltaTime;
                Step();
            }
        }

        protected void Step()
        {
            m_battleWorld.Step();
        }
 
    }
}