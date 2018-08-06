using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LitJson;

namespace Fsoul.Net {
public class NetMgr : MonoBehaviour {
    private static NetMgr mInstance;
    public static NetMgr Instance {
        get {
            if(mInstance == null) {
                GameObject go = new GameObject("__NetMgr");
                mInstance = go.AddComponent<NetMgr>();
                mInstance.mConn = new TCPConnection();
                PacketListener packetListener = mInstance.HandlePacket;
                mInstance.mConn.AddPacketListener(packetListener);
            }
            return mInstance;
        }
    }

    private int mSessionId = 0;
    private TCPConnection mConn;
    private Dictionary<int, Action<string>> cbs = new Dictionary<int,Action<string>>();

    public void Connect(string host, int port, double timeout) {
        mConn.Connect(host, port, timeout);
    }

    public void SendPackage<ReqType, ResType>(ReqType req, Action<ResType> cb) 
        where ReqType:Msg
        where ResType:Msg {
            var request = req as ReqType;
            int sessionId = mSessionId++;
            request.SessionId = sessionId;
            NetPacket pkg = new NetPacket(request);
            mConn.SendPacket(pkg);
            cbs[sessionId] = (jsonStr) => {
                Debug.Log("res jsonStr:" + jsonStr);
                ResType res = JsonMapper.ToObject<ResType>(jsonStr);
                cb(res);
            };
    }

    private void HandlePacket(NetPacket pkg)
    {
        string jsonStr = Encoding.UTF8.GetString(pkg.Body);
        Msg res = JsonMapper.ToObject<Msg>(jsonStr);
        int sessionId = res.SessionId;
        if (cbs.ContainsKey(sessionId))
        {
            cbs[sessionId](jsonStr);
            cbs.Remove(sessionId);
        }
    }

    public void Update()
    {
        if (mConn != null)
        {
            mConn.Poll();
        }
    }

}
}
