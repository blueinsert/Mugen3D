using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Fsoul.Net {
public class Msg2 : Msg {
    public int id = 1;
    public int id2 = 2;
    public string content1 = "hehe";

    public Msg2()
    {
        type = "msg2";
    }

    public override byte[] Encode() {
        string c = JsonMapper.ToJson(this);
        NetBufferOut stream = new NetBufferOut();
        stream.WriteString(c);
        return stream.GetBuffer();
    }

    public override void Decode(byte[] data)
    {
    }
}
}
