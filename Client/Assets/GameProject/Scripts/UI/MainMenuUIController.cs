using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;

namespace bluebean.Mugen3D.UI
{
    public class MainMenuUIController : UIViewController
    {
        protected override void OnBindFieldsComplete()
        {
            base.OnBindFieldsComplete();
            SingleVSButton.onClick.AddListener(OnSingleVSButtonClick);
            TrainButton.onClick.AddListener(OnTrainButtonClick);
            OptionsButton.onClick.AddListener(OnOptionsButtonClick);
            ExitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnSingleVSButtonClick()
        {
            if (EventOnSingleVSButtonClick != null)
            {
                EventOnSingleVSButtonClick();
            }
        }

        private void OnTrainButtonClick()
        {
            if (EventOnTrainButtonClick != null)
            {
                EventOnTrainButtonClick();
            }
        }

        private void OnOptionsButtonClick()
        {
            if (EventOnOptionsButtonClick != null)
            {
                EventOnOptionsButtonClick();
            }
        }

        private void OnExitButtonClick()
        {
            if (EventOnExitButtonClick != null)
            {
                EventOnExitButtonClick();
            }
        }

        public event Action EventOnSingleVSButtonClick;
        public event Action EventOnTrainButtonClick;
        public event Action EventOnOptionsButtonClick;
        public event Action EventOnExitButtonClick;

        [AutoBind("./VerticalGroup/SingleVS")]
        public Button SingleVSButton;
        [AutoBind("./VerticalGroup/Train")]
        public Button TrainButton;
        [AutoBind("./VerticalGroup/Options")]
        public Button OptionsButton;
        [AutoBind("./VerticalGroup/Exit")]
        public Button ExitButton;
    }
}
