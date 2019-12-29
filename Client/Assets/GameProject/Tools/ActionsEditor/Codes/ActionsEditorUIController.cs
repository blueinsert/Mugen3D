using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;
using UnityEngine.EventSystems;
using bluebean.Mugen3D.Core;

namespace bluebean.Mugen3D.UI
{
    public class ActionsEditorUIController : UIViewController, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        protected override void OnBindFieldsComplete()
        {
            base.OnBindFieldsComplete();
            m_closeButton.onClick.AddListener(OnCloseButtonClick);
            m_loadDropDown.onValueChanged.AddListener(OnLoadDropDownValueChanged);
            m_saveButton.onClick.AddListener(OnSaveButtonClick);
            m_playButton.onClick.AddListener(OnPlayButtonClick);
            m_pauseButton.onClick.AddListener(OnPauseButtonClick);
            btnGoLeftAction.onClick.AddListener(OnBtnGoLeftActionClick);
            btnGoRightAction.onClick.AddListener(OnBtnGoRightActionClick);
            scrollActionList.onValueChanged.AddListener(OnScrollActionListValueChange);
            btnCreateAction.onClick.AddListener(OnBtnCreateActionClick);
            btnDeleAction.onClick.AddListener(OnBtnDeleteActionClick);
            dropdownAnimName.onValueChanged.AddListener(OnDropdownAnimNameChanged);
            labelAnimNo.onValueChanged.AddListener(OnAnimNoChanged);
            btnGoLeftActionElem.onClick.AddListener(OnBtnGoLeftActionElemClick);
            btnGoRightActionElem.onClick.AddListener(OnBtnGoRightActionElemClick);
            scrollActionElemList.onValueChanged.AddListener(OnScrollActionElemListValueChanged);
            btnCreateActionElem.onClick.AddListener(OnBtnCreateActionElemClick);
            btnDeleActionElem.onClick.AddListener(OnBtnDeleteActionElemClick);
            sliderNormalizedTime.onValueChanged.AddListener(OnSliderNormalizedTimeValueChanged);
            labelNormalizedTime.onEndEdit.AddListener(OnLabelNormalizedTimeEndEdit);
            labelDurationTime.onEndEdit.AddListener(OnLabelDurationTimeValueChanged);
            toggleLoopStart.onValueChanged.AddListener(OnToggleLoopStartValueChanged);
            btnAddClsn1.onClick.AddListener(OnBtnAddClsn1Clcik);
            btnAddClsn2.onClick.AddListener(OnBtnAddClsn2Clcik);
            btnAddClsn3.onClick.AddListener(OnBtnAddClsn3Clcik);
            btnDeleteClsn.onClick.AddListener(OnBtnDeleteClsnClick);
            btnUseLastClsn.onClick.AddListener(OnBtnUseLastClsnClick);
            m_clsnPool.Setup(m_prefabClsn, m_clsnContent);
        }

        /// <summary>
        /// 设置播放按钮状态
        /// </summary>
        /// <param name="isPlaying"></param>
        public void SetPlayButtonState(bool isPlaying) {
            m_playButtonStateCtrl.SetUIState(isPlaying ? "Playing" : "Stopped");
        }

        /// <summary>
        /// 设置下拉角色列表
        /// </summary>
        /// <param name="names"></param>
        public void SetCharsDropDown(List<string> names) {
            m_loadDropDown.ClearOptions();
            m_loadDropDown.AddOptions(names);
        }

        /// <summary>
        /// 设置动画数据
        /// </summary>
        /// <param name="actionDefList"></param>
        public void SetActionDefList(List<ActionDef> actionDefList) {
            m_cacheActionDefList = actionDefList;
        }

        /// <summary>
        /// 设置下拉动画列表
        /// </summary>
        /// <param name="animNameList"></param>
        public void SetDropdownAnimName(List<string> animNameList)
        {
            m_allAnimName = animNameList;
            dropdownAnimName.AddOptions(m_allAnimName);
        }

        /// <summary>
        /// 设置当前动作帧的判断框
        /// </summary>
        /// <param name="frame"></param>
        void SetClsns(ActionFrame frame)
        {
            m_clsnPool.Deactive();
            foreach (var clsn in frame.clsns) {
                bool isNew = false;
                var clsnUICtrl = m_clsnPool.Allocate(out isNew);
                if (isNew) {
                    clsnUICtrl.EventOnToggleValueChanged += OnClsnToggleValueChanged;
                }
                clsnUICtrl.SetClsn(clsn);
            }
        }

        /// <summary>
        /// 更新所有ui内容
        /// </summary>
        /// <param name="curActionIndex"></param>
        /// <param name="curActionElemIndex"></param>
        public void UpdateUI(int curActionIndex, int curActionElemIndex) {
            m_invokeUIListener = false;
            if (m_cacheActionDefList != null && m_cacheActionDefList.Count != 0)
            {
                var curAction = m_cacheActionDefList[curActionIndex];
                this.scrollActionList.numberOfSteps = m_cacheActionDefList.Count;
                this.scrollActionList.size = 1 / (float)(m_cacheActionDefList.Count);
                if (m_cacheActionDefList.Count > 1)
                    this.scrollActionList.value = curActionIndex / (float)(m_cacheActionDefList.Count - 1);
                else
                    this.scrollActionList.value = 0;
                this.dropdownAnimName.value = m_allAnimName.IndexOf(curAction.animName);
                this.labelAnimNo.text = curAction.animNo.ToString();
                this.labelCurActionNo.text = (curActionIndex+1) + "/" + m_cacheActionDefList.Count;
                if (curAction.frames != null && curAction.frames.Count != 0)
                {
                    var curActionElem = curAction.frames[curActionElemIndex];
                    this.scrollActionElemList.numberOfSteps = curAction.frames.Count;
                    this.scrollActionElemList.size = 1 / (float)(curAction.frames.Count);
                    if (curAction.frames.Count > 1)
                        this.scrollActionElemList.value = curActionElemIndex / (float)(curAction.frames.Count - 1);
                    else
                        this.scrollActionElemList.value = 0;
                    this.labelCurActionElemNo.text = (curActionElemIndex+1) + "/" + curAction.frames.Count + "-" + curActionElem.duration + "ticks";
                    this.labelNormalizedTime.text = curActionElem.normalizeTime.ToString();
                    this.sliderNormalizedTime.value = curActionElem.normalizeTime.AsFloat();
                    this.labelDurationTime.text = curActionElem.duration.ToString();
                    this.labelXOffset.text = curActionElem.xOffset.ToString();
                    this.labelYOffset.text = curActionElem.yOffset.ToString();
                    this.toggleLoopStart.isOn = curActionElemIndex == curAction.loopStartIndex;
                    SetClsns(curActionElem);
                }
                else
                {
                    this.labelCurActionElemNo.text = "0/0";
                    this.scrollActionElemList.numberOfSteps = 0;
                }
            }
            else
            {
                this.labelCurActionNo.text = "0/0";
                this.scrollActionList.numberOfSteps = 0;
            }
            m_invokeUIListener = true;
        }

        private void OnCloseButtonClick() {
            if (EventOnCloseButtonClick != null && m_invokeUIListener)
            {
                EventOnCloseButtonClick();
            }
        }

        private void OnLoadDropDownValueChanged(int index) {
            if (EventOnLoadDropDownValueChanged != null && m_invokeUIListener) {
                EventOnLoadDropDownValueChanged(index);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (EventOnPointerDown != null && m_invokeUIListener) {
                EventOnPointerDown(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (EventOnDrag != null && m_invokeUIListener)
            {
                EventOnDrag(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (EventOnPointerUp != null && m_invokeUIListener)
            {
                EventOnPointerUp(eventData);
            }
        }

        private void OnBtnGoLeftActionClick() {
            if (EventOnBtnGoLeftActionClick != null && m_invokeUIListener) {
                EventOnBtnGoLeftActionClick();
            }
        }

        private void OnBtnGoRightActionClick()
        {
            if (EventOnBtnGoRightActionClick != null && m_invokeUIListener)
            {
                EventOnBtnGoRightActionClick();
            }
        }

        private void OnScrollActionListValueChange(float value) {
            if (EventOnScrollActionListValueChange != null && m_invokeUIListener) {
                EventOnScrollActionListValueChange(value);
            }
        }

        private void OnBtnCreateActionClick() {
            if (EventOnBtnCreateActionClick != null && m_invokeUIListener)
            {
                EventOnBtnCreateActionClick();
            }
        }

        private void OnBtnDeleteActionClick()
        {
            if (EventOnBtnDeleteActionClick != null && m_invokeUIListener)
            {
                EventOnBtnDeleteActionClick();
            }
        }

        private void OnDropdownAnimNameChanged(int index) {
            if (EventOnDropdownAnimNameChanged != null && m_invokeUIListener) {
                EventOnDropdownAnimNameChanged(index);
            }
        }

        public void OnAnimNoChanged(string animNo) {
            if (EventOnAnimNoChanged != null && m_invokeUIListener) {
                EventOnAnimNoChanged(animNo);
            }
        }

        public void OnBtnGoLeftActionElemClick() {
            if (EventOnBtnGoLeftActionElemClick != null && m_invokeUIListener)
            {
                EventOnBtnGoLeftActionElemClick();
            }
        }

        public void OnBtnGoRightActionElemClick()
        {
            if (EventOnBtnGoRightActionElemClick != null && m_invokeUIListener)
            {
                EventOnBtnGoRightActionElemClick();
            }
        }

        private void OnBtnCreateActionElemClick()
        {
            if (EventOnBtnCreateActionElemClick != null && m_invokeUIListener)
            {
                EventOnBtnCreateActionElemClick();
            }
        }

        private void OnBtnDeleteActionElemClick()
        {
            if (EventOnBtnDeleteActionElemClick != null && m_invokeUIListener)
            {
                EventOnBtnDeleteActionElemClick();
            }
        }

        public void OnScrollActionElemListValueChanged(float value) {
            if (EventOnScrollActionElemListValueChanged != null && m_invokeUIListener)
            {
                EventOnScrollActionElemListValueChanged(value);
            }
        }

        public void OnLabelNormalizedTimeEndEdit(string content) {
            if (EventOnLabelNormalizedTimeEndEdit != null && m_invokeUIListener)
            {
                EventOnLabelNormalizedTimeEndEdit(content);
            }
        }

        public void OnSliderNormalizedTimeValueChanged(float value) {
            if (EventOnSliderNormalizedTimeValueChanged != null && m_invokeUIListener)
            {
                EventOnSliderNormalizedTimeValueChanged(value);
            }
        }

        public void OnLabelDurationTimeValueChanged(string s) {
            if (EventOnLabelDurationTimeValueChanged != null && m_invokeUIListener)
            {
                EventOnLabelDurationTimeValueChanged(s);
            }
        }

        public void OnToggleLoopStartValueChanged(bool value) {
            if (EventOnToggleLoopStartValueChanged != null && m_invokeUIListener)
            {
                EventOnToggleLoopStartValueChanged(value);
            }
        }

        public void OnBtnAddClsn1Clcik() {
            if (EventOnBtnAddClsnClcik != null && m_invokeUIListener)
            {
                EventOnBtnAddClsnClcik(1);
            }
        }

        public void OnBtnAddClsn2Clcik()
        {
            if (EventOnBtnAddClsnClcik != null && m_invokeUIListener)
            {
                EventOnBtnAddClsnClcik(2);
            }
        }

        public void OnBtnAddClsn3Clcik()
        {
            if (EventOnBtnAddClsnClcik != null && m_invokeUIListener)
            {
                EventOnBtnAddClsnClcik(3);
            }
        }

        public void OnBtnDeleteClsnClick()
        {
            if (EventOnBtnDeleteClsnClick != null && m_invokeUIListener)
            {
                if(m_curSelectClsn!=null)
                    EventOnBtnDeleteClsnClick(m_curSelectClsn);
            }
        }

        public void OnBtnUseLastClsnClick()
        {
            if (EventOnBtnUseLastClsnClick != null && m_invokeUIListener)
            {
                EventOnBtnUseLastClsnClick();
            }
        }

        public void OnSaveButtonClick() {
            if (EventOnSaveButtonClick != null && m_invokeUIListener)
            {
                EventOnSaveButtonClick();
            }
        }

        public void OnPlayButtonClick() {
            if (EventOnPlayButtonClick != null && m_invokeUIListener)
            {
                EventOnPlayButtonClick();
            }
        }

        public void OnPauseButtonClick()
        {
            if (EventOnPauseButtonClick != null && m_invokeUIListener)
            {
                EventOnPauseButtonClick();
            }
        }

        private void OnClsnToggleValueChanged(Clsn clsn,bool value) {
            if (value)
                m_curSelectClsn = clsn;
            else
                m_curSelectClsn = null;
        }

        public Action EventOnBtnUseLastClsnClick;
        public Action<Clsn> EventOnBtnDeleteClsnClick;
        public Action<int> EventOnBtnAddClsnClcik;
        public Action<bool> EventOnToggleLoopStartValueChanged;
        public Action<string> EventOnLabelDurationTimeValueChanged;
        public Action<float> EventOnSliderNormalizedTimeValueChanged;
        public Action<string> EventOnLabelNormalizedTimeEndEdit;
        public Action<float> EventOnScrollActionElemListValueChanged;
        public Action EventOnBtnDeleteActionElemClick;
        public Action EventOnBtnCreateActionElemClick;
        public Action EventOnBtnGoRightActionElemClick;
        public Action EventOnBtnGoLeftActionElemClick;
       
        public Action<string> EventOnAnimNoChanged;
        public Action<int> EventOnDropdownAnimNameChanged;
        public Action<float> EventOnScrollActionListValueChange;
        public Action EventOnBtnDeleteActionClick;
        public Action EventOnBtnCreateActionClick;
        public Action EventOnBtnGoLeftActionClick;
        public Action EventOnBtnGoRightActionClick;
        public Action<PointerEventData> EventOnPointerDown;
        public Action<PointerEventData> EventOnPointerUp;
        public Action<PointerEventData> EventOnDrag;
        public Action<int> EventOnLoadDropDownValueChanged;
        public Action EventOnSaveButtonClick;
        public Action EventOnPlayButtonClick;
        public Action EventOnPauseButtonClick;
        public Action EventOnCloseButtonClick;
        public Action<Clsn> EventOnClsnToggleValueChanged;

        List<string> m_allAnimName = new List<string>();
        List<ActionDef> m_cacheActionDefList;
        private bool m_invokeUIListener = true;
        private EasyGameObjectPool<ClsnUIController> m_clsnPool = new EasyGameObjectPool<ClsnUIController>();
        private Clsn m_curSelectClsn;

        #region AutoBind
        [AutoBind("./CloseButton")]
        public Button m_closeButton;
        [AutoBind("./Left/Menu/Load")]
        public Dropdown m_loadDropDown;
        [AutoBind("./Left/Menu/ButtonSave")]
        public Button m_saveButton;
        [AutoBind("./Left/Menu/ButtonPlay")]
        public Button m_playButton;
        [AutoBind("./Left/Menu/ButtonPlay")]
        public UIStateController m_playButtonStateCtrl;
        [AutoBind("./Left/Menu/ButtonPause")]
        public Button m_pauseButton;
        [AutoBind("./Left/ActionList/ButtonDelete")]
        public Button btnDeleAction;
        [AutoBind("./Left/ActionList/ButtonCreate")]
        public Button btnCreateAction;
        [AutoBind("./Left/ActionList/ButtonLeft")]
        public Button btnGoLeftAction;
        [AutoBind("./Left/ActionList/ButtonRight")]
        public Button btnGoRightAction;
        [AutoBind("./Left/ActionList/Scrollbar")]
        public Scrollbar scrollActionList;
        [AutoBind("./Left/ActionInfo/Text")]
        public Text labelCurActionNo;
        [AutoBind("./Left/ActionInfo/Name/Dropdown")]
        public Dropdown dropdownAnimName;
        [AutoBind("./Left/ActionInfo/No/InputField")]
        public InputField labelAnimNo;
        [AutoBind("./Left/ActionElemList/ButtonDelete")]
        public Button btnDeleActionElem;
        [AutoBind("./Left/ActionElemList/ButtonCreate")]
        public Button btnCreateActionElem;
        [AutoBind("./Left/ActionElemList/ButtonLeft")]
        public Button btnGoLeftActionElem;
        [AutoBind("./Left/ActionElemList/ButtonRight")]
        public Button btnGoRightActionElem;
        [AutoBind("./Left/ActionElemList/Scrollbar")]
        public Scrollbar scrollActionElemList;
        [AutoBind("./Left/ActionElemProperty/CurActionElemInfo")]
        public Text labelCurActionElemNo;
        [AutoBind("./Left/ActionElemProperty/NormalizedTime/InputField")]
        public InputField labelNormalizedTime;
        [AutoBind("./Left/ActionElemProperty/NormalizedTime/Slider")]
        public Slider sliderNormalizedTime;
        [AutoBind("./Left/ActionElemProperty/Duration/InputField")]
        public InputField labelDurationTime;
        [AutoBind("./Left/ActionElemProperty/XOffset/InputField")]
        public InputField labelXOffset;
        [AutoBind("./Left/ActionElemProperty/YOffset/InputField")]
        public InputField labelYOffset;
        [AutoBind("./Left/ActionElemProperty/IsLoopStart/Toggle")]
        public Toggle toggleLoopStart;
        [AutoBind("./Left/ClsnOperate/ButtonAddClsn1")]
        public Button btnAddClsn1;
        [AutoBind("./Left/ClsnOperate/ButtonAddClsn2")]
        public Button btnAddClsn2;
        [AutoBind("./Left/ClsnOperate/ButtonAddClsn3")]
        public Button btnAddClsn3;
        [AutoBind("./Left/ClsnOperate/ButtonDeleteClsn")]
        public Button btnDeleteClsn;
        [AutoBind("./Left/ClsnOperate/ButtonUseLastClsn")]
        public Button btnUseLastClsn;

        [AutoBind("./Right/CharacterArea/ToggleGroup")]
        public GameObject m_clsnContent;
        [AutoBind("./Prefabs/ToggleCLSN")]
        public GameObject m_prefabClsn;
        #endregion
    }
}
