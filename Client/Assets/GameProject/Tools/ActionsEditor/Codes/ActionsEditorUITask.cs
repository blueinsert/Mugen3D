﻿using bluebean.Mugen3D.Core;
using bluebean.UGFramework;
using bluebean.UGFramework.ConfigData;
using bluebean.UGFramework.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YamlDotNet.Serialization;
using WaitForSeconds = bluebean.UGFramework.WaitForSeconds;

namespace bluebean.Mugen3D.UI
{
    public enum ActionsEditorUITaskUpdateMask
    {
        Whole,
    }
    public class ActionsEditorUITask : UITask
    {

        public ActionsEditorUITask(string name) : base(typeof(ActionsEditorUITask).Name)
        {
        }

        public static void StartUITask(UIIntent prevIntent)
        {
            UIIntent intent = new UIIntent(typeof(ActionsEditorUITask).Name, prevIntent);
            UIManager.Instance.StartUITask(intent);
        }

        #region UITask生命周期

        private void InitDataFromIntent(UIIntent curIntent)
        {

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

        private void PushAllLayer()
        {
            foreach (var layer in m_layerDic.Values)
            {
                if (layer.State != SceneLayerState.Using)
                {
                    SceneTree.Instance.PushLayer(layer);
                }
            }
        }

        protected override bool OnResume(UIIntent intent)
        {
            bool res = base.OnResume(intent);
            PushAllLayer();
            return res;
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override bool IsNeedUpdateCache()
        {
            if (m_cacheCharacterConfigs == null)
            {
                var characters = GameManager.Instance.ConfigDataLoader.GetAllConfigDataCharacter();
                m_cacheCharacterConfigs = new List<ConfigDataCharacter>();
                foreach (var character in characters)
                {
                    m_cacheCharacterConfigs.Add(character.Value);
                }
            }
            if (m_updateCtx.m_isInit)
            {
                m_curCharacterConfig = m_cacheCharacterConfigs[0];
                m_updateCtx.ActiveUpdateMask((int)ActionsEditorUITaskUpdateMask.Whole);
            }
            return false;
        }

        protected override bool IsNeedLoadAssets()
        {
            return true;
        }

        protected override List<string> CollectAssetPathsToLoad()
        {
            List<string> resPath = new List<string>();
            if (m_curCharacterConfig != null)
            {
                resPath.Add("Assets/GameProject/RuntimeAssets/" + m_curCharacterConfig.Prefab);
                //反序列画动画配置信息
                var deserializer = new Deserializer();
                TextReader reader = File.OpenText(Application.dataPath + "/GameProject/RuntimeAssets/"+ m_curCharacterConfig.ActionDef);
                m_actionDefList = deserializer.Deserialize<List<Core.ActionDef>>(reader.ReadToEnd());
                UGFramework.Log.Debug.Log(string.Format("serialize {0}'s acitons def", m_curCharacterConfig.Name));
            }
            return resPath;
        }

        protected override void OnCreateAllUIViewController()
        {
            base.OnCreateAllUIViewController();

            m_actionEditUIController = m_viewControllerArray[0] as ActionsEditorUIController;
            m_actionEdit3DScenneController = m_viewControllerArray[1] as ActionEditor3DSceneController;

            m_canvas = m_actionEditUIController.GetComponentInParent<Canvas>();
            m_sceneCamera = m_actionEdit3DScenneController.GetComponentInParent<ThreeDSceneLayer>().LayerCamera;

            m_actionEditUIController.EventOnCloseButtonClick += OnCloseButtonClick;

            m_actionEditUIController.EventOnSaveButtonClick += OnSaveButtonClick;
            m_actionEditUIController.EventOnLoadDropDownValueChanged += OnLoadDropDownValueChanged;
            m_actionEditUIController.EventOnPlayButtonClick += OnPlayButtonClick;
            m_actionEditUIController.EventOnPauseButtonClick += OnPauseButtonClick;

            m_actionEditUIController.EventOnPointerDown += m_actionEdit3DScenneController.OnPointerDown;
            m_actionEditUIController.EventOnDrag += m_actionEdit3DScenneController.OnDrag;

            m_actionEditUIController.EventOnBtnGoLeftActionClick += OnGoLeftActionButtonClick;
            m_actionEditUIController.EventOnBtnGoRightActionClick += OnGoRightActionButtonClick;
            m_actionEditUIController.EventOnScrollActionListValueChange += OnScrollActionListValueChange;
            m_actionEditUIController.EventOnBtnCreateActionClick += OnBtnCreateActionClick;
            m_actionEditUIController.EventOnBtnDeleteActionClick += OnBtnDeleteActionClick;
            m_actionEditUIController.EventOnDropdownAnimNameChanged += OnDropdownAnimNameChanged;
            m_actionEditUIController.EventOnAnimNoChanged += OnAnimNoChanged;
            m_actionEditUIController.EventOnBtnGoLeftActionElemClick += OnBtnGoLeftActionElemClick;
            m_actionEditUIController.EventOnBtnGoRightActionElemClick += OnBtnGoRightActionElemClick;
            m_actionEditUIController.EventOnBtnCreateActionElemClick += OnBtnCreateActionElemClick;
            m_actionEditUIController.EventOnBtnDeleteActionElemClick += OnBtnDeleteActionElemClick;
            m_actionEditUIController.EventOnScrollActionElemListValueChanged += OnScrollActionElemListValueChanged;
            m_actionEditUIController.EventOnSliderNormalizedTimeValueChanged += OnSliderNormalizedTimeValueChanged;
            m_actionEditUIController.EventOnLabelNormalizedTimeEndEdit += OnLabelNormalizedTimeEndEdit;
            m_actionEditUIController.EventOnLabelDurationTimeValueChanged += OnLabelDurationTimeValueChanged;
            m_actionEditUIController.EventOnToggleLoopStartValueChanged += OnToggleLoopStartValueChanged;
            m_actionEditUIController.EventOnBtnAddClsnClcik += OnBtnAddClsnClcik;
            m_actionEditUIController.EventOnBtnDeleteClsnClick += OnBtnDeleteClsnClick;
            m_actionEditUIController.EventOnBtnUseLastClsnClick += OnBtnUseLastClsnClick;
        }

        protected override void OnClearAllLayerAndAssets()
        {
            base.OnClearAllLayerAndAssets();
            m_canvas = null;
            m_sceneCamera = null;
            if (m_actionEditUIController != null)
            {
                m_actionEditUIController.EventOnCloseButtonClick -= OnCloseButtonClick;

                m_actionEditUIController.EventOnSaveButtonClick -= OnSaveButtonClick;
                m_actionEditUIController.EventOnLoadDropDownValueChanged += OnLoadDropDownValueChanged;
                m_actionEditUIController.EventOnPlayButtonClick -= OnPlayButtonClick;
                m_actionEditUIController.EventOnPauseButtonClick -= OnPauseButtonClick;

                m_actionEditUIController.EventOnPointerDown -= m_actionEdit3DScenneController.OnPointerDown;
                m_actionEditUIController.EventOnDrag -= m_actionEdit3DScenneController.OnDrag;

                m_actionEditUIController.EventOnBtnGoLeftActionClick -= OnGoLeftActionButtonClick;
                m_actionEditUIController.EventOnBtnGoRightActionClick -= OnGoRightActionButtonClick;
                m_actionEditUIController.EventOnScrollActionListValueChange -= OnScrollActionListValueChange;
                m_actionEditUIController.EventOnBtnCreateActionClick -= OnBtnCreateActionClick;
                m_actionEditUIController.EventOnBtnDeleteActionClick -= OnBtnDeleteActionClick;
                m_actionEditUIController.EventOnDropdownAnimNameChanged -= OnDropdownAnimNameChanged;
                m_actionEditUIController.EventOnAnimNoChanged -= OnAnimNoChanged;
                m_actionEditUIController.EventOnBtnGoLeftActionElemClick -= OnBtnGoLeftActionElemClick;
                m_actionEditUIController.EventOnBtnGoRightActionElemClick -= OnBtnGoRightActionElemClick;
                m_actionEditUIController.EventOnScrollActionElemListValueChanged -= OnScrollActionElemListValueChanged;
                m_actionEditUIController.EventOnBtnCreateActionElemClick -= OnBtnCreateActionElemClick;
                m_actionEditUIController.EventOnBtnDeleteActionElemClick -= OnBtnDeleteActionElemClick;
                m_actionEditUIController.EventOnSliderNormalizedTimeValueChanged -= OnSliderNormalizedTimeValueChanged;
                m_actionEditUIController.EventOnLabelNormalizedTimeEndEdit -= OnLabelNormalizedTimeEndEdit;
                m_actionEditUIController.EventOnLabelDurationTimeValueChanged -= OnLabelDurationTimeValueChanged;
                m_actionEditUIController.EventOnToggleLoopStartValueChanged -= OnToggleLoopStartValueChanged;
                m_actionEditUIController.EventOnBtnAddClsnClcik -= OnBtnAddClsnClcik;
                m_actionEditUIController.EventOnBtnDeleteClsnClick -= OnBtnDeleteClsnClick;
                m_actionEditUIController.EventOnBtnUseLastClsnClick -= OnBtnUseLastClsnClick;

                m_actionEditUIController = null;
            }
            if (m_actionEdit3DScenneController != null)
            {

            }
        }
        protected override void UpdateView()
        {
            if (m_updateCtx.m_isInit) {
                //设置配置角色下拉列表
                List<string> names = new List<string>();
                foreach (var character in m_cacheCharacterConfigs)
                {
                    names.Add(character.Name);
                }
                m_actionEditUIController.SetCharsDropDown(names);
            }
            if (m_updateCtx.IsUpdateMaskActive((int)ActionsEditorUITaskUpdateMask.Whole))
            {
                if (m_curCharacterConfig != null)
                {
                    //实例化角色prefab
                    m_actionEdit3DScenneController.SetCharacter(GetAsset<GameObject>(m_curCharacterConfig.Prefab));
                    //初始动画名列表
                    InitAnimNameList(m_actionEdit3DScenneController.AnimController.anim);
                    m_actionEditUIController.SetDropdownAnimName(m_animNameList);
                   
                    m_actionEditUIController.SetActionDefList(m_actionDefList);
                }
            }
            m_actionEditUIController.UpdateUI(m_curActionIndex, m_curActionElemIndex);
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
            {
                m_actionEdit3DScenneController.SampleCharacterAnim(m_actionDefList[m_curActionIndex].animName, m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex].normalizeTime.AsFloat());
            }
        }

        protected override void OnTick()
        {
            if (m_coroutineScheduler != null) {
                m_coroutineScheduler.Tick();
            }
        }

        #endregion

        #region 内部函数

        public Vector3 ScenePosToUIPos(Vector3 wPos)
        {
            Vector3 screenPos = m_sceneCamera.WorldToScreenPoint(wPos);
            screenPos.z = m_canvas.planeDistance;
            var uiPos = m_canvas.worldCamera.ScreenToWorldPoint(screenPos);
            return uiPos;
        }

        public Vector3 UIPosToScenePos(Vector3 uiPos)
        {
            var screenPos = m_canvas.worldCamera.WorldToScreenPoint(uiPos);
            var wPos = m_sceneCamera.ScreenToWorldPoint(screenPos);
            return wPos;
        }

        private void InitAnimNameList(Animation anim) {
            m_animNameList.Clear();
            foreach (AnimationState state in anim)
            {
                m_animNameList.Add(state.name);
            }
        }

        #endregion

        #region UI回调

        private void OnGoLeftActionButtonClick()
        {
            UGFramework.Log.Debug.Log("OnGoLeftActionButtonClick");
            if (m_actionDefList == null || m_actionDefList.Count == 0)
                return;
            m_curActionIndex--;
            if (m_curActionIndex < 0)
            {
                m_curActionIndex = m_actionDefList.Count - 1;
            }
            m_curActionElemIndex = 0;

            StartUpdateView();
        }

        private void OnGoRightActionButtonClick()
        {
            UGFramework.Log.Debug.Log("OnGoRightActionButtonClick");
            if (m_actionDefList == null || m_actionDefList.Count == 0)
                return;
            m_curActionIndex++;
            if (m_curActionIndex >= m_actionDefList.Count)
            {
                m_curActionIndex = 0;
            }
            m_curActionElemIndex = 0;

            StartUpdateView();
        }

        private void OnScrollActionListValueChange(float value) {
            var step = m_actionDefList.Count;
            int index = (int)(value * (step - 1));
            m_curActionIndex = index;
            m_curActionElemIndex = 0;

            StartUpdateView();
        }

        private void OnBtnCreateActionClick() {
            int id = 0;
            if (m_actionDefList.Count > 0)
            {
                id = m_actionDefList[m_actionDefList.Count-1].animNo + 1;
            }
            ActionDef a = new ActionDef(id);
            m_actionDefList.Add(a);
            m_curActionIndex = m_actionDefList.Count - 1;
            m_curActionElemIndex = 0;
            StartUpdateView();
        }

        private void OnBtnDeleteActionClick()
        {
            if (m_actionDefList.Count == 0)
                return;
            int newIndex = m_curActionIndex;
            if (m_curActionIndex == 0)
            {//delete in the start
                newIndex = 0;
            }
            else if (m_curActionIndex == m_actionDefList.Count - 1)
            {//in end
                newIndex = m_actionDefList.Count - 2;
            }
            else
            {
                newIndex = m_curActionIndex;
            }
            m_actionDefList.RemoveAt(m_curActionIndex);
            m_curActionIndex = newIndex;
            StartUpdateView();
        }

        public void OnDropdownAnimNameChanged(int index)
        {
            m_actionDefList[m_curActionIndex].animName = m_animNameList[index];

            StartUpdateView();
        }

        public void OnAnimNoChanged(string animNo)
        {
            m_actionDefList[m_curActionIndex].animNo = int.Parse(animNo);
        }

        public void OnBtnGoLeftActionElemClick()
        {
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
            {
                m_curActionElemIndex--;
                if (m_curActionElemIndex < 0)
                {
                    m_curActionElemIndex = m_actionDefList[m_curActionIndex].frames.Count - 1;
                }
            }
            StartUpdateView();
        }

        public void OnBtnGoRightActionElemClick()
        {
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
            {
                m_curActionElemIndex++;
                if (m_curActionElemIndex >= m_actionDefList[m_curActionIndex].frames.Count)
                {
                    m_curActionElemIndex = 0;
                }
            }
            StartUpdateView();
        }

        public void OnScrollActionElemListValueChanged(float value)
        {
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
            {
                var step = m_actionDefList[m_curActionIndex].frames.Count;
                int index = (int)(value * (step - 1));
                m_curActionElemIndex = index;
                StartUpdateView();
            }
        }

        private void OnBtnCreateActionElemClick()
        {
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null)
            {
                ActionFrame frame = new ActionFrame();
                int newIndex = 0;
                if (m_actionDefList[m_curActionIndex].frames.Count == 0)
                {
                    m_actionDefList[m_curActionIndex].frames.Add(frame);
                    newIndex = 0;
                }
                else
                {
                    frame.normalizeTime = m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex].normalizeTime;
                    m_actionDefList[m_curActionIndex].frames.Insert(m_curActionElemIndex + 1, frame);
                    newIndex = m_curActionElemIndex + 1;
                }
                m_curActionElemIndex = newIndex;
                StartUpdateView();
            }
        }

        private void OnBtnDeleteActionElemClick()
        {
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count!=0)
            {
                int newIndex = m_curActionElemIndex;
                if (m_curActionElemIndex == 0)
                {//delete in the start
                    newIndex = 0;
                }
                else if (m_curActionElemIndex == m_actionDefList[m_curActionIndex].frames.Count - 1)
                {//in end
                    newIndex = m_actionDefList[m_curActionIndex].frames.Count - 2;
                }
                else
                {//in middle
                    newIndex = m_curActionElemIndex;
                }
                m_actionDefList[m_curActionIndex].frames.RemoveAt(m_curActionElemIndex);
                m_curActionElemIndex = newIndex;
                StartUpdateView();
            }
        }

        public void OnLabelNormalizedTimeEndEdit(string content)
        {
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
            {
                m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex].normalizeTime = float.Parse(content).ToNumber();
                StartUpdateView();
            }
        }

        public void OnSliderNormalizedTimeValueChanged(float value)
        {
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
            {
                m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex].normalizeTime = value.ToNumber();
                StartUpdateView();
            }
        }

        public void OnLabelDurationTimeValueChanged(string s)
        {
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
            {
                m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex].duration = int.Parse(s);
                StartUpdateView();
            }
        }

        public void OnToggleLoopStartValueChanged(bool value)
        {
            UGFramework.Log.Debug.Log("OnToggleLoopStartValueChanged:" + value);
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
            {
                if (value) {
                    m_actionDefList[m_curActionIndex].loopStartIndex = m_curActionElemIndex;
                }
                else
                {
                    m_actionDefList[m_curActionIndex].loopStartIndex = -1;
                }
                StartUpdateView();
            }
        }

        public void OnBtnAddClsnClcik(int type)
        {
            UGFramework.Log.Debug.Log(string.Format("OnBtnAddClsnClcik type:{0}", type));
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
            {
                Clsn clsn = new Clsn();
                clsn.type = type;
                clsn.x1 = -1;
                clsn.y1 = -1;
                clsn.x2 = 1;
                clsn.y2 = 1;
                m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex].clsns.Add(clsn);
            }
            StartUpdateView();
        }

        public void OnBtnDeleteClsnClick(Clsn clsn)
        {
            UGFramework.Log.Debug.Log("OnBtnDeleteClsnClick");
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
            {
                m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex].clsns.Remove(clsn);
            }
            StartUpdateView();
        }

        public void OnBtnUseLastClsnClick()
        {
            UGFramework.Log.Debug.Log("OnBtnUseLastClsnClick");
            if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count >= 2 && m_curActionElemIndex >= 1)
            {
                m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex].clsns = new List<Clsn>();
                var lastElem = m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex - 1];
                foreach (var clsn in lastElem.clsns)
                {
                    Clsn newClsn = new Clsn(clsn);
                    m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex].clsns.Add(newClsn);
                }
                StartUpdateView();
            }
        }

       
        private void OnLoadDropDownValueChanged(int index)
        {
            ConfigDataCharacter characterConfig = m_cacheCharacterConfigs[index];
            UGFramework.Log.Debug.Log(string.Format("ActionsEditorUITask.OnLoadDropDownValueChanged index:{0} name:{1}", index, characterConfig.Name));
            m_curCharacterConfig = characterConfig;
            m_updateCtx.ActiveUpdateMask((int)ActionsEditorUITaskUpdateMask.Whole);
            StartUpdateUITask();
        }

        private void OnSaveButtonClick() {
            if (m_curCharacterConfig == null)
                return;
            YamlDotNet.Serialization.Serializer serializer = new Serializer();
            StringWriter strWriter = new StringWriter();
            serializer.Serialize(strWriter, m_actionDefList);
            using (var writer = File.CreateText(Application.dataPath + "/GameProject/RuntimeAssets/" + m_curCharacterConfig.ActionDef))
            {
                writer.Write(strWriter.ToString());
            }
            UGFramework.Log.Debug.Log("save success");
        }

        IEnumerator Coro_PlayAnim() {
            while (m_isPlaying) {
                if (m_actionDefList != null && m_actionDefList.Count != 0 && m_actionDefList[m_curActionIndex].frames != null && m_actionDefList[m_curActionIndex].frames.Count != 0)
                {
                    m_curActionElemIndex++;
                    if (m_curActionElemIndex >= m_actionDefList[m_curActionIndex].frames.Count)
                    {
                        m_curActionElemIndex = 0;
                    }
                    m_actionEditUIController.UpdateUI(m_curActionIndex, m_curActionElemIndex);
                    m_actionEdit3DScenneController.SampleCharacterAnim(m_actionDefList[m_curActionIndex].animName, m_actionDefList[m_curActionIndex].frames[m_curActionElemIndex].normalizeTime.AsFloat());
                }
                else
                {
                    break;
                }
                yield return new WaitForSeconds(1.0f/ 60.0f);
            }
            yield return null;
        }

        public void OnPlayButtonClick()
        {
            if (m_isPlaying)
                return;
            m_isPlaying = true;
            m_actionEditUIController.SetPlayButtonState(true);
            if (m_coroutineScheduler == null)
                m_coroutineScheduler = new CoroutineScheduler();
            m_coroutineScheduler.StartCorcoutine(Coro_PlayAnim());
        }

        public void OnPauseButtonClick()
        {
            m_isPlaying = false;
            m_actionEditUIController.SetPlayButtonState(false);
        }

        private void OnCloseButtonClick()
        {
            ReturnToPrevUITask();
        }
        #endregion

        #region 变量

        private Camera m_sceneCamera;
        private Canvas m_canvas;

        private bool m_isPlaying = false;
        private CoroutineScheduler m_coroutineScheduler;

        private ConfigDataCharacter m_curCharacterConfig = null;
        private List<string> m_animNameList = new List<string>();
        private List<ActionDef> m_actionDefList = null;
        private int m_curActionIndex;
        private int m_curActionElemIndex;

        private List<ConfigDataCharacter> m_cacheCharacterConfigs = null;

        private ActionsEditorUIController m_actionEditUIController = null;
        private ActionEditor3DSceneController m_actionEdit3DScenneController = null;

        #region 资源描述

        protected override LayerDesc[] LayerDescArray
        {
            get
            {
                return m_layerDescs;
            }
        }

        private LayerDesc[] m_layerDescs = new LayerDesc[] {
            new LayerDesc(){
                LayerName = "ActionEditorUI",
                AssetPath = "Assets/GameProject/Tools/ActionsEditor/Prefabs/ActionsEditorUIPrefab.prefab",
            },
            new LayerDesc(){
                LayerName = "ActionEditor3DScene",
                AssetPath = "Assets/GameProject/Tools/ActionsEditor/Prefabs/ActionsEditor3DScenePrefab.prefab",
                IsUILayer = false,
            },
        };

        protected override ViewControllerDesc[] ViewControllerDescArray
        {
            get { return m_viewControllerDescs; }
        }

        private ViewControllerDesc[] m_viewControllerDescs = new ViewControllerDesc[]{
            new ViewControllerDesc()
            {
                AtachLayerName = "ActionEditorUI",
                AtachPath = "./",
                TypeFullName = "bluebean.Mugen3D.UI.ActionsEditorUIController",
            },
            new ViewControllerDesc()
            {
                AtachLayerName = "ActionEditor3DScene",
                AtachPath = "./",
                TypeFullName = "bluebean.Mugen3D.UI.ActionEditor3DSceneController",
            },
        };
        #endregion

        #endregion
    }
}
