using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;
using bluebean.UGFramework.ConfigData;
using bluebean.Mugen3D.Core;
using bluebean.Mugen3D.ClientGame;

namespace bluebean.Mugen3D.UI
{
    public class BattleUITask : UITask
    {
        public BattleUITask(string name) : base(typeof(BattleUITask).Name)
        {
        }

        public static void StartUITask(UIIntent prevIntent,ConfigDataCharacter p1Config, ConfigDataCharacter p2Config, ConfigDataStage stageConfig)
        {
            UIIntent intent = new UIIntent(typeof(BattleUITask).Name, prevIntent);
            intent.SetCustomParam(ParamKey_P1Config, p1Config);
            intent.SetCustomParam(ParamKey_P2Config, p2Config);
            intent.SetCustomParam(ParamKey_StageConfig, stageConfig);
            UIManager.Instance.StartUITask(intent);
        }

        #region UITask生命周期

        private void InitDataFromIntent(UIIntent curIntent)
        {
            m_stageConfig = curIntent.GetCustomClassParam<ConfigDataStage>(ParamKey_StageConfig);
            m_p1Config = curIntent.GetCustomClassParam<ConfigDataCharacter>(ParamKey_P1Config);
            m_p2Config = curIntent.GetCustomClassParam<ConfigDataCharacter>(ParamKey_P2Config);
        }

        protected override void OnIntentChange(UIIntent prevIntent, UIIntent curIntent)
        {
            InitDataFromIntent(curIntent);
        }

       
        protected override bool OnStart(UIIntent intent)
        {
            bool res = base.OnStart(intent);
            return res;
        }

        protected override bool OnResume(UIIntent intent)
        {
            bool res = base.OnResume(intent);
            return res;
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override bool IsNeedUpdateCache()
        {
            return false;
        }

        protected override bool IsNeedLoadAssets()
        {
            return true;
        }

        protected override List<string> CollectAssetPathsToLoad()
        {
            List<string> resPath = new List<string>();
            resPath.Add(AssetUtility.MakeAssetPath(m_stageConfig.Prefab));
            resPath.Add(AssetUtility.MakeAssetPath(m_p1Config.Prefab));
            resPath.Add(AssetUtility.MakeAssetPath(m_p2Config.Prefab));
            foreach(var luaScript in m_p1Config.LuaScripts)
            {
                resPath.Add(AssetUtility.MakeAssetPath(luaScript));
            }
            foreach (var luaScript in m_p2Config.LuaScripts)
            {
                resPath.Add(AssetUtility.MakeAssetPath(luaScript));
            }
            resPath.Add(AssetUtility.MakeAssetPath("Lua_ABS/FsmScriptLoader.lua.txt"));
            resPath.Add(AssetUtility.MakeAssetPath("Lua_ABS/Common.lua.txt"));
            resPath.Add(AssetUtility.MakeAssetPath("Lua_ABS/Enums.lua.txt"));
            return resPath;
        }

        protected override void OnCreateAllUIViewController()
        {
            base.OnCreateAllUIViewController();
            if (m_viewControllerArray.Length > 0)
            {
                m_battleUIController = m_viewControllerArray[0] as BattleUIController;
            }
            if (m_viewControllerArray.Length > 1)
            {
                m_battleSceneViewController = m_viewControllerArray[1] as BattleSceneViewController;
            }
        }

        private void PushAllLayer()
        {
            foreach (var layer in m_layerDic.Values)
            {
                if(layer.State!= SceneLayerState.Using)
                {
                    SceneTree.Instance.PushLayer(layer);
                }
            }
        }

        protected override void UpdateView()
        {
            PushAllLayer();
            if(m_clientBattleWorld == null)
            {
                m_clientBattleWorld = new ClientBattleWorld(m_battleSceneViewController.gameObject, this);
                m_clientBattleWorld.StartSingleVSMatch(m_p1Config, m_p2Config, m_stageConfig);
            }
        }

        protected override void OnTick()
        {
            if (m_clientBattleWorld != null)
            {
                m_clientBattleWorld.Tick();
            }
        }

        #endregion

        #region UI回调
        #endregion

        #region 变量

        private ClientBattleWorld m_clientBattleWorld;

        private ConfigDataStage m_stageConfig;
        private ConfigDataCharacter m_p1Config;
        private ConfigDataCharacter m_p2Config;

        private BattleUIController m_battleUIController;
        private BattleSceneViewController m_battleSceneViewController;

        #region 资源描述

        protected override LayerDesc[] LayerDescArray {
            get {
                return m_layerDescs;
            }
        }

        private LayerDesc[] m_layerDescs = new LayerDesc[] {
            new LayerDesc(){
                LayerName = "BattleUI",
                AssetPath = "Assets/GameProject/RuntimeAssets/UI/Battle_ABS/Prefabs/BattleUIPrefab.prefab",
            },
            new LayerDesc(){
                LayerName = "BattleScene",
                AssetPath = "Assets/GameProject/RuntimeAssets/Other_ABS/BattleScene.prefab",
                IsUILayer = false,
            }
        };

        protected override ViewControllerDesc[] ViewControllerDescArray {
            get { return m_viewControllerDescs; }
        }

        private ViewControllerDesc[] m_viewControllerDescs = new ViewControllerDesc[]{
            new ViewControllerDesc()
            {
                AtachLayerName = "BattleUI",
                AtachPath = "./",
                TypeFullName = "bluebean.Mugen3D.UI.BattleUIController",
            },
            new ViewControllerDesc(){
                AtachLayerName = "BattleScene",
                AtachPath = "./",
                TypeFullName = "bluebean.Mugen3D.ClientGame.BattleSceneViewController",
            }
        };
        #endregion

        public const string ParamKey_P1Config = "P1Config";
        public const string ParamKey_P2Config = "P2Config";
        public const string ParamKey_StageConfig = "StageConfig";
        #endregion
    }
}
