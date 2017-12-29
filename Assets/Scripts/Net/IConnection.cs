// Copyright (C) 2017 Fsoul Inc.

using System;

namespace Fsoul.Net {

public enum NetError {
    NONE            = 0,
    TIMEOUT         = 1,
    REMOTE_DOWN     = 2,
    REMOTE_CLOSE    = 3,
}

public delegate void ConnectListener(NetError e);

public delegate void DisconnectListener(NetError e);

public delegate void PacketListener(NetPacket packet);

public interface IConnection {
    void Connect(string host, int port, double timeout);
    void Disconnect();
    bool IsConnected();
    void SendPacket(NetPacket packet);
    void Poll();
    void AddConnectListener(ConnectListener listener);
    void RemoveConnectListener(ConnectListener listener);
    void AddDisconnectListener(DisconnectListener listener);
    void RemoveDisconnectListener(DisconnectListener listener);
    void AddPacketListener(PacketListener listener);
    void RemovePacketListener(PacketListener listener);
}

}