using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fsoul.Net {

    public class Msg1 : Msg {

        public string content;

        public Msg1(string c) {
            content = c;
            type = "msg1";
        }

        public override void Decode(byte[] contentBytes)
        {
            NetBufferIn stream = new NetBufferIn(contentBytes, 0);
            content = stream.ReadString();
        }

        public override byte[] Encode()
        {
            NetBufferOut stream = new NetBufferOut();
            stream.WriteString(content);
            return stream.GetBuffer();
        }

    }
}
