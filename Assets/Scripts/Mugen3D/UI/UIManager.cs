using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;

namespace Mugen3D
{
    public class UIDef
    {
        public string name { get; set; }
        public string script { get; set; }
        public string prefab { get; set; }
    }

    public class UIManager : Singleton<UIManager>
    {
        private int m_currentId = 0;
        private Dictionary<string, UIDef> m_uiDefs = new Dictionary<string, UIDef>();
        private Dictionary<string, Dictionary<int, UIView>> m_views = new Dictionary<string, Dictionary<int, UIView>>();

        public void Awake()
        {
            m_uiDefs = Core.ConfigReader.Parse<Dictionary<string, UIDef>>(ResourceLoader.LoadText("UIDef"));
            /*
            //Add Test
            m_uiDefs.Add("test", new UIDef() { name = "test", script = "hehe", prefab = "1/2/2" });
            YamlDotNet.Serialization.Serializer serializer = new Serializer();
            StringWriter strWriter = new StringWriter();
            serializer.Serialize(strWriter, m_uiDefs);
            using (TextWriter writer = File.CreateText("Assets/Resources/UIDef.txt"))
            {
                writer.Write(strWriter.ToString());
            }
            */
        }

        private void OnViewDestroy(UIView view)
        {
            m_views[view.name].Remove(view.id);
            GameObject.Destroy(view.gameObject);
        }

        public UIView Add(string name, Transform parent)
        {
            if (!m_uiDefs.ContainsKey(name))
            {
                Debug.LogError("uidefs do't contain " + name);
            }
            UIDef def = m_uiDefs[name];
            Type t = Type.GetType(def.script);
            if (!t.IsSubclassOf(typeof(UIView))) {
                Debug.LogError("script is't inherit from UIView");
            }
            var prefab = ResourceLoader.Load<GameObject>(def.prefab);
            var go = GameObject.Instantiate(prefab, parent);
            go.gameObject.name = name; 
            var view = (UIView)go.AddComponent(t);
            view.Init(m_currentId++, name);
            if (!m_views.ContainsKey(name))
            {
                m_views.Add(name, new Dictionary<int, UIView>());
            }
            m_views[name].Add(view.id, view);
            go.gameObject.name = go.gameObject.name + "_" + m_views[name].Count;
            view.onDestroy += OnViewDestroy;
            return view;
        }

    }
}
