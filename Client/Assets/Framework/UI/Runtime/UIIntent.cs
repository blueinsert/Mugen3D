using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework.UI
{
    public class UIIntent
    {
        //public string Mode { get; set; }
        public string Name { get { return m_name; } }
        public UIIntent PrevIntent { get { return m_prevIntent; } }
        private string m_name;
        private UIIntent m_prevIntent;
        private readonly Dictionary<string, object> m_customParamDic = new Dictionary<string, object>();

        public UIIntent(string name, UIIntent prevIntent = null)
        {
            m_name = name;
            m_prevIntent = prevIntent;
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

        public object GetCustomParam(string key)
        {
            if (m_customParamDic.ContainsKey(key))
            {
                return m_customParamDic[key];
            }
            return null;
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
