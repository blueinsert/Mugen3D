using bluebean.UGFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.UI
{
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
            return false;
        }

        protected override bool IsNeedLoadAssets()
        {
            return true;
        }

        protected override List<string> CollectAssetPathsToLoad()
        {
            List<string> resPath = new List<string>();
            return resPath;
        }

        protected override void OnCreateAllUIViewController()
        {
            base.OnCreateAllUIViewController();
            
        }

        protected override void UpdateView()
        {
            
        }

        protected override void OnTick()
        {
            
        }

        #endregion

        #region UI回调
        #endregion

        #region 变量

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
                AssetPath = "Assets/GameProject/Tools/ActionsEditor/Prefabs/ActionsEditor.prefab",
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
                TypeFullName = "bluebean.Mugen3D.UI.ActionEditUIController",
            },
        };
        #endregion

        #endregion
    }
}
