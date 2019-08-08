using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;
using bluebean.UGFramework.UI;

namespace bluebean.Mugen3D.UI
{
    public class MainMenuUITask : UITask
    {
        public MainMenuUITask(string name) : base(typeof(MainMenuUITask).Name)
        {
        }

        #region UITask生命周期

        protected override void OnCreateAllUIViewController()
        {
            base.OnCreateAllUIViewController();
            if (m_uiViewControllerArray.Length >= 0)
            {
                m_uiController = m_uiViewControllerArray[0] as MainMenuUIController;
                //注册事件
                m_uiController.EventOnSingleVSButtonClick += OnSingleVSButtonClick;
                m_uiController.EventOnTrainButtonClick += OnTrainButtonClick;
                m_uiController.EventOnOptionsButtonClick += OnOptionButtonClick;
                m_uiController.EventOnExitButtonClick += OnExitButtonClick;
            }
        }

        protected override void UpdateView()
        {
            m_uiController.ShowOpenTween();
        }

        #endregion

        #region UI回调

        private void OnSingleVSButtonClick()
        {
            Debug.Log("MainMenuUITask:OnSingleVSButtonClick");
            Pause();
            CharacterSelectUITask.StartUITask(m_curUIIntent,CharacterSelectUITask.Mode_Training);
        }

        private void OnTrainButtonClick()
        {
            Debug.Log("MainMenuUITask:OnTrainButtonClick");
            Pause();
            CharacterSelectUITask.StartUITask(m_curUIIntent, CharacterSelectUITask.Mode_Training);
        }

        private void OnOptionButtonClick()
        {
            Debug.Log("MainMenuUITask:OnOptionButtonClick");
        }

        private void OnExitButtonClick()
        {
            Debug.Log("MainMenuUITask:OnExitButtonClick");
        }

        #endregion

        #region 资源描述

        protected override UILayerDesc[] UILayerDescArray { get => m_uiLayerDescs; }

        private UILayerDesc[] m_uiLayerDescs = new UILayerDesc[] {
            new UILayerDesc(){
                LayerName = "MainMenu",
                AssetPath = "Assets/GameProject/RuntimeAssets/UI/Menu_ABS/Prefabs/MainMenuUIPrefab.prefab",
            }
        };

        protected override UIViewControllerDesc[] UIViewControllerDescArray { get => m_uiViewControllerDescs; }

        private UIViewControllerDesc[] m_uiViewControllerDescs = new UIViewControllerDesc[]{
            new UIViewControllerDesc()
            {
                AtachLayerName = "MainMenu",
                AtachPath = "./",
                TypeFullName = "bluebean.Mugen3D.UI.MainMenuUIController",
            }
        };
        #endregion

        private MainMenuUIController m_uiController;



    }
}

