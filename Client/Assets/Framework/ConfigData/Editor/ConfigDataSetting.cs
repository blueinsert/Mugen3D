using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace bluebean.UGFramework.ConfigData
{
    public enum ConfigDataSerializeType
    {
        Bin,
        Xml,
    }

    [System.Serializable]
    public class ConfigDataSetting : ScriptableObject
    {

        private const string ConfigDataSettingAssetPath = "Assets/Framework/ConfigData/Editor/ConfigDataSettingAsset.asset";

        [Header("配置数据表路径")]
        public string m_configDataPath = "../Design/ConfigData";

        [Header("代码模板路径")]
        public string m_codeTempletePath = "Assets/Framework/ConfigData/Editor/Templete";

        [Header("自动化代码输出路径")]
        public string m_autoGenCodeOutputPath = "Assets/GameProject/Scripts/ConfigData/AutoGen";

        [Header("序列化配置表数据输出路径")]
        public string m_serializedTableDataOutputPath = "Assets/GameProject/RuntimeAssets/ConfigData_ABS";

        [Header("产生代码的命名空间")]
        public string m_codeNameSpace = "bluebean.UGFramework.ConfigData";

        [Header("配置表数据序列化类型")]
        public ConfigDataSerializeType m_configDataSerializeType = ConfigDataSerializeType.Bin;

        private static ConfigDataSetting GetAsset()
        {
            ConfigDataSetting setting = AssetDatabase.LoadAssetAtPath(ConfigDataSettingAssetPath, typeof(ConfigDataSetting)) as ConfigDataSetting;
            return setting;
        }

        private static ConfigDataSetting CreateAsset()
        {
            ConfigDataSetting setting = ScriptableObject.CreateInstance<ConfigDataSetting>();
            AssetDatabase.CreateAsset(setting, ConfigDataSettingAssetPath);
            return setting;
        }

        private static ConfigDataSetting m_instance;
        public static ConfigDataSetting Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = GetAsset();
                    if (m_instance == null)
                    {
                        m_instance = CreateAsset();
                    }
                }
                return m_instance;
            }
        }
    }
}
