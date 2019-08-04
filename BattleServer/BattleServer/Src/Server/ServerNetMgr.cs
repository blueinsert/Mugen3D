using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BattleServer
{

    public class ServerNetMgr
    {
        //监听套接字
        public Socket listenfd;
        public Conn[] conns;
        const int MAX_CONN = 100;
        public long HEART_BEAT_TIME = 5;
        public Protocol.ProtocolBase proto;
        System.Timers.Timer timer = new System.Timers.Timer(1000);

        //开启服务器
        public void Start(string host, int port)
        {
            timer.Elapsed += new System.Timers.ElapsedEventHandler(HandleMainTimer);
            timer.AutoReset = false;
            timer.Enabled = true;
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
            Console.WriteLine("[服务器]启动成功 listening in:" + host + ":" + port);
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
                    MemoryData.players[conn] = new Player(conn);
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
                    if (!conn.isUse)
                    {
                        return;
                    }
                    int count = conn.socket.EndReceive(ar);
                    //关闭信号
                    if (count <= 0)
                    {
                        Console.WriteLine("收到 [" + conn.GetAdress() + "] 断开链接");
                        CloseConn(conn);
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
                    CloseConn(conn);
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

        private void HandleMsg(Conn conn, Protocol.ProtocolBase protoBase)
        {
            Handler.Process(conn, protoBase);
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

        public void HandleMainTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            HeartBeat();
            timer.Start();
        }

        //心跳
        public void HeartBeat()
        {
            //Console.WriteLine("HeartBeat");
            long timeNow = TimeUtility.GetTimeStamp();
            for (int i = 0; i < conns.Length; i++)
            {
                Conn conn = conns[i];
                if (conn == null) continue;
                if (!conn.isUse) continue;

                if (timeNow - conn.lastTickTime  > HEART_BEAT_TIME)
                {
                    Console.WriteLine("[心跳引起断开连接] " + conn.GetAdress());
                    CloseConn(conn);
                }
            }
        }

        private void CloseConn(Conn conn)
        {
            lock (conn)
            {
                var player = MemoryData.players[conn];
                if (player != null && player.curRoom != null)
                {
                    if (player.curRoom.status == RoomStatus.Battling)
                    {
                        player.curRoom.EndGame();
                    }
                    else
                    {
                        player.curRoom.RemovePlayer(player);
                    }
                }
                MemoryData.players[conn] = null;
                conn.Close();
            }
        }

        //关闭
        public void Close()
        {
            for (int i = 0; i < conns.Length; i++)
            {
                Conn conn = conns[i];
                if (conn == null) continue;
                if (!conn.isUse) continue;
                CloseConn(conn); 
            }
        }

    }
}
 