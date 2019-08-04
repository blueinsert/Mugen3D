using System;
using System.Collections;
using System.Collections.Generic;

namespace BattleServer
{
    public class RoomMgr
    {
        const int MAX_ROOM_NUM = 10;

        private static RoomMgr m_instance;
        public static RoomMgr Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new RoomMgr();
                    m_instance.Init();
                }
                return m_instance;
            }
        }

        private Dictionary<int, Room> m_rooms = new Dictionary<int, Room>();

        private void Init()
        {
            for(int i = 0; i < MAX_ROOM_NUM; i++){
                m_rooms.Add(i, new Room(i));
            }
        }

        public void FindMatch(Player player)
        {
            lock (m_rooms)
            {
                for (int i = 0; i < m_rooms.Count; i++)
                {
                    var room = m_rooms[i];
                    if (!room.IsFull())
                    {
                        room.AddPlayer(player);
                        return;
                    }
                }
            }   
        }

        public void Update()
        {
            for (int i = 0; i < MAX_ROOM_NUM; i++)
            {
                var room = m_rooms[i];
                room.Update();
            }
        }
    }
}
