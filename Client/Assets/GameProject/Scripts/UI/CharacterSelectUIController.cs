using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.UI
{
    public class CharacterSelectUIController : UIViewController
    {
        protected override void OnBindFieldsComplete()
        {
            base.OnBindFieldsComplete();
            ReturnButton.onClick.AddListener(OnReturnButtonClick);
            m_objPool.Setup(LittleHeadPrefab, Content);
        }

        public void ShowOpenTween()
        {
            UIStateController.SetUIState("Open");
        }

        public void SetCharacters(List<ConfigDataCharacter> characters)
        {
            m_configDataCharacters.Clear();
            m_configDataCharacters.AddRange(characters);

            m_characterScrollItemUIControllerList.Clear();
            m_objPool.Deactive();
            int index = 0;
            foreach(var character in characters)
            {
                bool isNew = false;
                var viewCtrl = m_objPool.Allocate(out isNew);
                if (isNew)
                {
                    viewCtrl.EventOnClick += OnLittleHeadClick;
                }
                viewCtrl.SetIndex(index++);
                viewCtrl.UpdateInfo(character);
                m_characterScrollItemUIControllerList.Add(viewCtrl);
            }
        }

        public void UpdateUI(int p1Index, int p2Index)
        {
            foreach(var uiCtrl in m_characterScrollItemUIControllerList)
            {
                uiCtrl.SetUIState(p1Index, p2Index);
            }
            //更新p1
            var configP1 = m_configDataCharacters[p1Index];
            P1CharacterNameText.text = configP1.Name;
            //更新p2
            var configP2 = m_configDataCharacters[p2Index];
            P2CharacterNameText.text = configP2.Name;
        }

        #region UI回调

        private void OnLittleHeadClick(int index)
        {
            if (EventOnLittleHeadClick != null)
            {
                EventOnLittleHeadClick(index);
            }
        }

        private void OnReturnButtonClick()
        {
            UIStateController.SetUIState("Close", () => {
                if (EventOnReturnButtonClick != null)
                {
                    EventOnReturnButtonClick();
                }
            });
        }

        #endregion
        public event Action<int> EventOnLittleHeadClick;

        public event Action EventOnReturnButtonClick;

        private readonly List<ConfigDataCharacter> m_configDataCharacters = new List<ConfigDataCharacter>();
        private readonly List<CharacterScrollItemUIController> m_characterScrollItemUIControllerList = new List<CharacterScrollItemUIController>();
        private readonly EasyGameObjectPool<CharacterScrollItemUIController> m_objPool = new EasyGameObjectPool<CharacterScrollItemUIController>();

        [AutoBind("./SelectArea/ScrollRect/Viewport/Content")]
        public GameObject Content;
        [AutoBind("./SelectArea/ScrollRect/ItemRoot/LittleHead")]
        public GameObject LittleHeadPrefab;
        [AutoBind("./ReturnButton")]
        public Button ReturnButton;
        [AutoBind("./")]
        public UIStateController UIStateController;
        [AutoBind("./P1/LabelName")]
        public Text P1CharacterNameText;
        [AutoBind("./P1/ImgMediumHead")]
        public Image P1CharacterIconImage;
        [AutoBind("./P2/LabelName")]
        public Text P2CharacterNameText;
        [AutoBind("./P2/ImgMediumHead")]
        public Image P2CharacterIconImage;
    }

    /// <summary>
    /// 选人物的小头像的UIController
    /// </summary>
    public class CharacterScrollItemUIController : UIViewController
    {
        protected override void OnBindFieldsComplete()
        {
            Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (EventOnClick != null)
            {
                EventOnClick(m_index);
            }
        }

        public void SetUIState(int p1Index, int p2Index)
        {
            if(m_index == p1Index && m_index == p2Index)
            {
                UIStateController.SetUIState("Both");
            }
            else
            {
                if(m_index == p1Index)
                {
                    UIStateController.SetUIState("P1");
                }else if(m_index == p2Index)
                {
                    UIStateController.SetUIState("P2");
                }
                else
                {
                    UIStateController.SetUIState("None");
                }
            }
        }

        public void SetIndex(int index)
        {
            m_index = index;
        }

        public int GetIndex()
        {
            return m_index;
        }

        public void UpdateInfo(ConfigDataCharacter configDataCharacter)
        {
            //todo
        }

        private int m_index;

        public event Action<int> EventOnClick;

        [AutoBind("./")]
        public Button Button;
        [AutoBind("./")]
        public UIStateController UIStateController;
        [AutoBind("./Pos/ImgHead")]
        public Image HeadImage;

    }
}
