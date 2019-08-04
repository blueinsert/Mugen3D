using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace bluebean.UGFramework.ConfigData
{
    public class ConfigDataColumnTypeAttribute : Attribute
    {
        public Type m_columnType;
        public string m_typeName;

        public ConfigDataColumnTypeAttribute(Type columnType, string typeName)
        {
            m_columnType = columnType;
            m_typeName = typeName;
        }
    }

    public class ConfigDataValueConverter
    {

        #region value parser funcs

        [ConfigDataColumnType(typeof(int), "int")]
        public static int ParseInt(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return 0;
            }
            return Int32.Parse(s);
        }

        [ConfigDataColumnType(typeof(float), "float")]
        public static float ParseFloat(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return 0f;
            }
            return float.Parse(s);
        }

        [ConfigDataColumnType(typeof(string), "string")]
        public static string ParseString(string s)
        {
            return s;
        }

        [ConfigDataColumnType(typeof(bool), "bool")]
        public static bool ParseBool(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return false;
            }
            return bool.Parse(s);
        }

        #endregion

        /// <summary>
        /// 获取原生数据类型的转换方法
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        private static MethodInfo GetConvertMethod(string typeStr)
        {
            Type type = typeof(ConfigDataValueConverter);
            var methodInfos = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var methodInfo in methodInfos)
            {
                var configDataColumnTypeAttributes = methodInfo.GetCustomAttributes(typeof(ConfigDataColumnTypeAttribute), true) as ConfigDataColumnTypeAttribute[];
                foreach (var configDataColumnTypeAttribute in configDataColumnTypeAttributes)
                {
                    if (configDataColumnTypeAttribute.m_typeName == typeStr)
                    {
                        return methodInfo;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取实际数据
        /// </summary>
        /// <param name="valueStr"></param>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static System.Object GetValue(string valueStr, string typeStr)
        {
            var methodInfo = GetConvertMethod(typeStr);
            if (methodInfo != null)
            {
                return methodInfo.Invoke(null, new object[] { valueStr });
            }
            else
            {
                //可能是数据表配置的枚举类型
                var enumType = ClassLoader.GetType(ConfigDataSetting.Instance.m_codeNameSpace + "." + typeStr);
                if (enumType != null)
                {
                    return Enum.Parse(enumType, valueStr);
                }
                else
                {
                    throw new Exception(string.Format("Can't get enum type by '{0}'", typeStr));
                }
            }
        }

    }
}
