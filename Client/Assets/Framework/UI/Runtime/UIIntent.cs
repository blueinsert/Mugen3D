using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework.UI
{
    public class UIIntent
    {
        public string Name { get { return m_name; } }
        public string Mode { get { return m_mode; } }
        public UIIntent PrevIntent { get { return m_prevIntent; } }

        private string m_name;
        private string m_mode;
        private UIIntent m_prevIntent;
        private readonly Dictionary<string, object> m_customParamDic = new Dictionary<string, object>();

        public UIIntent(string name, UIIntent prevIntent = null, string mode = "")
        {
            m_name = name;
            m_prevIntent = prevIntent;
            m_mode = mode;
        }

        public void SetCustomParam(string key, object value)
        {
            if (m_customParamDic.ContainsKey(key))
            {
                m_customParamDic[key] = value;
            }
            else
            {
                m_customParamDic.Add(key, value);
            }
        }

        public T GetCustomClassParam<T>(string key) where T:class
        {
            if (m_customParamDic.ContainsKey(key))
            {
                return m_customParamDic[key] as T;
            }
            return null;
        }

        public T GetCustomStructParam<T>(string key) where T : struct
        {
            if (m_customParamDic.ContainsKey(key))
            {
                return (T)m_customParamDic[key];
            }
            return default(T);
        }

        public void ClearCustomParam(string key)
        {
            if (m_customParamDic.ContainsKey(key))
            {
                m_customParamDic.Remove(key);
            }
        }

        public void ClearAllCustomParam()
        {
            m_customParamDic.Clear();
        }
    }
}
