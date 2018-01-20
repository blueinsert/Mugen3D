using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSoul.UI;
public class Main : MonoBehaviour {
    public Canvas canvas;

    public void Awake()
    {
        LuaMgr.Instance.Env.DoString(string.Format("return require('{0}')", "main"));
        UIManager.Instance.PushView("ViewMainMenu", GameObject.Find("UIRoot/Canvas/BaseGroup").transform);
    }
}
