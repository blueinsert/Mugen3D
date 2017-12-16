using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSoul.UI;
public class Main : MonoBehaviour {
    public Canvas canvas;

    public void Awake()
    {
        LuaMgr.Instance.Env.DoString(string.Format("return require('{0}')", "main"));
        var t = UIManager.Instance.AddView("UIRoot", canvas.transform);
        UIManager.Instance.AddUIGroup("bg", t.Find("BGGroup"));
        UIManager.Instance.AddUIGroup("base", t.Find("BaseGroup"));
        UIManager.Instance.AddUIGroup("popup", t.Find("PopupGroup"));
        UIManager.Instance.AddView("PageMenu", UIManager.Instance.GetUIGroup("base"));

        SpriteLoader.Instance.GetSprite("iceBigHead");
    }
}
