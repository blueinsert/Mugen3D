using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Net;

public class PvpTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        /*
        serverNetMgr = Mugen3D.Net.Server.ServerNetMgr.Instance;
        serverNetMgr.Start("127.0.0.1", 1234);
        clientNetMgr = Mugen3D.Net.ClientNetMgr.Instance;
        clientNetMgr.Connect("127.0.0.1", 1234);
        Mugen3D.Net.Protocol.ProtocolBytes proto = new Mugen3D.Net.Protocol.ProtocolBytes();
        proto.AddString("hello");
        clientNetMgr.conn.Send(proto);
         */
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI() {
        if (GUILayout.Button("CreateRoom"))
        {
            Mugen3D.Net.Server.ServerNetMgr.Instance.Start("127.0.0.1", 1234);
            Mugen3D.Net.ClientNetMgr.Instance.Connect("127.0.0.1", 1234);
            Mugen3D.Net.Protocol.ProtocolBytes proto = new Mugen3D.Net.Protocol.ProtocolBytes();
            proto.AddString("CreateRoom");
            Mugen3D.Net.ClientNetMgr.Instance.conn.Send(proto);
            Mugen3D.Net.ClientNetMgr.Instance.conn.msgDist.AddListener("GameStart", (res) => {
                Debug.Log("GameStart");
                GUIDebug.Instance.AddMsg("msg", "GameStart");
            });
        }
        if (GUILayout.Button("JoinRoom"))
        {
            Mugen3D.Net.ClientNetMgr.Instance.Connect("127.0.0.1", 1234);
            Mugen3D.Net.Protocol.ProtocolBytes proto = new Mugen3D.Net.Protocol.ProtocolBytes();
            proto.AddString("JoinRoom");
            Mugen3D.Net.ClientNetMgr.Instance.conn.Send(proto);
            Mugen3D.Net.ClientNetMgr.Instance.conn.msgDist.AddListener("GameStart", (res) =>
            {
                Debug.Log("GameStart");
                GUIDebug.Instance.AddMsg("msg", "GameStart");
            });
        }
    }
}
