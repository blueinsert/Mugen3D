using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvpTest : MonoBehaviour {

    Mugen3D.Net.ServerNetMgr serverNetMgr;
    Mugen3D.Net.ClientNetMgr clientNetMgr;

	// Use this for initialization
	void Start () {
        serverNetMgr = Mugen3D.Net.ServerNetMgr.Instance;
        serverNetMgr.Start("127.0.0.1", 1234);
        clientNetMgr = Mugen3D.Net.ClientNetMgr.Instance;
        clientNetMgr.Connect("127.0.0.1", 1234);
        Mugen3D.Net.Protocol.ProtocolBytes proto = new Mugen3D.Net.Protocol.ProtocolBytes();
        proto.AddString("hello");
        clientNetMgr.conn.Send(proto);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
