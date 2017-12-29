using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fsoul.Net {
public class NetMgr : MonoBehaviour {
    private static NetMgr mInstance;
    public static NetMgr Instance {
        get {
            if(mInstance == null) {
                GameObject go = new GameObject("__NetMgr");
                mInstance = go.AddComponent<NetMgr>();
                mInstance.mConn = new TCPConnection();
            }
            return mInstance;
        }
    }

    private TCPConnection mConn;

    public void Connect(string host, int port, double timeout) {
        mConn.Connect(host, port, timeout);
    }

    public void SendPackage(NetPacket p) {
        mConn.SendPacket(p);
    }

}
}
