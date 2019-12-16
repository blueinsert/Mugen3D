using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.UI
{
    public class CharacterGridPos
    {
        public int Col { get; set; }
        public int Row { get; set; }

        public CharacterGridPos(int index, int rowSize)
        {
            Row = index / rowSize;
            Col = index % rowSize;
        }

        public int GetIndex(int rowSize)
        {
            return this.Row * rowSize + this.Col;
        }
    }
    public class CharacterSelectUITask : UITask
    {
        /// <summary>
        /// 选人阶段
        /// </summary>
        public enum TrainingModeCharacterSelectStage
        {
            P1,
            P2,
            AllComplete,
        }

        public CharacterSelectUITask(string name) : base(typeof(CharacterSelectUITask).Name)
        {
            RegisterModeStr(Mode_Training);
        }

        public static void StartUITask(UIIntent prevIntent, string mode)
        {
            UIIntent intent = new UIIntent(typeof(CharacterSelectUITask).Name, prevIntent, mode);
            UIManager.Instance.StartUITask(intent);
        }

        #region UITask生命流程

        protected override void OnCreateAllUIViewController()
        {
            base.OnCreateAllUIViewController();
            if (m_viewControllerArray.Length >= 0)
            {
                m_uiController = m_viewControllerArray[0] as CharacterSelectUIController;
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
            foreach (var configCharacter in m_configDataCharacters)
            {
                assetList.Add(AssetUtility.MakeSpritePath(configCharacter.LittleHeadIcon));
                assetList.Add(AssetUtility.MakeSpritePath(configCharacter.MediumHeadIcon));
                assetList.Add(AssetUtility.MakeSpritePath(configCharacter.BigHeadIcon));
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
            m_configDataCharacters.Sort((a, b) =>
            {
                return a.ID - b.ID;
            });
        }

        protected override void UpdateView()
        {
            if (m_updateCtx.m_isInit)
            {
                m_uiController.SetCharacters(m_configDataCharacters, m_assetDic);
                m_uiController.ShowOpenTween();
            }
            m_uiController.UpdateUI(m_p1CharacterIndex, m_p2CharacterIndex);
        }

        protected override void OnTick()
        {
            base.OnTick();
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnUpKeyDown();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnDownKeyDown();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnLeftKeyDown();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnRightKeyDown();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                OnEnsureKeyDown();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                OnCancelKeyDown();
            }
        }

        #endregion

        #region UI回调

        private void OnUpKeyDown()
        {
            if (CurMode == Mode_Training)
            {
                if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P1)
                {
                    m_p1CharacterIndex = GetUpGridIndex(m_p1CharacterIndex);
                }
                else if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P2)
                {
                    m_p2CharacterIndex = GetUpGridIndex(m_p2CharacterIndex);
                }
            }
            m_uiController.UpdateUI(m_p1CharacterIndex, m_p2CharacterIndex);
        }

        private void OnDownKeyDown()
        {
            if (CurMode == Mode_Training)
            {
                if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P1)
                {
                    m_p1CharacterIndex = GetDownGridIndex(m_p1CharacterIndex);
                }
                else if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P2)
                {
                    m_p2CharacterIndex = GetDownGridIndex(m_p2CharacterIndex);
                }
            }
            m_uiController.UpdateUI(m_p1CharacterIndex, m_p2CharacterIndex);
        }

        private void OnLeftKeyDown()
        {
            if (CurMode == Mode_Training)
            {
                if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P1)
                {
                    m_p1CharacterIndex = GetLeftGridIndex(m_p1CharacterIndex);
                }
                else if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P2)
                {
                    m_p2CharacterIndex = GetLeftGridIndex(m_p2CharacterIndex);
                }
            }
            m_uiController.UpdateUI(m_p1CharacterIndex, m_p2CharacterIndex);
        }

        private void OnRightKeyDown()
        {
            if (CurMode == Mode_Training)
            {
                if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P1)
                {
                    m_p1CharacterIndex = GetRightGridIndex(m_p1CharacterIndex);
                }
                else if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P2)
                {
                    m_p2CharacterIndex = GetRightGridIndex(m_p2CharacterIndex);
                }
            }
            m_uiController.UpdateUI(m_p1CharacterIndex, m_p2CharacterIndex);
        }

        private void OnEnsureKeyDown()
        {
            if (CurMode == Mode_Training)
            {
                if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P1)
                {
                    m_trainingModeCharacterSelectStage = TrainingModeCharacterSelectStage.P2;
                }
                else if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P2)
                {
                    m_trainingModeCharacterSelectStage = TrainingModeCharacterSelectStage.AllComplete;
                }
                if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.AllComplete)
                {
                    Debug.Log("CharacterSelectUITask TraningMode:TrainingModeCharacterSelectStage == AllComplete");
                    Pause();
                    BattleUITask.StartUITask(m_curUIIntent,m_configDataCharacters[m_p1CharacterIndex], m_configDataCharacters[m_p2CharacterIndex], ConfigDataLoader.Instance.GetConfigDataStage(1));
                }
            }
        }

        private void OnCancelKeyDown()
        {
            if (CurMode == Mode_Training)
            {
                if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P1)
                {

                }
                else if (m_trainingModeCharacterSelectStage == TrainingModeCharacterSelectStage.P2)
                {

                }
            }
        }

        private void OnReturnButtonClick()
        {
            ReturnToPrevUITask();
        }

        private void OnLittleHeadButtonClick(int index)
        {
            m_p1CharacterIndex = index;
            m_uiController.UpdateUI(m_p1CharacterIndex, m_p2CharacterIndex);
        }

        #endregion

        #region 私有方法

        private int GetLeftGridIndex(int prevIndex)
        {
            var gridPos = new CharacterGridPos(prevIndex, GridSizeX);
            gridPos.Col--;
            gridPos.Row = Mathf.Clamp(gridPos.Row, 0, GridSizeY - 1);
            gridPos.Col = Mathf.Clamp(gridPos.Col, 0, GridSizeX - 1);
            int newIndex = gridPos.GetIndex(GridSizeX);
            if (newIndex >= 0 && newIndex < m_configDataCharacters.Count)
            {
                return newIndex;
            }
            return prevIndex;
        }

        private int GetRightGridIndex(int prevIndex)
        {
            var gridPos = new CharacterGridPos(prevIndex, GridSizeX);
            gridPos.Col++;
            gridPos.Row = Mathf.Clamp(gridPos.Row, 0, GridSizeY - 1);
            gridPos.Col = Mathf.Clamp(gridPos.Col, 0, GridSizeX - 1);
            int newIndex = gridPos.GetIndex(GridSizeX);
            if (newIndex >= 0 && newIndex < m_configDataCharacters.Count)
            {
                return newIndex;
            }
            return prevIndex;
        }

        private int GetUpGridIndex(int prevIndex)
        {
            var gridPos = new CharacterGridPos(prevIndex, GridSizeX);
            gridPos.Row--;
            gridPos.Row = Mathf.Clamp(gridPos.Row, 0, GridSizeY - 1);
            gridPos.Col = Mathf.Clamp(gridPos.Col, 0, GridSizeX - 1);
            int newIndex = gridPos.GetIndex(GridSizeX);
            if (newIndex >= 0 && newIndex < m_configDataCharacters.Count)
            {
                return newIndex;
            }
            return prevIndex;
        }

        private int GetDownGridIndex(int prevIndex)
        {
            var gridPos = new CharacterGridPos(prevIndex, GridSizeX);
            gridPos.Row++;
            gridPos.Row = Mathf.Clamp(gridPos.Row, 0, GridSizeY - 1);
            gridPos.Col = Mathf.Clamp(gridPos.Col, 0, GridSizeX - 1);
            int newIndex = gridPos.GetIndex(GridSizeX);
            if (newIndex >= 0 && newIndex < m_configDataCharacters.Count)
            {
                return newIndex;
            }
            return prevIndex;
        }

        #endregion

        #region 变量

        private TrainingModeCharacterSelectStage m_trainingModeCharacterSelectStage = TrainingModeCharacterSelectStage.P1;

        private bool m_isP1SelectComplete = false;

        private bool m_isP2SelectComplete = false;

        public int m_p1CharacterIndex = 0;

        public int m_p2CharacterIndex = 0;

        public ConfigDataLoader m_configDataLoader = GameManager.Instance.ConfigDataLoader;

        public readonly List<ConfigDataCharacter> m_configDataCharacters = new List<ConfigDataCharacter>();

        private CharacterSelectUIController m_uiController;

        #region 资源描述

        protected override LayerDesc[] LayerDescArray {
            get { return m_uiLayerDescs; }
        }

        private LayerDesc[] m_uiLayerDescs = new LayerDesc[] {
            new LayerDesc(){
                LayerName = "CharacterSelect",
                AssetPath = "Assets/GameProject/RuntimeAssets/UI/Menu_ABS/Prefabs/CharacterSelectUIPrefab.prefab",
            }
        };

        protected override ViewControllerDesc[] ViewControllerDescArray {
            get { return m_uiViewControllerDescs; }
        }

        private ViewControllerDesc[] m_uiViewControllerDescs = new ViewControllerDesc[]{
            new ViewControllerDesc()
            {
                AtachLayerName = "CharacterSelect",
                AtachPath = "./",
                TypeFullName = "bluebean.Mugen3D.UI.CharacterSelectUIController",
            }
        };
        #endregion

        private const int GridSizeX = 6;

        private const int GridSizeY = 8;

        public const string Mode_Training = "Mode_Training";
        #endregion
    }
}
