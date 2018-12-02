using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var uiMgr = Mugen3D.UIManager.Instance;
        uiMgr.Add("test", this.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
