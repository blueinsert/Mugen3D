using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework
{
    public class AssetProvider : IAssetProvider
    {
        private Dictionary<string, UnityEngine.Object> m_assetDic;

        public AssetProvider(Dictionary<string, UnityEngine.Object> assetDic)
        {
            m_assetDic = assetDic;
        }

        public T GetAsset<T>(string path) where T : UnityEngine.Object
        {
            return AssetUtility.GetAsset<T>(m_assetDic, path);
        }

    }
}
