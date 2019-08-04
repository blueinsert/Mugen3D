using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServer
{
    public enum PlayerStatus
    {
        None,
        Prepare,
        Fighting,
    }

    public class Player
    {
        public Conn conn { get; private set; }
        public PlayerStatus status = PlayerStatus.None;
        public Room curRoom {get; set;}
        public int curInput = 0;

        public Player(Conn conn)
        {
            this.conn = conn;
        }

    }
}
