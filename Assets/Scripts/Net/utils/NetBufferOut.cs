
using System;
using System.Text;
using System.IO;

namespace Fsoul.Net {

public class NetBufferOut {

    private MemoryStream m_stream;

    public NetBufferOut() {
        m_stream = new MemoryStream();
    }

    public byte[] GetBuffer() {
        return m_stream.ToArray();
    }

    public void WriteUInt8(byte value) {
        m_stream.WriteByte(value);
    }

    public void WriteUInt16(UInt16 value) {
        var bytes = new byte[2];
        Bits.PutUInt16(value, bytes, 0);
        m_stream.Write(bytes, 0, bytes.Length);
    }    

    public void WriteUInt32(UInt32 value) {
        var bytes = new byte[4];
        Bits.PutUInt32(value, bytes, 0);
        m_stream.Write(bytes, 0, bytes.Length);
    }

    public void WriteUInt64(UInt64 value) {
        var bytes = new byte[8];
        Bits.PutUInt64(value, bytes, 0);
        m_stream.Write(bytes, 0, bytes.Length);
    }

    public void WriteString(string value) {
        var bytes = Encoding.UTF8.GetBytes(value);
        //WriteUInt32((UInt32)bytes.Length);
        m_stream.Write(bytes, 0, bytes.Length);
    }
}

}