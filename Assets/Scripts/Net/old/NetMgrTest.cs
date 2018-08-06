using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fsoul.Net {

public class NetMgrTest : MonoBehaviour {
  
	void Start () {
	    NetMgr.Instance.Connect("127.0.0.1",1024,12);
       
        NetMgr.Instance.SendPackage<TestReq,TestRes>(new TestReq(), (res)=>{
            Debug.Log("Receive res: res.sessionId:"+res.SessionId);
        });
	}
	
	void Update () {
     
	}

}
}
