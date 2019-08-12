using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework
{
    public interface IAssetProvider
    {
        T GetAsset<T>(string path) where T : UnityEngine.Object;
    }
}
