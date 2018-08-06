// Copyright (C) 2017 Fsoul Inc.

using System;

namespace Fsoul.Net {

public class DH {
    public static readonly UInt64 P = 0xffffffffffffffc5;
    public static readonly UInt64 G = 5;

    public static UInt64 PowModP(UInt64 a, UInt64 b) {
        if(a > P) {
            a %= P;
        }
        return _PowModP(a, b);
    }

    private static UInt64 _PowModP(UInt64 a, UInt64 b) {
        if(b == 1) {
            return a;
        }
        UInt64 t = _PowModP(a, b>>1);
        t = _MulModP(t, t);
        if((b%2) != 0) {
            t = _MulModP(t, a);
        }
        return t;
    }

    private static UInt64 _MulModP(UInt64 a, UInt64 b) {
        UInt64 m = 0;
        while(b != 0) {
            if((b&1) != 0) {
                UInt64 t = P - a;
                if(m >= t) {
                    m -= t;
                } else {
                    m += a;
                }
            }
            if(a >= (P-a)) {
                a = a*2 - P;
            } else {
                a = a*2;
            }
            b >>= 1;
        }
        return m;
    }
}

}