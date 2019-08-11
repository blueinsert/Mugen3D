using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.UGFramework.UI;
using bluebean.UGFramework.ConfigData;
using bluebean.Mugen3D.Core;

namespace bluebean.Mugen3D.UI
{
    public class BattleUITask : UITask, IBattleWorldListener
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
        protected override void OnCreateAllUIViewController()
        {
            base.OnCreateAllUIViewController();
            if (m_uiViewControllerArray.Length >= 0)
            {
               
            }
        }

        protected override void UpdateView()
        {
        }

        #endregion

        #region UI回调
        #endregion

        private ConfigDataStage m_stageConfig;
        private ConfigDataCamera m_cameraConfig;
        private ConfigDataCharacter m_p1Config;
        private ConfigDataCharacter m_p2Config;

        #region 资源描述

        protected override UILayerDesc[] UILayerDescArray { get => m_uiLayerDescs; }

        private UILayerDesc[] m_uiLayerDescs = new UILayerDesc[] {
            new UILayerDesc(){
                LayerName = "BattleUI",
                AssetPath = "Assets/GameProject/RuntimeAssets/UI/Battle_ABS/Prefabs/BattleUIPrefab.prefab",
            }
        };

        protected override UIViewControllerDesc[] UIViewControllerDescArray { get => m_uiViewControllerDescs; }

        private UIViewControllerDesc[] m_uiViewControllerDescs = new UIViewControllerDesc[]{
            new UIViewControllerDesc()
            {
                AtachLayerName = "BattleUI",
                AtachPath = "./",
                TypeFullName = "bluebean.Mugen3D.UI.BattleUIController",
            }
        };
        #endregion
    }
}
