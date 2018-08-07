using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Net.Server
{
    public class RoomMgr
    {
        private static RoomMgr m_instance;
        public static RoomMgr Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new RoomMgr();
                }
                return m_instance;
            }
        }

        public Room room;

        public void CreateRoom(Conn owner)
        {
            if (room != null)
            {
                Debug.LogError("room has created");
            }
            room = new Room(owner);
            Debug.Log(owner.GetAdress() + " Create Room");
        }

        public void JoinRoom(Conn conn)
        {
            if (room == null)
            {
                Debug.LogError("room is null");
            }
            room.AddPlayer(conn);
            Debug.Log(conn.GetAdress() + " Join Room");
        }


    }
}
