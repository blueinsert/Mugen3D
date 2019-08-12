using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace bluebean.UGFramework
{
    public class AssetUtility
    {
        public const string RuntimeAssetsPath = "Assets/GameProject/RuntimeAssets/";

        public static string GetSpritePath(string path)
        {
            if (path.Contains("@"))
            {
                return path;
            }
            return string.Format("{0}{1}@{2}", RuntimeAssetsPath, path, Path.GetFileNameWithoutExtension(path));
        }

        public static T GetAsset<T>(Dictionary<string, UnityEngine.Object> assetDic, string path) where T : UnityEngine.Object
        {
            string realPath = string.Format("{0}{1}", RuntimeAssetsPath, path);
            UnityEngine.Object asset;
            if (assetDic.TryGetValue(realPath, out asset))
            {
                return asset as T;
            }
            return null;
        }

        public static Sprite GetSprite(Dictionary<string, UnityEngine.Object> assetDic, string path)
        {
            return GetAsset<Sprite>(assetDic, AssetUtility.GetSpritePath(path));
        }
    }
}
