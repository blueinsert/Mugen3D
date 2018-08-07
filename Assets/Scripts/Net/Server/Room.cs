using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Net.Server
{
    public class Room
    {
        private Conn p1;
        private Conn p2;

        public Room(Conn owner)
        {
            p1 = owner;
        }

        public void AddPlayer(Conn p2)
        {
            if (this.p2 != null)
            {
                Debug.LogError("p2 has existed");
                return;
            }
            this.p2 = p2;
            if (this.p1 != null && this.p2 != null)
            {
                Protocol.ProtocolBytes proto = new Protocol.ProtocolBytes();
                proto.AddString("GameStart");
                Broadcast(proto);
            }
        }

        //广播
        public void Broadcast(Protocol.ProtocolBase protocol)
        {
            p1.Send(protocol);
            p2.Send(protocol);
        }
    }
}