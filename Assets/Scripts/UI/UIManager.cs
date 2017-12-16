using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSoul.UI
{
    public class UIManager
    {
        private Dictionary<string, Object> mPrefabsCache = new Dictionary<string, Object>();
        private Dictionary<string, Transform> mUIGroup = new Dictionary<string, Transform>();
        private static UIManager mInstance;
        public static UIManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new UIManager();
                }
                return mInstance;
            }
        }
        private UIManager() { }

        public void AddUIGroup(string name, Transform t)
        {
            mUIGroup[name] = t;
        }

        public Transform GetUIGroup(string name)
        {
            if (mUIGroup.ContainsKey(name))
            {
                return mUIGroup[name];
            }
            return null;
        }

        public Transform AddView(string prefabName, Transform parent)
        {
            UnityEngine.Object o;
            if (mPrefabsCache.ContainsKey(prefabName))
            {
                o = mPrefabsCache[prefabName];
            }
            else
            {
                o = Resources.Load<UnityEngine.Object>("Prefabs/UI/" + prefabName);
                mPrefabsCache[prefabName] = o;
            }
            GameObject go = GameObject.Instantiate(o, parent) as GameObject;
            go.name = prefabName + "_Instance";
            go.transform.parent = parent;
            return go.transform;
        }
    }
}
