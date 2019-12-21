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

        private void LoadAssetByAssetDatabase(string path, bool hasSubAsset, Action<string, UnityEngine.Object[]> onEnd)
        {
            UnityEngine.Object[] assets;
            if (hasSubAsset)
            {
                assets = AssetDatabase.LoadAllAssetsAtPath(path);
            }
            else
            {
                assets = new UnityEngine.Object[1];
                assets[0] = AssetDatabase.LoadAssetAtPath(path,typeof(UnityEngine.Object));
            }
            
            if(assets != null && assets[0] != null)
            {
                Debug.Log("Load Asset Success by AssetDataBase AssetPath:" + path);
            }
            else
            {
                Debug.LogError("Load Asset Fail by AssetDataBase AssetPath:" + path);
            }
            onEnd(path, assets);
        }

        private IEnumerable LoadAssetFromBundle<T>(string path, Action<string, T> onEnd) where T : UnityEngine.Object
        {
            //todo
            yield return null;
            onEnd(path, null);
        }

        private IEnumerator LoadAssetByResourceLoad(string path, bool hasSubAsset, Action<string, UnityEngine.Object[]> onEnd)
        {
            //从全路径中解析出相对Resources的路径
            string pathInResoruces = PathHelper.GetSubPathInResources(path);
            //去掉文件扩展名
            pathInResoruces = PathHelper.RemoveExtension(pathInResoruces);
            UnityEngine.Object[] assets;
            if (hasSubAsset)
            {
                assets = Resources.LoadAll(pathInResoruces);
            }
            else
            {
                assets = new UnityEngine.Object[1];
                assets[0] = Resources.Load(pathInResoruces);
            }
            yield return null;
            if (assets != null && assets[0] != null)
            {
                Debug.Log("Load Asset Success by ResourceLoad AssetPath:" + path);
            }
            onEnd(path, assets);
        }

        public IEnumerator LoadAsset<T>(string path, Action<string, T> onEnd) where T : UnityEngine.Object
        {
            UnityEngine.Object[] assets = null;
            bool hasSubAsset = false;
            string mainAssetPath, subAssetPath = "";
            int atIndex = path.IndexOf("@");
            if (atIndex != -1)
            {
                mainAssetPath = path.Substring(0, atIndex);
                subAssetPath = path.Substring(atIndex + 1);
                hasSubAsset = true;
            }
            else
            {
                mainAssetPath = path;
            }
            //1.尝试从缓存中获取
            /*
            if(GetAssetFromCache(path, out obj))
            {
                onEnd(path, obj as T);
                yield break;
            }
           
            //2.从assetBundle中获取
            if(obj == null)
            {
                yield return LoadAssetFromBundle<T>(path, (p, o) => { obj = o; });
            }
             */
            //3.使用assetDataBase加载
            if (Application.isEditor && assets == null)
            {
                LoadAssetByAssetDatabase(mainAssetPath, hasSubAsset, (loadPath, loadAssets) => { assets = loadAssets; });
            }
            //4.使用resource加载
            if (assets == null)
            {
                yield return LoadAssetByResourceLoad(mainAssetPath, hasSubAsset, (loadPath, loadAssets) => { assets = loadAssets; });
            }
            LoadEnd:
            UnityEngine.Object asset = null;
            if (hasSubAsset)
            {
                if(assets[0].GetType() == typeof(Texture2D) || assets[0].GetType() == typeof(Sprite))
                {
                    foreach (var obj in assets)
                    {
                        if (obj.name == subAssetPath && obj.GetType() == typeof(Sprite))
                        {
                            asset = obj;
                            break;
                        }
                    }
                }
                
            }
            else
            {
                asset = assets[0];
            }
            onEnd(path, asset as T);
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

