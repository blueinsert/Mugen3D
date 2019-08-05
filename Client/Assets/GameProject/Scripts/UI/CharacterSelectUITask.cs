using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.UI
{
    public class CharacterSelectUITask : UITask
    {
        public CharacterSelectUITask(string name) : base(typeof(CharacterSelectUITask).Name)
        {
        }

        public static void StartUITask(UIIntent prevIntent)
        {
            UIIntent intent = new UIIntent(typeof(CharacterSelectUITask).Name, prevIntent);
            UIManager.Instance.StartUITask(intent);
        }

        #region UITask生命流程

        protected override void OnCreateAllUIViewController()
        {
            base.OnCreateAllUIViewController();
            if (m_uiViewControllerArray.Length >= 0)
            {
                m_uiController = m_uiViewControllerArray[0] as CharacterSelectUIController;
                //注册事件
                m_uiController.EventOnReturnButtonClick += OnReturnButtonClick;
                m_uiController.EventOnLittleHeadClick += OnLittleHeadButtonClick;
            }
        }

        protected override bool IsNeedUpdateCache()
        {
            if (m_configDataCharacters.Count == 0)
            {
                return true;
            }
            return false;
        }

        protected override bool IsNeedLoadAssets()
        {
            if (m_updateCtx.m_isInit)
                return true;
            return false;
        }

        protected override List<string> CollectAssetPathsToLoad()
        {
            List<string> assetList = new List<string>();
            foreach(var configCharacter in m_configDataCharacters)
            {
                assetList.Add(AssetUtility.GetSpritePath(configCharacter.LittleHeadIcon));
                assetList.Add(AssetUtility.GetSpritePath(configCharacter.MediumHeadIcon));
                assetList.Add(AssetUtility.GetSpritePath(configCharacter.BigHeadIcon));
            }
            return assetList;
        }

        protected override void UpdateCache()
        {
            m_configDataCharacters.Clear();
           foreach (var pair in m_configDataLoader.GetAllConfigDataCharacter())
            {
                m_configDataCharacters.Add(pair.Value);
            }
            m_configDataCharacters.Sort((a, b) => {
                return a.ID - b.ID;
            });
        }

        protected override void UpdateView()
        {
            if (m_updateCtx.m_isInit)
                m_uiController.SetCharacters(m_configDataCharacters, m_assetDic);
            m_uiController.UpdateUI(m_p1CharacteIndex, m_p2CharacterIndex);
            m_uiController.ShowOpenTween();
        }

        #endregion

        #region UI回调

        private void OnReturnButtonClick()
        {
            ReturnToPrevUITask();
        }

        private void OnLittleHeadButtonClick(int index)
        {
            m_p1CharacteIndex = index;
            m_uiController.UpdateUI(m_p1CharacteIndex, m_p2CharacterIndex);
        }

        #endregion

        #region 变量

        public int m_p1CharacteIndex = 0;

        public int m_p2CharacterIndex = 0;

        public ConfigDataLoader m_configDataLoader = GameManager.Instance.ConfigDataLoader;

        public readonly List<ConfigDataCharacter> m_configDataCharacters = new List<ConfigDataCharacter>();

        private CharacterSelectUIController m_uiController;

        #region 资源描述

        protected override UILayerDesc[] UILayerDescArray { get => m_uiLayerDescs; }

        private UILayerDesc[] m_uiLayerDescs = new UILayerDesc[] {
            new UILayerDesc(){
                LayerName = "CharacterSelect",
                AssetPath = "Assets/GameProject/RuntimeAssets/UI/Menu_ABS/Prefabs/CharacterSelectUIPrefab.prefab",
            }
        };

        protected override UIViewControllerDesc[] UIViewControllerDescArray { get => m_uiViewControllerDescs; }

        private UIViewControllerDesc[] m_uiViewControllerDescs = new UIViewControllerDesc[]{
            new UIViewControllerDesc()
            {
                AtachLayerName = "CharacterSelect",
                AtachPath = "./",
                TypeFullName = "bluebean.Mugen3D.UI.CharacterSelectUIController",
            }
        };
        #endregion

        #endregion
    }
}
