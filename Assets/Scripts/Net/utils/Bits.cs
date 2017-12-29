// Copyright (C) 2017 Fsoul Inc.

using System;

namespace Fsoul.Net {

public class Bits {
    public static UInt16 GetUInt16(byte[] buf, int offset) {
        return (UInt16)((UInt16)buf[offset+0] | 
                        (UInt16)buf[offset+1] << 8);
    }

    public static UInt32 GetUInt32(byte[] buf, int offset) {
        return (UInt32)buf[offset+0] | 
               (UInt32)buf[offset+1] << 8 | 
               (UInt32)buf[offset+2] << 16 | 
               (UInt32)buf[offset+3] << 24;
    }

    public static UInt64 GetUInt64(byte[] buf, int offset) {
        return (UInt64)buf[offset+0] | 
               (UInt64)buf[offset+1] << 8 | 
               (UInt64)buf[offset+2] << 16 | 
               (UInt64)buf[offset+3] << 24 | 
               (UInt64)buf[offset+4] << 32 | 
               (UInt64)buf[offset+5] << 40 | 
               (UInt64)buf[offset+6] << 48 |
               (UInt64)buf[offset+7] << 56;
    }

    public static void PutUInt16(UInt16 n, byte[] buf, int offset) {
        buf[offset+0] = (byte)(n);
        buf[offset+1] = (byte)(n >> 8);
    }

    public static void PutUInt32(UInt32 n, byte[] buf, int offset) {
        buf[offset+0] = (byte)(n);
        buf[offset+1] = (byte)(n >> 8);
        buf[offset+2] = (byte)(n >> 16);
        buf[offset+3] = (byte)(n >> 24);
    }

    public static void PutUInt64(UInt64 n, byte[] buf, int offset) {
        buf[offset+0] = (byte)(n);
        buf[offset+1] = (byte)(n >> 8);
        buf[offset+2] = (byte)(n >> 16);
        buf[offset+3] = (byte)(n >> 24);
        buf[offset+4] = (byte)(n >> 32);
        buf[offset+5] = (byte)(n >> 40);
        buf[offset+6] = (byte)(n >> 48);
        buf[offset+7] = (byte)(n >> 56);
    }
}

}