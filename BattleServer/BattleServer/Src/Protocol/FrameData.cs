using System.Collections;
using System.Collections.Generic;

namespace BattleServer.Protocol
{

    public class FrameData : ProtocolBytes
    {
        public int frameNo { get; private set; }
        public int input { get; private set; }

        public FrameData(int frameNo, int input)
        {
            this.frameNo = frameNo;
            this.input = input;
        }

        public FrameData(byte[] data)
        {
            this.bytes = data;
            int start = 0;
            string protoName = GetString(start, ref start);
            this.frameNo = GetInt(start, ref start);
            this.input = GetInt(start, ref start);
        }

        public override byte[] Encode()
        {
            bytes = null;
            AddString("FrameData");
            AddInt(frameNo);
            AddInt(input);
            return bytes;
        }

    }
}
