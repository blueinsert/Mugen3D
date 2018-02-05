using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSoul.UI;
public class Main : MonoBehaviour {

    public void Start()
    {
        LuaMgr.Instance.Env.DoString(string.Format("return require('{0}')", "main"));
        SceneManager.Instance.LoadMainMenu();
        GameObject.Destroy(this.gameObject);
    }
}
