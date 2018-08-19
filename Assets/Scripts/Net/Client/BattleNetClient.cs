using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Net
{
    public class BattleNetClient : MonoBehaviour
    {
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

        public int roomId { get; private set; }

        private void Init()
        {
            conn.msgDist.AddListener("MatchCreate", (res) => {
                Protocol.ProtocolBytes protocol = (Protocol.ProtocolBytes)res;
                int start = 0;
                string protoName = protocol.GetString(start, ref start);
                roomId = protocol.GetInt(start, ref start);
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
                    Protocol.ProtocolBytes protocol = (Protocol.ProtocolBytes)res;
                    int start = 0;
                    string protoName = protocol.GetString(start, ref start);
                    int frameNo = protocol.GetInt(start, ref start);
                    int input1 = protocol.GetInt(start, ref start);
                    int input2 = protocol.GetInt(start, ref start);
                    onGameUpdate(frameNo, new int[] { input1, input2 });
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

        public void FindMatch()
        {
            Mugen3D.Net.Protocol.ProtocolBytes proto = new Mugen3D.Net.Protocol.ProtocolBytes();
            proto.AddString("FindMatch");
            conn.Send(proto);
        }

        public void CancelFindMatch()
        {
            Mugen3D.Net.Protocol.ProtocolBytes proto = new Mugen3D.Net.Protocol.ProtocolBytes();
            proto.AddString("CancelFindMatch");
            conn.Send(proto);
        }

        public void SendInput(int frameNo, int value)
        {      
            Protocol.FrameData frameData = new Protocol.FrameData(roomId, frameNo, value);
            conn.Send(frameData);
        }
    }
}
