using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Net.Protocol
{

    public class MatchInfo : ProtocolBytes
    {
        public int roomId { get; private set; }

        public MatchInfo(int roomId)
        {
            this.roomId = roomId;
        }

        public MatchInfo(byte[] data)
        {
            this.bytes = data;
            int start = 0;
            string protoName = GetString(start, ref start);
            this.roomId = GetInt(start, ref start);
        }

        //编码器
        public override byte[] Encode()
        {
            bytes = null;
            AddString("MatchInfo");
            AddInt(roomId);
            return bytes;
        }
    }
}
