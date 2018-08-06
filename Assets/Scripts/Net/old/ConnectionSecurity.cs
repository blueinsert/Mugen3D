// Copyright (C) 2015 Fsoul Inc.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Fsoul.Net {

public class ConnectionSecurity {

    private const byte HEADER_PING            = 0x1;
    private const byte HEADER_PONG            = 0x2;
    private const byte HEADER_HANDSHAKE       = 0x3;
    private const byte HEADER_HANDSHAKE_RES   = 0x4;
    private const byte HEADER_RPC             = 0x5;
 
    private string mLastError;
    private Socket mSocket;
    private byte[] mReadBuffer = new byte[64*1024];
    private byte[] mWriteBuffer = new byte[64*1024];
    private int mReadBufferStart = 0;
    private int mReadBufferEnd = 0;
    private byte[] mCipher = new byte[8];
    private UInt32 mConnId = 0;
    private UInt32 mCurrentPackageId = 0;
    private byte[] mHandshakeResPackage = null;
    private byte[] mPongPackage = null;

    private Queue<byte[]> mRPCPackages = new Queue<byte[]>();

    public UInt32 GetCurrentPackageId() {
        return mCurrentPackageId;
    }

    public string GetLastError() {
        return mLastError;
    }

    public bool IsConnected() {
        return mSocket != null && mSocket.Connected;
    }

    public IEnumerator Connect(string host, int port, double timeout, Action<bool> cb) {
        IAsyncResult result = null;
        try {
            result = BeginConnect(host, port);
        } catch(Exception e) {
            mLastError = e.ToString();
            cb(false);
            yield break;
        }
        
        DateTime start = DateTime.Now;
        while(!result.IsCompleted && DateTime.Now.Subtract(start).TotalSeconds < timeout) {
            yield return null;
        }

        if(result.IsCompleted) {
            try {
                EndConnect(result);
            } catch(Exception e) {
                mLastError = e.ToString();
                cb(false);
                yield break;
            }
        } else {
            mLastError = "Connection timeout";
            cb(false);
            yield break;
        }

        cb(true);
    }

    public IEnumerator Handshake(string sessionKey, float timeout, Action<bool> cb) {
        Random random = new Random((int)DateTime.Now.Ticks);
        random.NextBytes(mCipher);
        UInt64 sceretKey = Bits.GetUInt64(mCipher, 0);
        UInt64 exchangeKey = DH.PowModP(DH.G, sceretKey); // Compute exchange key
        byte[] exchangeKeyBytes = new byte[8];
        Bits.PutUInt64(exchangeKey, exchangeKeyBytes, 0);
        byte[] buf = new byte[16];
        Buffer.BlockCopy(exchangeKeyBytes, 0, buf, 0, 8);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] sessionKeyBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(sessionKey));
        Buffer.BlockCopy(sessionKeyBytes, 0, buf, 8, 8);
        try {
            SendPackage(buf, HEADER_HANDSHAKE);
        } catch(Exception e) {
            mLastError = e.Message;
            cb(false);
            yield break;
        }

        DateTime start = DateTime.Now;
        while(mHandshakeResPackage == null && DateTime.Now.Subtract(start).TotalSeconds < timeout) {
            yield return null;
        }
        if(mHandshakeResPackage != null) {
            if(mHandshakeResPackage.Length == 12) {
                mConnId = Bits.GetUInt32(mHandshakeResPackage, 0);
                exchangeKey = Bits.GetUInt64(mHandshakeResPackage, 4);
                UInt64 shareKey = DH.PowModP(exchangeKey, sceretKey);
                Bits.PutUInt64(shareKey, mCipher, 0);
                mHandshakeResPackage = null;
                cb(true);
            } else {
                mHandshakeResPackage = null;
                mLastError = "Handshake failed: invalid data";
                cb(false);
            }
        } else {
            mLastError = "Handshake failed: timeout";
            cb(false);
        }
    }

    public IEnumerator Ping(float timeout, Action<bool> cb) {
        byte[] buf = new byte[8];
        Random random = new Random((int)DateTime.Now.Ticks);
        random.NextBytes(buf);
        try {
            SendPackage(buf, HEADER_PING);
        } catch(Exception e) {
            mLastError = e.Message;
            cb(false);
            yield break;
        }

        DateTime start = DateTime.Now;
        while(mPongPackage == null && DateTime.Now.Subtract(start).TotalSeconds < timeout) {
            yield return null;
        }
        if(mPongPackage != null) {
            mPongPackage = null;
            cb(true);
        } else {
            mLastError = "Ping timeout";
            cb(false);
        }
    }

    public void Disconnect() {
        if(mSocket != null) {
            mSocket.Close();
            mSocket = null;
        }
    }

    public bool Send(byte[] data) {
        UInt32 id;
        return Send(data, out id);
    }

    public bool Send(byte[] data, out UInt32 packageId) {
        packageId = 0;
        try {
            packageId = SendPackage(data, HEADER_RPC);
            return true;
        } catch(Exception e) {
            mLastError = e.Message;
            return false;
        }
    }

    // Return false if network error occur.
    public bool Update() {
        try {
            byte[] package = ProcessReadBuffer();
            if(package != null) {
                HandlePackage(package);
                return true;
            }
            if(mSocket.Poll(1, SelectMode.SelectRead)) {
                int n = mSocket.Receive(mReadBuffer, mReadBufferEnd, mReadBuffer.Length-mReadBufferEnd, SocketFlags.None);
                if(n <= 0) {
                    throw new Exception("Socket is closed by remote peer");
                }
                mReadBufferEnd += n;
            }
            return true;
        } catch(Exception e) {
            mLastError = e.Message + "\n" + e.StackTrace;
            return false;
        }
    }

    public byte[] GetRPCPackage() {
        if(mRPCPackages.Count > 0) {
            return mRPCPackages.Dequeue();
        }
        return null;
    }  

    private IAsyncResult BeginConnect(string host, int port) {
        AddressFamily af;
        string ip;
        CompatibilityIP.GetIpType(host, port.ToString(), out ip, out af);
        mSocket = new Socket(af, SocketType.Stream, ProtocolType.Tcp);
        return mSocket.BeginConnect(ip, port, null, null);
    }

    private void EndConnect(IAsyncResult result) {
        mSocket.EndConnect(result);
        mSocket.NoDelay = true;
    }

    private UInt32 SendPackage(byte[] data, byte header) {
        if(mCurrentPackageId == 0) {
            Random random = new Random((int)DateTime.Now.Ticks);
            mCurrentPackageId = (UInt32)random.Next();
        }
        if(mWriteBuffer.Length < data.Length + 7) {
            mWriteBuffer = new byte[data.Length + 7];
        }
        Bits.PutUInt16((UInt16)(data.Length+5), mWriteBuffer, 0);
        mWriteBuffer[2] = header;
        Bits.PutUInt32(mCurrentPackageId, mWriteBuffer, 3);
        if(header == HEADER_RPC) {
            RC4.Encode(data, 0, mWriteBuffer, 7, data.Length, mCipher);
        } else {
            Buffer.BlockCopy(data, 0, mWriteBuffer, 7, data.Length);
        }
        mSocket.Send(mWriteBuffer, 0, data.Length + 7, SocketFlags.None);
        return mCurrentPackageId;
    }

    private void HandlePackage(byte[] package) {
        if(package.Length < 6) {
            throw new Exception("Invalid package length " + package.Length);
        }
        byte header = package[0];
        mCurrentPackageId = Bits.GetUInt32(package, 1);
        byte[] payload = new byte[package.Length-5];
        Buffer.BlockCopy(package, 5, payload, 0, payload.Length);
        switch(header) {
        case HEADER_PONG:
            HandlePong(payload);
            break;
        case HEADER_HANDSHAKE_RES:
            HandleHandshakeResponse(payload);
            break;
        case HEADER_RPC:
            HandleRPC(payload);
            break;
        default:
            break;
        }
    }

    private void HandlePong(byte[] payload) {
        if(mPongPackage == null) {
            mPongPackage = payload;
        }
    }

    private void HandleHandshakeResponse(byte[] payload) {
        if(mHandshakeResPackage == null) {
            mHandshakeResPackage = payload;
        }
    }

    private void HandleRPC(byte[] payload) {
        RC4.Encode(payload, 0, payload, 0, payload.Length, mCipher);
        mRPCPackages.Enqueue(payload);
    }

    private byte[] ProcessReadBuffer() {
        if((mReadBufferEnd - mReadBufferStart) > 2) {
            int length = (int)Bits.GetUInt16(mReadBuffer, mReadBufferStart);
            if(length <= 0) {
                throw new Exception("Invalid package length " + length);
            }
            if((mReadBufferEnd - mReadBufferStart) >= (2 + length)) {
                var package = new byte[length];
                Buffer.BlockCopy(mReadBuffer, mReadBufferStart + 2, package, 0, package.Length);
                mReadBufferStart += (2 + length);
                ProcessRemainBuffer();
                return package;
            }
        }
        return null;
    }

    private void ProcessRemainBuffer() {
        if(mReadBufferStart > 0) {
            if(mReadBufferStart != mReadBufferEnd) {
                int remain = mReadBufferEnd - mReadBufferStart;
                for(int i = 0; i < remain; i++) {
                    mReadBuffer[i] = mReadBuffer[mReadBufferStart+i];
                }
                mReadBufferStart = 0;
                mReadBufferEnd = remain;
            } else {
                mReadBufferStart = 0;
                mReadBufferEnd = 0;
            }
        }
    }

}

} // namespace Fsoul.Net
