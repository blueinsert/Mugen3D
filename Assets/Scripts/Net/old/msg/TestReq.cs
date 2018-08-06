using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Fsoul.Net
{
    public class TestReq : Msg
    {
        public TestReq() {
            this.Type = "TestReq";
        }

        public override byte[] Encode()
        {
            string c = JsonMapper.ToJson(this);
            NetBufferOut stream = new NetBufferOut();
            stream.WriteString(c);
            return stream.GetBuffer();
        }
    }
}
