using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Net.Protocol
{

    public class FrameData : ProtocolBytes
    {
        public int roomId {get; private set;}
        public int frameNo { get; private set; }
        public int input { get; private set; }

        public FrameData(int roomId, int frameNo, int input)
        {
            this.roomId = roomId;
            this.frameNo = frameNo;
            this.input = input;
        }

        public FrameData(byte[] data)
        {
            this.bytes = data;
            int start = 0;
            string protoName = GetString(start, ref start);
            this.roomId = GetInt(start, ref start);
            this.frameNo = GetInt(start, ref start);
            this.input = GetInt(start, ref start);
        }

        //编码器
        public override byte[] Encode()
        {
            bytes = null;
            AddString("FrameData");
            AddInt(roomId);
            AddInt(frameNo);
            AddInt(input);
            return bytes;
        }

    }
}
