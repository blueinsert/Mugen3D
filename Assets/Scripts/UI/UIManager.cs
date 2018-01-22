using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSoul.UI
{

public class ViewDef {
    public string name;
    public string prefab;
    public string script;
    public ViewDef(string name, string prefab, string script)
    {
        this.name = name;
        this.prefab = prefab;
        this.script = script;
    }
}

    public class UIManager
    {
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


        private Dictionary<string, ViewDef> uidef = new Dictionary<string, ViewDef>();

        public void Init(XLua.LuaTable uidef) {
            uidef.Get<XLua.LuaTable>("Views").ForEach((string k, XLua.LuaTable v)=>{
                string name = v.Get<string>("name");
                string prefab = v.Get<string>("prefab");
                string script = v.Get<string>("script");
                ViewDef viewdef = new ViewDef(name, prefab, script);
                this.uidef.Add(name, viewdef);
            });
        }

        private List<LuaView> m_views = new List<LuaView>();
        private LuaView m_curView;

        private LuaView AddLuaView(GameObject go, string script)
        {
            var luaView = go.AddComponent<LuaView>();
            var module = LuaMgr.Instance.Env.DoString(string.Format("return require('{0}')", script))[0] as XLua.LuaTable;
            luaView.SetLuaModule(module);
            luaView.Init();
            return luaView;
        }

        public LuaView AddView(string viewName, Transform parent)
        {
            if (!uidef.ContainsKey(viewName))
            {
                Debug.LogError("uidef don't contain " + viewName);
            }
            var viewdef = uidef[viewName];
            UnityEngine.Object prefab = Resources.Load<UnityEngine.Object>(viewdef.prefab);
            GameObject go = GameObject.Instantiate(prefab, parent) as GameObject;
            LuaView luaView = AddLuaView(go, viewdef.script);
            luaView.viewName = viewdef.name;
            luaView.isInStack = false;
            return luaView;
        }

        public LuaView PushView(string viewName, Transform parent)
        {
            var view = AddView(viewName, parent);
            if(m_views.Count!=0){
                foreach(var v in m_views){
                    if(v.interactable){
                        v.interactable = false;
                    }
                }
            }
            m_views.Add(view);
            view.interactable = true; 
            view.isInStack = true;
            m_curView = view;
            return view;
        }

        public void PopView(LuaView view)
        {
            if (view.isInStack)
            {
                m_views.Remove(view);
                Debug.Log("view be poped:" + view.viewName);
                if (m_views.Count != 0)
                {
                    m_views[m_views.Count - 1].interactable = true;
                }
            }
        }

    }
}
