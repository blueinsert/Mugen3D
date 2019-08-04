using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServer
{
    class ConnMsgHandler
    {
        public static void HeartBeat(Conn conn, Protocol.ProtocolBase proto)
        {
            conn.lastTickTime = TimeUtility.GetTimeStamp();
            //Console.WriteLine("[HeartBeat]" + conn.GetAdress());
        }
    }
}
