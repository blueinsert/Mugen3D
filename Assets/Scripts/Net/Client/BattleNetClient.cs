using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Net
{
    public class BattleNetClient : MonoBehaviour
    {
        public event Action<int> onRoomCreate;
        public event Action onMatchCreate;
        public event Action onGameStart;
        public event Action<int, int[]> onGameUpdate;
        public event Action onGameEnd;

        private static BattleNetClient m_instance;

        public static BattleNetClient Instance
        {
            get
            {
                if (m_instance == null)
                {
                    GameObject go = new GameObject("__BattleNetClient");
                    m_instance = go.AddComponent<BattleNetClient>();
                }
                m_instance.Init();
                return m_instance;
            }
        }

        private void Init()
        {
            conn.msgDist.AddListener("RoomCreate", (res) =>
            {
                if (onRoomCreate != null)
                {
                    onRoomCreate(1);
                }
            });
            conn.msgDist.AddListener("MatchCreate", (res) => {
                if (onMatchCreate != null)
                {
                    onMatchCreate();
                }
            });
            conn.msgDist.AddListener("GameStart", (res) =>
            {
                if (onGameStart != null)
                {
                    onGameStart();
                }
            });
            conn.msgDist.AddListener("GameUpdate", (res) =>
            {
                if (onGameUpdate != null)
                {
                    onGameUpdate(2, new int[]{1,1});
                }
            });
        }

        public static void Destroy()
        {
            GameObject.Destroy(m_instance.gameObject);
            m_instance = null;
        }

        private Connection conn = new Connection();

        public void Update()
        {
            conn.Update();
        }

        public bool Connect(string host, int port)
        {
            return conn.Connect(host, port);
        }

        public void CreateRoom()
        {
            Mugen3D.Net.Protocol.ProtocolBytes proto = new Mugen3D.Net.Protocol.ProtocolBytes();
            proto.AddString("CreateRoom");
            conn.Send(proto);
        }

        public void JoinRoom(int roomId)
        {
            Mugen3D.Net.Protocol.ProtocolBytes proto = new Mugen3D.Net.Protocol.ProtocolBytes();
            proto.AddString("JoinRoom");
            //proto.AddInt(roomId);
            conn.Send(proto);
        }

        public void SendInput(int frameNo, int value)
        {
            Mugen3D.Net.Protocol.ProtocolBytes proto = new Mugen3D.Net.Protocol.ProtocolBytes();
            proto.AddString("Input");
            proto.AddInt(frameNo);
            proto.AddInt(value);
        }
    }
}
