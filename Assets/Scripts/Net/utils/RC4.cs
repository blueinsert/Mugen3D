// Copyright (C) 2017 Fsoul Inc.

using System;

namespace Fsoul.Net {

public class RC4 {
    private static byte[] _s = new byte[256];

    public static void Encode(byte[] src, int srcOffset, byte[] dst, int dstOffset, int size, byte[] cipher) {
        for(int i = 0; i < 256; i++) {
            _s[i] = (byte)i;
        }
        for(int i = 0, j = 0; i < 256; i++) {
            j = (byte)((j + _s[i] + cipher[i % cipher.Length]) % 256);
            byte t = _s[i];
            _s[i] = _s[j];
            _s[j] = t;
        }
        int _i = 0;
        int _j = 0;
        for(int i = 0; i < size; i++) {
            _i = (byte)((_i + 1) % 256);
            _j = (byte)((_j + _s[_i]) % 256);
            byte t = _s[_i];
            _s[_i] = _s[_j];
            _s[_j] = t;
            dst[dstOffset+i] = (byte)(src[srcOffset+i] ^ _s[(_s[_i] + _s[_j]) % 256]);
        }  
    }
}

}