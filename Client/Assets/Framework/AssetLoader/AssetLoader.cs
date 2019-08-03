using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.UGFramework
{
    public partial class AssetLoader : ITickable
    {

        #region 单例模式
        private AssetLoader() { }
        private static AssetLoader m_instance;
        public static AssetLoader Instance
        {
            get
            {
                return m_instance;
            }
        }
        public static AssetLoader CreateInstance()
        {
            m_instance = new AssetLoader();
            return m_instance;
        }
        #endregion

        private readonly CoroutineScheduler m_coroutineManager = new CoroutineScheduler();

        private void LoadAssetByAssetDatabase<T>(string path, Action<string, T> onEnd) where T : UnityEngine.Object
        {
            var obj = AssetDatabase.LoadAssetAtPath<T>(path);
            if(obj != null)
            {
                Debug.Log("Load Asset Success by AssetDataBase AssetPath:" + path);
            }
            onEnd(path, obj as T);
        }

        private IEnumerable LoadAssetFromBundle<T>(string path, Action<string, T> onEnd) where T : UnityEngine.Object
        {
            //todo
            yield return null;
            onEnd(path, null);
        }

        private IEnumerator LoadAssetByResourceLoad<T>(string path, Action<string, T> onEnd) where T : UnityEngine.Object
        {
            //从全路径中解析出相对Resources的路径
            string pathInResoruces = PathHelper.GetSubPathInResources(path);
            //去掉文件扩展名
            pathInResoruces = PathHelper.RemoveExtension(pathInResoruces);
            var resourcesRequest = Resources.LoadAsync<T>(path);
            while (!resourcesRequest.isDone)
            {
                yield return null;
            }
            if (resourcesRequest.asset != null)
            {
                Debug.Log("Load Asset Success by ResourceLoad AssetPath:" + path);
            }
            onEnd(path, resourcesRequest.asset as T);
        }

        public IEnumerator LoadAsset<T>(string path, Action<string, T> onEnd) where T : UnityEngine.Object
        {
            UnityEngine.Object obj = null;
            //1.尝试从缓存中获取
            /*
            if(GetAssetFromCache(path, out obj))
            {
                onEnd(path, obj as T);
                yield break;
            }
            */
            //2.从assetBundle中获取
            if(obj == null)
            {
                yield return LoadAssetFromBundle<T>(path, (p, o) => { obj = o; });
            }
            //3.使用assetDataBase加载
            if (Application.isEditor && obj == null)
            {
                LoadAssetByAssetDatabase<T>(path, (p, o) => { obj = o; });
            }
            //4.使用resource加载
            if (obj == null)
            {
                yield return LoadAssetByResourceLoad<T>(path, (p, asset) => { obj = asset; });
            }
            onEnd(path, obj as T);
        }

        public void StartLoadAssetCoroutine<T>(string path, Action<string, T> onEnd) where T : UnityEngine.Object
        {
            m_coroutineManager.StartCorcoutine(LoadAsset(path, onEnd));
        }

        public void StartLoadAssetCoroutine<T>(List<string> assetPaths, Action<Dictionary<string,T>> onComplete) where T : UnityEngine.Object
        {
            Dictionary<string, T> assetDic = new Dictionary<string, T>();
            int subCoroutineNum = 3;
            int subCoroutineCompleteCount = 0;
            for(int i = 0; i < subCoroutineNum; i++)
            {
                m_coroutineManager.StartCorcoutine(AssetLoadSubCoroutine(subCoroutineNum, i, assetPaths, assetDic, ()=> {
                    subCoroutineCompleteCount++;
                    if(subCoroutineCompleteCount == subCoroutineNum)
                    {
                        onComplete(assetDic);
                    }
                }));
            }
            
        }

        IEnumerator AssetLoadSubCoroutine<T>(int subCoroutineCount, int subCoroutineIndex, List<string> assetPaths, Dictionary<string, T> assetDic, Action onComplete) where T : UnityEngine.Object
        {
            for(int i = 0; i < assetPaths.Count; i++)
            {
                if ((i % subCoroutineCount) == subCoroutineIndex)
                {
                    var coroutine = LoadAsset<T>(assetPaths[i], (p, asset) =>
                    {
                        assetDic.Add(p, asset);
                    });
                    yield return coroutine;
                }
            }
            onComplete();
        }

        public void Tick()
        {
            m_coroutineManager.Tick();
        }
    }
}

