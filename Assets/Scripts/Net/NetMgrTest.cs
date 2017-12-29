using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fsoul.Net {

public class NetMgrTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    NetMgr.Instance.Connect("127.0.0.1",1024,12);
        Msg1 msg = new Msg1("hello");
        Msg2 msg2 = new Msg2();
        NetPacket p = new NetPacket(msg2);
        NetMgr.Instance.SendPackage(p);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
}
