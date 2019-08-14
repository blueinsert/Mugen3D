using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.UGFramework
{
    public class AssetUtility
    {
        public const string RuntimeAssetsPath = "Assets/GameProject/RuntimeAssets/";

        private static T _GetAsset<T>(Dictionary<string, UnityEngine.Object> assetDic, string path) where T : UnityEngine.Object
        {
            UnityEngine.Object asset;
            T res = null;
            if (assetDic.TryGetValue(path, out asset))
            {
                res = asset as T;
            }
            if(res == null)
            {
                Debug.LogError(string.Format("AssetUtility:_GetAsset asset is null, path:{0}", path));
            }
            return res;
        }

        public static string MakeSpritePath(string path)
        {
            if (path.Contains("@"))
            {
                return path;
            }
            return string.Format("{0}{1}@{2}", RuntimeAssetsPath, path, Path.GetFileNameWithoutExtension(path));
        }

        public static Sprite GetSprite(Dictionary<string, UnityEngine.Object> assetDic, string path)
        {
            return _GetAsset<Sprite>(assetDic, MakeSpritePath(path));
        }

        public static string MakeAssetPath(string path)
        {
            string realPath = string.Format("{0}{1}", RuntimeAssetsPath, path);
            return realPath;
        }

        public static T GetAsset<T>(Dictionary<string, UnityEngine.Object> assetDic, string path) where T : UnityEngine.Object
        {
            string realPath = string.Format("{0}{1}", RuntimeAssetsPath, path);
            return _GetAsset<T>(assetDic, realPath);
        }

    }
}
