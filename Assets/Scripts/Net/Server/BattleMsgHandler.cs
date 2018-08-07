using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Net.Server
{
    public class BattleMsgHandler
    {

        public static void CreateRoom(Conn conn, Protocol.ProtocolBase proto)
        {
            RoomMgr.Instance.CreateRoom(conn);
        }

        public static void JoinRoom(Conn conn, Protocol.ProtocolBase proto)
        {
            RoomMgr.Instance.JoinRoom(conn);
        }
    }
}
