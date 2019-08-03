using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework.ConfigData
{
    public partial class ConfigDataLoader
    {
        private ConfigDataLoader() { }
        private static ConfigDataLoader m_instance;
        public static ConfigDataLoader Instance
        {
            get
            {
                m_instance = new ConfigDataLoader();
                return m_instance;
            }
        }

        public static ConfigDataLoader CreateInstance()
        {
            m_instance = new ConfigDataLoader();
            return m_instance;
        }

        private readonly HashSet<string> m_allConfigTableNames = new HashSet<string>();

        private readonly Dictionary<string, Action<AssetObject>> m_deserializeFuncDics = new Dictionary<string, Action<AssetObject>>();

        partial void InitAllConfigDataTableName();

        partial void InitAllDeserializeFuncs();

         
        public IEnumerator LoadAllConfigData(Action<bool> onEnd = null)
        {
            InitAllConfigDataTableName();
            InitAllDeserializeFuncs();
            foreach(var tableName in m_allConfigTableNames)
            {
                string assetPath = "Assets/GameProject/RuntimeAssets/ConfigData_ABS/ConfigData" + tableName + ".asset";
                yield return AssetLoader.Instance.LoadAsset<AssetObject>(assetPath, (p, scriptableObj) => {
                    if (scriptableObj != null)
                    {
                        var deserializer = m_deserializeFuncDics["ConfigData" + tableName];
                        if (deserializer != null)
                        {
                            deserializer(scriptableObj);
                        }else
                        {
                            Debug.Log(string.Format("table {0}'s deserializeFunc is null:", assetPath));
                            onEnd(false);
                        }
                    }
                    else
                    {
                        Debug.Log(string.Format("table {0}'s scriptableObj is null:", assetPath));
                        onEnd(false);
                    }
                });
            }
            onEnd(true);
        }
    }
}
