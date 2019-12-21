using bluebean.Mugen3D.Core;
using bluebean.UGFramework;
using bluebean.UGFramework.ConfigData;
using bluebean.UGFramework.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YamlDotNet.Serialization;

namespace bluebean.Mugen3D.UI
{
    public enum ActionsEditorUITaskUpdateMask {
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

        protected override bool OnResume(UIIntent intent)
        {
            bool res = base.OnResume(intent);
            return res;
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override bool IsNeedUpdateCache()
        {
            if (m_cacheCharacterConfigs == null) {
                var characters = GameManager.Instance.ConfigDataLoader.GetAllConfigDataCharacter();
                m_cacheCharacterConfigs = new List<ConfigDataCharacter>();
                foreach (var character in characters) {
                    m_cacheCharacterConfigs.Add(character.Value);
                }
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
            if (m_curCharacterConfig != null) {
                resPath.Add("Assets/GameProject/RuntimeAssets/" + m_curCharacterConfig.Prefab);
                resPath.Add("Assets/GameProject/RuntimeAssets/" + m_curCharacterConfig.ActionDef);
            }
            return resPath;
        }

        protected override void OnCreateAllUIViewController()
        {
            base.OnCreateAllUIViewController();
          
            m_actionEditUIController = m_viewControllerArray[0] as ActionsEditorUIController;
            m_actionEdit3DScenneController = m_viewControllerArray[1] as ActionEditor3DSceneController;

            m_actionEditUIController.EventOnCloseButtonClick += OnCloseButtonClick;
            m_actionEditUIController.EventOnLoadDropDownValueChanged += OnLoadDropDownValueChanged;
            m_actionEditUIController.EventOnPointerDown += m_actionEdit3DScenneController.OnPointerDown;
            m_actionEditUIController.EventOnDrag += m_actionEdit3DScenneController.OnDrag;
        }

        protected override void OnClearAllLayerAndAssets()
        {
            base.OnClearAllLayerAndAssets();
            if (m_actionEditUIController != null)
            {
                m_actionEditUIController.EventOnCloseButtonClick -= OnCloseButtonClick;
                m_actionEditUIController.EventOnPointerDown -= m_actionEdit3DScenneController.OnPointerDown;
                m_actionEditUIController.EventOnDrag -= m_actionEdit3DScenneController.OnDrag;
                m_actionEditUIController = null;
            }
            if (m_actionEdit3DScenneController != null) {
               
            }
        }
        protected override void UpdateView()
        {
            if (m_updateCtx.IsUpdateMaskActive((int)ActionsEditorUITaskUpdateMask.Whole)){
                if (m_curCharacterConfig != null) {
                    m_actionEdit3DScenneController.SetCharacter(GetAsset<GameObject>(m_curCharacterConfig.Prefab));
                    var deserializer = new Deserializer();
                    m_actionDefList = deserializer.Deserialize<ActionsConfig>(GetAsset<TextAsset>(m_curCharacterConfig.ActionDef).text).actions;
                    UGFramework.Log.Debug.Log(string.Format("serialize {0}'s acitons def", m_curCharacterConfig.Name));
                }
            }
            List<string> names = new List<string>();
            foreach (var character in m_cacheCharacterConfigs) {
                names.Add(character.Name);
            }
            m_actionEditUIController.SetCharsDropDown(names);
        }

        protected override void OnTick()
        {
            
        }

        #endregion

        #region UI回调

        private void OnCloseButtonClick() {
            ReturnToPrevUITask();
        }

        private void OnLoadDropDownValueChanged(int index) {
            ConfigDataCharacter characterConfig = m_cacheCharacterConfigs[index];
            UGFramework.Log.Debug.Log(string.Format("ActionsEditorUITask.OnLoadDropDownValueChanged index:{0} name:{1}",index, characterConfig.Name));
            m_curCharacterConfig = characterConfig;
            m_updateCtx.ActiveUpdateMask((int)ActionsEditorUITaskUpdateMask.Whole);
            StartUpdateUITask();
        }

        #endregion

        #region 变量
        private ConfigDataCharacter m_curCharacterConfig = null;
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
