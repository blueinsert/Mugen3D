using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bluebean.Mugen3D.ClientGame
{
    public class PopupContinueMenu : UIView
    {
        public Button buttonContinue;
        public Button buttonReturn;

        public System.Action onClicReturn;
        public System.Action onClicContinue;

        private void Awake()
        {
            buttonContinue = this.transform.Find("Pos/ButtonContinue").GetComponent<Button>();
            buttonReturn = this.transform.Find("Pos/ButtonReturn").GetComponent<Button>();
            buttonContinue.onClick.AddListener(() => {
                buttonContinue.enabled = false;
                if (this.onClicContinue != null)
                    this.onClicContinue();
            });
            buttonReturn.onClick.AddListener(() => {
                buttonReturn.enabled = false;
                if (this.onClicReturn != null)
                    this.onClicReturn();
            });
        }
    }
}