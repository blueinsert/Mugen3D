using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Mugen3D.Net
{
    //网络链接
    public class Connection
    {
        //常量
        const int BUFFER_SIZE = 1024;
        const float HEART_BEAT_TIME = 30;

        //Socket
        private Socket socket;
        //Buff
        private byte[] readBuff = new byte[BUFFER_SIZE];
        private int buffCount = 0;
        //沾包分包
        private Int32 msgLength = 0;
        private byte[] lenBytes = new byte[sizeof(Int32)];
        //协议
        public Protocol.ProtocolBase proto = new Protocol.ProtocolBytes();
        //心跳时间
        public float lastTickTime = 0;

        //消息分发
        public MsgDistribution msgDist = new MsgDistribution();
        ///状态
        public enum Status
        {
            None,
            Connected,
        };
        public Status status = Status.None;


        //连接服务端
        public bool Connect(string host, int port)
        {
            try
            {
                //socket
                socket = new Socket(AddressFamily.InterNetwork,
                          SocketType.Stream, ProtocolType.Tcp);
                //Connect
                socket.Connect(host, port);
                //BeginReceive
                socket.BeginReceive(readBuff, buffCount,
                          BUFFER_SIZE - buffCount, SocketFlags.None,
                          ReceiveCb, readBuff);
                Debug.Log("连接成功");
                //状态
                status = Status.Connected;
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("连接失败:" + e.Message);
                return false;
            }
        }

        //关闭连接
        public bool Close()
        {
            try
            {
                socket.Close();
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("关闭失败:" + e.Message);
                return false;
            }
        }

        //接收回调
        private void ReceiveCb(IAsyncResult ar)
        {
            try
            {
                int count = socket.EndReceive(ar);
                buffCount = buffCount + count;
                ProcessData();
                socket.BeginReceive(readBuff, buffCount,
                         BUFFER_SIZE - buffCount, SocketFlags.None,
                         ReceiveCb, readBuff);
            }
            catch (Exception e)
            {
                Debug.Log("ReceiveCb失败:" + e.Message);
                status = Status.None;
            }
        }

        //消息处理
        private void ProcessData()
        {
            //沾包分包处理
            if (buffCount < sizeof(Int32))
                return;
            //包体长度
            Array.Copy(readBuff, lenBytes, sizeof(Int32));
            msgLength = BitConverter.ToInt32(lenBytes, 0);
            if (buffCount < msgLength + sizeof(Int32))
                return;
            //协议解码
            Protocol.ProtocolBase protocol = proto.Decode(readBuff, sizeof(Int32), msgLength);
            //Debug.Log("收到消息 " + protocol.GetDesc());
            lock (msgDist.msgList)
            {
                msgDist.msgList.Add(protocol);
            }
            //清除已处理的消息
            int count = buffCount - msgLength - sizeof(Int32);
            Array.Copy(readBuff, sizeof(Int32) + msgLength, readBuff, 0, count);
            buffCount = count;
            if (buffCount > 0)
            {
                ProcessData();
            }
        }


        public bool Send(Protocol.ProtocolBase protocol)
        {
            if (status != Status.Connected)
            {
                Debug.LogError("[Connection]还没链接就发送数据是不好的");
                return true;
            }

            byte[] b = protocol.Encode();
            byte[] length = BitConverter.GetBytes(b.Length);

            byte[] sendbuff = length.Concat(b).ToArray();
            socket.Send(sendbuff);
            return true;
        }

        public bool Send(Protocol.ProtocolBase protocol, string cbName, MsgDistribution.Delegate cb)
        {
            if (status != Status.Connected)
                return false;
            msgDist.AddOnceListener(cbName, cb);
            return Send(protocol);
        }

        public bool Send(Protocol.ProtocolBase protocol, MsgDistribution.Delegate cb)
        {
            string cbName = protocol.GetName();
            return Send(protocol, cbName, cb);
        }



        public void Update()
        {
            //消息
            msgDist.Update();
            //心跳
            if (status == Status.Connected)
            {
                if (Time.time - lastTickTime > HEART_BEAT_TIME)
                {
                    Protocol.ProtocolBytes protocol = new Protocol.ProtocolBytes();
                    protocol.AddString("HeatBeat");
                    //ProtocolBase protocol = NetMgr.GetHeatBeatProtocol();
                    Send(protocol);
                    lastTickTime = Time.time;
                }
            }
        }
    }
}