using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CSharpCallLuaConfig {
    [XLua.CSharpCallLua]
    public static List<System.Type> TYPES = new List<System.Type> {
        typeof(LuaMonoBehaviour.MonoBehaviourEvent),
        typeof(LuaMonoBehaviour.LuaBehaviourInit),
        typeof(UnityEngine.Events.UnityAction)
    };
}
