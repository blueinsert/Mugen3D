using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class EntityLoader
    {
        public static Unit curUnit;

        public static Player LoadPlayer(PlayerId id, string playerName, Transform parent)
        {
            UnityEngine.Object prefab = Resources.Load<UnityEngine.Object>("Chars/" + playerName + "/" + playerName);
            GameObject go = GameObject.Instantiate(prefab, parent) as GameObject;
            Player p = go.GetComponentInChildren<Player>();
            p.Init();
            p.id = id;
            curUnit = p;
            XLua.LuaTable fsm = LuaMgr.Instance.Env.DoString(string.Format("return (require('{0}')).new(CS.Mugen3D.EntityLoader.curUnit)", "FSM/Chars/" + playerName + "/" + playerName))[0] as XLua.LuaTable;
            p.SetFSM(fsm);
            curUnit = null;
            World.Instance.AddEntity(p);
            return p;
        }

        public static Helper LoadHelper(string helperName, Player master, Transform parent)
        {
            UnityEngine.Object prefab = Resources.Load<UnityEngine.Object>("Helpers/" + helperName + "/" + helperName);
            GameObject go = GameObject.Instantiate(prefab, parent) as GameObject;
            Helper helper = go.GetComponentInChildren<Helper>();
            helper.master = master;
            helper.Init();
            World.Instance.AddEntity(helper);
            return helper;
        }
    }
}

