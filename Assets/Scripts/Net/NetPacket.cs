// Copyright (C) 2017 Fsoul Inc.

using System;

namespace Fsoul.Net {

public class NetPacket {

    private int length;

    public byte[] Body { get; set; }

    public NetPacket() {
        length = -1;
        Body = null;
    }

    public NetPacket(Msg msg) { 
        this.Body = msg.Encode();
    }

    public int Decode(byte[] data, int offset, int size) {
        int n = 0;

        if (length == -1) {
            if (size < 2) return n;
            length = (int)Bits.GetUInt16(data, offset);
            n += 2;
            size -= 2;
        }

        if (Body == null) {
            if (size < length) return n;
            Body = new byte[length];
            Buffer.BlockCopy(data, offset + n, Body, 0, length);
            n += length;
            size -= length;
        }

        return n;
    }

    public byte[] Encode() {
        length = Body.Length;
        var data = new byte[length + 2];
        Bits.PutUInt16((UInt16)length, data, 0);
        Buffer.BlockCopy(Body, 0, data, 2, Body.Length);
        return data;
    }

    public bool IsComplete() {
        return Body != null;
    }

}

}