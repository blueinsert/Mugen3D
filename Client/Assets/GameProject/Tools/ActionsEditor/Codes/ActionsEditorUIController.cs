using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;
using UnityEngine.EventSystems;

namespace bluebean.Mugen3D.UI
{
    public class ActionsEditorUIController : UIViewController, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        protected override void OnBindFieldsComplete()
        {
            base.OnBindFieldsComplete();
            m_closeButton.onClick.AddListener(OnCloseButtonClick);
            m_loadDropDown.onValueChanged.AddListener(OnLoadDropDownValueChanged);
            GameObjectUtility.AttachUIController<ClsnUIController>(m_prefabClsn);
        }

        public void SetCharsDropDown(List<string> names) {
            m_loadDropDown.ClearOptions();
            m_loadDropDown.AddOptions(names);
        }

        private void OnCloseButtonClick() {
            if (EventOnCloseButtonClick != null)
            {
                EventOnCloseButtonClick();
            }
        }

        private void OnLoadDropDownValueChanged(int index) {
            if (EventOnLoadDropDownValueChanged != null) {
                EventOnLoadDropDownValueChanged(index);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (EventOnPointerDown != null) {
                EventOnPointerDown(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (EventOnDrag != null)
            {
                EventOnDrag(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (EventOnPointerUp != null)
            {
                EventOnPointerUp(eventData);
            }
        }

        public Action<PointerEventData> EventOnPointerDown;
        public Action<PointerEventData> EventOnPointerUp;
        public Action<PointerEventData> EventOnDrag;
        public Action<int> EventOnLoadDropDownValueChanged;
        public Action EventOnCloseButtonClick;

        [AutoBind("./CloseButton")]
        public Button m_closeButton;
        [AutoBind("./Left/Menu/Load")]
        public Dropdown m_loadDropDown;
        [AutoBind("./Left/Menu/ButtonSave")]
        public Button m_saveButton;
        [AutoBind("./Left/Menu/ButtonPlay")]
        public Button m_playButton;
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
    }
}
