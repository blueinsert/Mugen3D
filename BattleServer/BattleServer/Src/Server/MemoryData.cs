using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServer
{
    class MemoryData
    {
        public static Dictionary<Conn, Player> players = new Dictionary<Conn, Player>();
    }
}
