
using System;
using System.Text;

namespace Fsoul.Net {

public class NetBufferIn {

    private byte[] m_buffer;
    private int m_offset;

    public NetBufferIn(byte[] buffer, int offset) {
        m_buffer = buffer;
        m_offset = offset;
    }

    public byte ReadUInt8() {
        var n = m_buffer[m_offset];
        m_offset += 1;
        return n;
    }

    public UInt32 ReadUInt16() {
        var n = Bits.GetUInt16(m_buffer, m_offset);
        m_offset += 2;
        return n;
    }    

    public UInt32 ReadUInt32() {
        var n = Bits.GetUInt32(m_buffer, m_offset);
        m_offset += 4;
        return n;
    }

    public UInt64 ReadUInt64() {
        var n = Bits.GetUInt64(m_buffer, m_offset);
        m_offset += 8;
        return n;
    }

    public string ReadString() {
        var length = ReadUInt32();
        var bytes = new byte[length];
        Buffer.BlockCopy(m_buffer, m_offset, bytes, 0, bytes.Length);
        m_offset += (int)length;
        return Encoding.UTF8.GetString(bytes);
    }

}

}