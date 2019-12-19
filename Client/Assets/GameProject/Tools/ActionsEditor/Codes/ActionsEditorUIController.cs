using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;

namespace bluebean.Mugen3D.UI
{
    public class ActionsEditorUIController : UIViewController
    {
        protected override void OnBindFieldsComplete()
        {
            base.OnBindFieldsComplete();
            m_closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnCloseButtonClick() {
            if (EventOnCloseButtonClick != null)
            {
                EventOnCloseButtonClick();
            }
        }

        public Action EventOnCloseButtonClick;

        [AutoBind("./CloseButton")]
        public Button m_closeButton;
        [AutoBind("./Left/Menu/ButtonLoad")]
        public Button m_loadButton;
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
    }
}
