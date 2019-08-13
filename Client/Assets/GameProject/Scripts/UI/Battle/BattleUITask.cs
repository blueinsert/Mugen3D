using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;
using bluebean.UGFramework.ConfigData;
using bluebean.Mugen3D.Core;

namespace bluebean.Mugen3D.UI
{
    public class BattleUITask : UITask
    {
        public BattleUITask(string name) : base(typeof(BattleUITask).Name)
        {
        }

        public static void StartUITask(UIIntent prevIntent, string mode)
        {
            UIIntent intent = new UIIntent(typeof(BattleUITask).Name, prevIntent, mode);
            UIManager.Instance.StartUITask(intent);
        }

        #region UITask生命周期

        private void InitDataFromIntent(UIIntent curIntent)
        {
            //test
            m_stageConfig = ConfigDataLoader.Instance.GetConfigDataStage(1);
            m_cameraConfig = ConfigDataLoader.Instance.GetConfigDataCamera(1);
            m_p1Config = ConfigDataLoader.Instance.GetConfigDataCharacter(1);
            m_p2Config = ConfigDataLoader.Instance.GetConfigDataCharacter(1);
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

        protected override void OnCreateAllUIViewController()
        {
            base.OnCreateAllUIViewController();
            if (m_viewControllerArray.Length >= 0)
            {
               
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
        }

        #endregion

        #region UI回调
        #endregion

        private ConfigDataStage m_stageConfig;
        private ConfigDataCamera m_cameraConfig;
        private ConfigDataCharacter m_p1Config;
        private ConfigDataCharacter m_p2Config;

        #region 资源描述

        protected override LayerDesc[] LayerDescArray { get => m_layerDescs; }

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

        protected override ViewControllerDesc[] ViewControllerDescArray { get => m_viewControllerDescs; }

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
    }
}
