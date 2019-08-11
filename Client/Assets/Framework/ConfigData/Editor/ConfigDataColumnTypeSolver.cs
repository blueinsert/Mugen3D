using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using FixPointMath;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.UGFramework.ConfigData
{
    public class ColumnDataTypeAttribute : Attribute
    {
        public Type m_columnType;
        public string m_typeName;

        public ColumnDataTypeAttribute(Type columnType, string typeName)
        {
            m_columnType = columnType;
            m_typeName = typeName;
        }
    }

    public class ConfigDataColumnTypeSolver
    {

        #region value parser funcs

        [ColumnDataType(typeof(int), "int")]
        public static int ParseInt(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return 0;
            }
            return Int32.Parse(s);
        }

        [ColumnDataType(typeof(float), "float")]
        public static float ParseFloat(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return 0f;
            }
            return float.Parse(s);
        }

        [ColumnDataType(typeof(string), "string")]
        public static string ParseString(string s)
        {
            return s;
        }

        [ColumnDataType(typeof(bool), "bool")]
        public static bool ParseBool(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return false;
            }
            return bool.Parse(s);
        }

        [ColumnDataType(typeof(Number), "number")]
        public static Number ParseNumber(string s)
        {
            float v;
            Number number = new Number(0);
            if (float.TryParse(s,out v))
            {
                number = new Number((int)(v * 10000)) / new Number(10000);
            }
            else
            {
                Debug.LogError(string.Format("ParseNumber error while parse {0}", s));
            }
            return number;
        }

        [ColumnDataType(typeof(Vector), "vector")]
        public static Vector ParseVector2(string s)
        {
            if(s.StartsWith("(") && s.EndsWith(")"))
            {
                string subStr = s.Substring(1, s.Length - 2);
                string[] splits = subStr.Split(new char[] { ',' });
                if (splits.Length != 2)
                {
                    Debug.LogError(string.Format("ParseVector2 error splits.Length != 2 while parse {0}", s));
                    return new Vector();
                }
                Number number1 = ParseNumber(splits[0]);
                Number number2 = ParseNumber(splits[1]);
                return new Vector(number1, number2);
            }
            else
            {
                Debug.LogError(string.Format("ParseVector2 error while parse {0}", s));
            }
            return new Vector();
        }

        #endregion

        /// <summary>
        /// 获取解析支持的对应类型
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static Type GetType(string typeStr)
        {
            typeStr = typeStr.ToLower();
            Type type = typeof(ConfigDataColumnTypeSolver);
            var methodInfos = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var methodInfo in methodInfos)
            {
                var configDataColumnTypeAttributes = methodInfo.GetCustomAttributes(typeof(ColumnDataTypeAttribute), true) as ColumnDataTypeAttribute[];
                foreach (var configDataColumnTypeAttribute in configDataColumnTypeAttributes) {
                    if(configDataColumnTypeAttribute.m_typeName == typeStr)
                    {
                        return configDataColumnTypeAttribute.m_columnType;
                    }
                }     
            }
            return null;
        }

        /// <summary>
        /// 获取解析支持类型的转换方法
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        private static MethodInfo GetConvertMethod(string typeStr)
        {
            Type type = typeof(ConfigDataColumnTypeSolver);
            var methodInfos = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var methodInfo in methodInfos)
            {
                var configDataColumnTypeAttributes = methodInfo.GetCustomAttributes(typeof(ColumnDataTypeAttribute), true) as ColumnDataTypeAttribute[];
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
            return null;
        }

    }
}
