// Copyright (C) 2017 Fsoul Inc.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Fsoul.Net {

public class TCPConnection : IConnection {

    public enum State {
        DISCONNECTED    = 0,
        CONNECTING      = 1,
        CONNECTED       = 2
    }

    private State m_state;
    private Socket m_socket;
    private DateTime m_connectTimeout;
    private byte[] m_buffer = new byte[8*1024];
    private int m_bufferReadPos = 0;
    private int m_bufferWritePos = 0;
    private List<ConnectListener> m_connectListeners = new List<ConnectListener>();
    private List<DisconnectListener> m_disconnectListeners = new List<DisconnectListener>();
    private List<PacketListener> m_packetListeners = new List<PacketListener>();
    private NetPacket m_packet;

    public void Connect(string host, int port, double timeout) {
        AddressFamily af;
        string ip;
        CompatibilityIP.GetIpType(host, port.ToString(), out ip, out af);
        try {
            Disconnect();
            m_socket = new Socket(af, SocketType.Stream, ProtocolType.Tcp);
            m_socket.Blocking = false;
            m_socket.NoDelay = true;
            m_socket.Connect(ip, port);
            m_connectTimeout = DateTime.Now.AddSeconds(timeout);
            m_state = State.CONNECTING;
        } catch (SocketException e) {
            if (e.ErrorCode != 10035) { // WSAEWOULDBLOCK
                UnityEngine.Debug.LogError(e.Message);
                HandleConnectError(false);
            }
        } catch (Exception e) {
            UnityEngine.Debug.LogError(e.Message);
            HandleConnectError(false);
        }
    }

    public void Disconnect() {
        if (m_state != State.DISCONNECTED) {
            try {
                m_socket.Shutdown(SocketShutdown.Both);
                m_socket.Close();
            } catch (Exception) {

            }
            m_socket = null;
            m_state = State.DISCONNECTED;
        }
    }

    public bool IsConnected() {
        return m_state == State.CONNECTED;
    }

    public void SendPacket(NetPacket p) {
        byte[] data = p.Encode();
        //Log.Info("Send bytes " + data.Length);
        m_socket.Send(data, 0, data.Length, SocketFlags.None);
    }

    public void Poll() {
        if (m_state == State.CONNECTING) {
            try {
                if (m_socket.Poll(1000, SelectMode.SelectError)) {
                    HandleConnectError(false);
                } else if (m_socket.Poll(1000, SelectMode.SelectWrite)) {
                    HandleConnectSuccess();
                } else if (DateTime.Now.CompareTo(m_connectTimeout) > 0) {
                    HandleConnectError(false);
                }
            } catch(Exception e) {
                HandleConnectError(false);
            }
        } else if (m_state == State.CONNECTED) {
            try {
                if (m_socket.Poll(1000, SelectMode.SelectRead)) {
                    int n = m_socket.Receive(m_buffer, m_bufferWritePos, m_buffer.Length - m_bufferWritePos, SocketFlags.None);
                    if (n <= 0) {
                        HandleDisconnect(true);
                    } else {
                        m_bufferWritePos += n;
                        HandleReceiveData();
                    }
                }
            } catch(Exception e) {
                UnityEngine.Debug.LogError(e.Message+"\n"+e.StackTrace);
                HandleDisconnect(false);
            }
        }
    }

    public void AddConnectListener(ConnectListener listener) {
        m_connectListeners.Add(listener);
    }

    public void RemoveConnectListener(ConnectListener listener) {
        m_connectListeners.Remove(listener);
    }

    public void AddDisconnectListener(DisconnectListener listener) {
        m_disconnectListeners.Add(listener);
    }

    public void RemoveDisconnectListener(DisconnectListener listener) {
        m_disconnectListeners.Remove(listener);
    }

    public void AddPacketListener(PacketListener listener) {
        m_packetListeners.Add(listener);
    }

    public void RemovePacketListener(PacketListener listener) {
        m_packetListeners.Remove(listener);
    }

    private void HandleConnectSuccess() {
        m_state = State.CONNECTED;
        foreach(var l in m_connectListeners) {
            l(NetError.NONE);
        }
    }

    private void HandleConnectError(bool isTimeout) {
        Disconnect();
        m_state = State.DISCONNECTED;
        foreach(var l in m_connectListeners) {
            if (isTimeout) {
                l(NetError.TIMEOUT);
            } else {
                l(NetError.REMOTE_DOWN);
            }
        }
    }

    private void HandleDisconnect(bool remoteClose) {
        Disconnect();
        m_state = State.DISCONNECTED;
        foreach(var l in m_disconnectListeners) {
            if (remoteClose) {
                l(NetError.REMOTE_CLOSE);
            } else {
                l(NetError.REMOTE_DOWN);
            }
        }
    }

    private void HandleReceiveData() {
        if (m_packet == null) {
            m_packet = new NetPacket();
        }

        int n = m_packet.Decode(m_buffer, m_bufferReadPos, m_bufferWritePos - m_bufferReadPos);
        m_bufferReadPos += n;
        if (m_bufferReadPos == m_bufferWritePos) {
            m_bufferReadPos = m_bufferWritePos = 0;
        }

        if (m_packet.IsComplete()) {
            foreach(var ln in m_packetListeners) {
                try {
                    ln(m_packet);
                } catch (Exception e) {
                    UnityEngine.Debug.LogError(e.Message+"\n"+e.StackTrace);
                }
            }
            m_packet = null;
            HandleReceiveData(); // handle rest packet
        }
    }
}

}