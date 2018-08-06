using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mugen3D.Net
{

    public class ServerNetMgr : MonoBehaviour
    {

        private static ServerNetMgr m_instance;
        public static ServerNetMgr Instance
        {
            get
            {
                if (m_instance == null)
                {
                    GameObject go = new GameObject("_ServerNetMgr");
                    m_instance = go.AddComponent<ServerNetMgr>();
                }
                return m_instance;
            }
        }

        public static void Destroy()
        {
            GameObject.Destroy(m_instance.gameObject);
            m_instance = null;
        }

        //监听套接字
        public Socket listenfd;
        public Conn[] conns;
        const int MAX_CONN = 2;
        public long HEART_BEAT_TIME = 180;
        public Protocol.ProtocolBase proto;

        public void Start() { }

        //开启服务器
        public void Start(string host, int port)
        {
            InvokeRepeating("HeartBeat", 1, 60);
            proto = new Protocol.ProtocolBytes();
            //链接池
            conns = new Conn[MAX_CONN];
            for (int i = 0; i < MAX_CONN; i++)
            {
                conns[i] = new Conn();
            }
            //Socket
            listenfd = new Socket(AddressFamily.InterNetwork,
                                  SocketType.Stream, ProtocolType.Tcp);
            //Bind
            IPAddress ipAdr = IPAddress.Parse(host);
            IPEndPoint ipEp = new IPEndPoint(ipAdr, port);
            listenfd.Bind(ipEp);
            //Listen
            listenfd.Listen(MAX_CONN);
            //Accept
            listenfd.BeginAccept(AcceptCb, null);
            Console.WriteLine("[服务器]启动成功");
        }

        //获取链接池索引，返回负数表示获取失败
        public int NewIndex()
        {
            if (conns == null)
                return -1;
            for (int i = 0; i < conns.Length; i++)
            {
                if (conns[i] == null)
                {
                    conns[i] = new Conn();
                    return i;
                }
                else if (conns[i].isUse == false)
                {
                    return i;
                }
            }
            return -1;
        }

        private void AcceptCb(IAsyncResult ar)
        {
            try
            {
                Socket socket = listenfd.EndAccept(ar);
                int index = NewIndex();

                if (index < 0)
                {
                    socket.Close();
                    Console.Write("[警告]链接已满");
                }
                else
                {
                    Conn conn = conns[index];
                    conn.Init(socket);
                    string adr = conn.GetAdress();
                    Console.WriteLine("客户端连接 [" + adr + "] conn池ID：" + index);
                    conn.socket.BeginReceive(conn.readBuff,
                                             conn.buffCount, conn.BuffRemain(),
                                             SocketFlags.None, ReceiveCb, conn);
                }
                listenfd.BeginAccept(AcceptCb, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("AcceptCb失败:" + e.Message);
            }
        }

        private void ReceiveCb(IAsyncResult ar)
        {
            Conn conn = (Conn)ar.AsyncState;
            lock (conn)
            {
                try
                {
                    int count = conn.socket.EndReceive(ar);
                    //关闭信号
                    if (count <= 0)
                    {
                        Console.WriteLine("收到 [" + conn.GetAdress() + "] 断开链接");
                        conn.Close();
                        return;
                    }
                    conn.buffCount += count;
                    ProcessData(conn);
                    //继续接收	
                    conn.socket.BeginReceive(conn.readBuff,
                                             conn.buffCount, conn.BuffRemain(),
                                             SocketFlags.None, ReceiveCb, conn);
                }
                catch (Exception e)
                {
                    Console.WriteLine("收到 [" + conn.GetAdress() + "] 断开链接 " + e.Message);
                    conn.Close();
                }
            }
        }


        private void ProcessData(Conn conn)
        {
            //小于长度字节
            if (conn.buffCount < sizeof(Int32))
            {
                return;
            }
            //消息长度
            Array.Copy(conn.readBuff, conn.lenBytes, sizeof(Int32));
            conn.msgLength = BitConverter.ToInt32(conn.lenBytes, 0);
            if (conn.buffCount < conn.msgLength + sizeof(Int32))
            {
                return;
            }
            //处理消息
            Protocol.ProtocolBase protocol = proto.Decode(conn.readBuff, sizeof(Int32), conn.msgLength);
            HandleMsg(conn, protocol);
            //清除已处理的消息
            int count = conn.buffCount - conn.msgLength - sizeof(Int32);
            Array.Copy(conn.readBuff, sizeof(Int32) + conn.msgLength, conn.readBuff, 0, count);
            conn.buffCount = count;
            if (conn.buffCount > 0)
            {
                ProcessData(conn);
            }
        }

        public void Update()
        {

        }

        private void HandleMsg(Conn conn, Protocol.ProtocolBase protoBase)
        {
            string name = protoBase.GetName();
            Debug.Log(conn.GetAdress() + " protocol:" + name);
        }

        //发送
        public void Send(Conn conn, Protocol.ProtocolBase protocol)
        {
            byte[] bytes = protocol.Encode();
            byte[] length = BitConverter.GetBytes(bytes.Length);
            byte[] sendbuff = length.Concat(bytes).ToArray();
            try
            {
                conn.socket.BeginSend(sendbuff, 0, sendbuff.Length, SocketFlags.None, null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("[发送消息]" + conn.GetAdress() + " : " + e.Message);
            }
        }

        //广播
        public void Broadcast(Protocol.ProtocolBase protocol)
        {
            for (int i = 0; i < conns.Length; i++)
            {
                if (!conns[i].isUse)
                    continue;
                Send(conns[i], protocol);
            }
        }

        //心跳
        public void HeartBeat()
        {
            long timeNow = TimeUtility.GetTimeStamp();
            for (int i = 0; i < conns.Length; i++)
            {
                Conn conn = conns[i];
                if (conn == null) continue;
                if (!conn.isUse) continue;

                if (timeNow - conn.lastTickTime  > HEART_BEAT_TIME)
                {
                    Console.WriteLine("[心跳引起断开连接]" + conn.GetAdress());
                    lock (conn)
                        conn.Close();
                }
            }
        }

    }
}
 