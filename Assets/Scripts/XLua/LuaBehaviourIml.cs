using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaBehaviourIml : LuaMonoBehaviour {
    public string luaFile;

    void Awake()
    {
        var module = LuaMgr.Instance.Env.DoString(string.Format("return require('{0}')", luaFile))[0] as XLua.LuaTable;
        SetLuaModule(module);
    }

}
