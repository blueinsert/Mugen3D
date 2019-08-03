using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework.UI
{
    public class UIUpdateContext
    {
        public Action m_onViewUpdateComplete;
        public Action m_redirctOnLoadAllAssetComplete;
        public bool m_isInit;
        public bool m_isResume;
        private int m_updateMask;

        public void ActiveUpdateMask(int slot)
        {
            m_updateMask = m_updateMask | 1 << slot;
        }

        public bool IsUpdateMaskActive(int slot)
        {
            return (m_updateMask & 1 << slot) != 0;
        }

        public void SetRedirectOnLoadAssetComplete(Action action)
        {
            m_redirctOnLoadAllAssetComplete = action;
        }

        public void Clear()
        {
            m_onViewUpdateComplete = null;
            m_redirctOnLoadAllAssetComplete = null;
            m_updateMask = 0;
            m_isInit = false;
            m_isResume = false;
        }
    }

    public class UILayerDesc
    {
        public string LayerName { get; set; }
        public string AssetPath { get; set; }
    }

    public class UIViewControllerDesc
    {
        public string AtachLayerName { get; set; }
        public string AtachPath { get; set; }
        public string TypeFullName { get; set; }
    }

    public class UITask : Task
    {
        public UIUpdateContext UpdateCtx { get { return m_updateCtx; } }

        private UISceneLayer MainLayer
        {
            get
            {
                if (UILayerDescArray == null || UILayerDescArray.Length == 0 || m_uiLayerDic.Count == 0)
                {
                    return null;
                }
                return m_uiLayerDic[UILayerDescArray[0].LayerName];
            }
        }

        private int m_instanceID = 0;

        private readonly UIUpdateContext m_updateCtx = new UIUpdateContext();

        private readonly Dictionary<string, UnityEngine.Object> m_assetDic = new Dictionary<string, UnityEngine.Object>();

        protected readonly Dictionary<string, UISceneLayer> m_uiLayerDic = new Dictionary<string, UISceneLayer>();

        protected UIViewController[] m_uiViewControllerArray = null;

        protected virtual UILayerDesc[] UILayerDescArray { get; set; }

        protected virtual UIViewControllerDesc[] UIViewControllerDescArray { get; set; }

        public UITask(string name) : base(name) { }

        #region 内部方法

        /// <summary>
        /// 隐藏所有显示层
        /// </summary>
        private void HideAllLayers()
        {
            foreach (var layer in m_uiLayerDic.Values)
            {
                if (layer != null && layer.State == SceneLayerState.Using)
                {
                    SceneTree.Instance.PopLayer(layer);
                }
            }
        }

        /// <summary>
        /// 销毁所有显示层
        /// </summary>
        private void DestroyAllLayers()
        {
            foreach (var layer in m_uiLayerDic.Values)
            {
                if (layer != null)
                    SceneTree.Instance.FreeLayer(layer);
            }
        }

        #endregion

        #region 公共方法

        public void ReturnFromRedirect()
        {
            StartUpdateView();
        }

        public void OnNewIntent(UIIntent intent)
        {
            StartUpdateUITask(intent);
        }

        public void SetInstanceID(int instanceID)
        {
            m_instanceID = instanceID;
        }

        #endregion

        #region Task重载方法

        protected override bool OnStart(object param)
        {
            StartUpdateUITask(param as UIIntent);
            return true;
        }

        protected override void OnPause()
        {
            HideAllLayers();
        }

        protected override bool OnResume(object param)
        {
            StartUpdateUITask(param as UIIntent);
            if (MainLayer != null)
                SceneTree.Instance.PushLayer(MainLayer);
            return true;
        }

        protected override void OnStop()
        {
            DestroyAllLayers();
            //clear cache
            m_assetDic.Clear();
            m_uiLayerDic.Clear();
        }

        #endregion

        #region UI更新流程

        /// <summary>
        /// 在启动之前，准备需要耗时的数据准备，比如从网络上拉取
        /// </summary>
        /// <param name="onPrepareEnd"></param>
        public virtual void PrapareDataForStart(Action<bool> onPrepareEnd)
        {
            onPrepareEnd(true);
        }

        /// <summary>
        /// 更新UITask
        /// </summary>
        /// <param name="intent"></param>
        protected void StartUpdateUITask(UIIntent intent)
        {
            bool isNeedUpdateCache = IsNeedUpdateCache();
            if (isNeedUpdateCache)
            {
                UpdateCache();
            }
            bool isNeedLoadUILayer = IsNeedLoadUILayer();
            bool isNeedLoadAssets = IsNeedLoadAssets();
            if (isNeedLoadUILayer || isNeedLoadAssets)
            {
                bool isLoadUILayerComplete = !isNeedLoadUILayer;
                bool isLoadAssetsComplete = !isNeedLoadAssets;
                if (isNeedLoadUILayer)
                {
                    LoadUILayer(() =>
                    {
                        isLoadUILayerComplete = true;
                        if (isLoadUILayerComplete && isLoadAssetsComplete)
                        {
                            OnLoadUILayersAndAssetsComplete();
                        }
                    });
                }
                if (isNeedLoadAssets)
                {
                    LoadAssets(() =>
                    {
                        isLoadAssetsComplete = true;
                        if (isLoadUILayerComplete && isLoadAssetsComplete)
                        {
                            OnLoadUILayersAndAssetsComplete();
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 是否需要更新缓存
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsNeedUpdateCache()
        {
            return false;
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        protected virtual void UpdateCache()
        {

        }

        /// <summary>
        /// 是否需要加载UI显示层
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsNeedLoadUILayer()
        {
            return m_updateCtx.m_isInit;
        }

        /// <summary>
        /// 加载UI显示层
        /// </summary>
        /// <param name="onComplete"></param>
        protected virtual void LoadUILayer(Action onComplete)
        {
            var uiLayerDescs = UILayerDescArray;
            if (uiLayerDescs == null || uiLayerDescs.Length == 0)
            {
                onComplete();
            }
            List<UILayerDesc> toLoadLayerDescs = new List<UILayerDesc>();
            foreach (var layerDesc in uiLayerDescs)
            {
                if (!m_uiLayerDic.ContainsKey(layerDesc.LayerName))
                {
                    toLoadLayerDescs.Add(layerDesc);
                }
            }
            if (toLoadLayerDescs.Count == 0)
            {
                onComplete();
            }
            int toLoadUILayerNum = toLoadLayerDescs.Count;
            int loadCompleteCount = 0;
            foreach (var uiLayerDesc in toLoadLayerDescs)
            {
                SceneTree.Instance.CreateLayer(SceneLayerType.UI, uiLayerDesc.LayerName, m_instanceID, uiLayerDesc.AssetPath, (layer) =>
                {
                    m_uiLayerDic.Add(uiLayerDesc.LayerName, layer as UISceneLayer);
                    loadCompleteCount++;
                    if (loadCompleteCount == toLoadUILayerNum)
                    {
                        onComplete();
                    }
                });
            }
        }

        /// <summary>
        /// 是否需要加载资源
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsNeedLoadAssets()
        {
            return false;
        }

        /// <summary>
        /// 收集资源路径List
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> CollectAssetPathsToLoad()
        {
            return new List<string>();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="onComplete"></param>
        private void LoadAssets(Action onComplete)
        {
            var assetPaths = CollectAssetPathsToLoad();
            List<string> realAssetPaths = new List<string>();
            foreach (string path in assetPaths)
            {
                if (!m_assetDic.ContainsKey(path))
                {
                    realAssetPaths.Add(path);
                }
            }
            if (realAssetPaths.Count == 0)
            {
                onComplete();
            }
            else
            {
                AssetLoader.Instance.StartLoadAssetCoroutine<UnityEngine.Object>(realAssetPaths, (assetDic) =>
                {
                    foreach (var pair in assetDic)
                    {
                        m_assetDic.Add(pair.Key, pair.Value);
                    }
                    onComplete();
                });
            }
        }

        protected virtual void OnCreateAllUIViewController()
        {

        }

        /// <summary>
        /// 创建UI视图控制器
        /// </summary>
        private void CreateAllUIViewController()
        {
            if (UIViewControllerDescArray.Length <= 0)
            {
                return;
            }
            m_uiViewControllerArray = new UIViewController[UIViewControllerDescArray.Length];
            for (int i = 0; i < UIViewControllerDescArray.Length; i++)
            {
                var uiViewControllerDesc = UIViewControllerDescArray[i];
                var uiLayer = m_uiLayerDic[uiViewControllerDesc.AtachLayerName];
                var uiViewController = MonoViewController.AttachViewControllerToGameObject(uiLayer.PrefabInstance, uiViewControllerDesc.AtachPath, uiViewControllerDesc.TypeFullName) as UIViewController;
                m_uiViewControllerArray[i] = uiViewController;
            }
            foreach (var uiViewController in m_uiViewControllerArray)
            {
                uiViewController.AutoBindFields();
            }
            OnCreateAllUIViewController();
        }

        /// <summary>
        /// 加载所有资源完成
        /// </summary>
        private void OnLoadUILayersAndAssetsComplete()
        {
            if (m_uiViewControllerArray == null)
            {
                CreateAllUIViewController();
            }
            if (m_updateCtx.m_isInit)
            {
                if (MainLayer != null)
                    SceneTree.Instance.PushLayer(MainLayer);
            }
            if (m_updateCtx.m_redirctOnLoadAllAssetComplete != null)
            {
                m_updateCtx.m_redirctOnLoadAllAssetComplete();
                m_updateCtx.m_redirctOnLoadAllAssetComplete = null;
                return;
            }
            StartUpdateView();
        }

        /// <summary>
        /// 更新UI视图
        /// </summary>
        protected void StartUpdateView()
        {
            UpdateView();
            if (m_updateCtx.m_onViewUpdateComplete != null)
            {
                m_updateCtx.m_onViewUpdateComplete();
            }
            m_updateCtx.Clear();
        }

        /// <summary>
        /// 更新UI视图
        /// </summary>
        protected virtual void UpdateView()
        {

        }

        #endregion
    }
}
