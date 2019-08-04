using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServer
{
    class Handler
    {
        public static void Process(Conn conn, Protocol.ProtocolBase protoBase)
        {
            string name = protoBase.GetName();
            var player = MemoryData.players[conn];
            switch (name)
            { 
                case "FindMatch":
                    BattleMsgHandler.FindMatch(player, protoBase);
                    break;
                case "CancelFindMatch":
                    BattleMsgHandler.CancelFindMatch(player, protoBase);
                    break;
                case "Progress":
                    BattleMsgHandler.UpdateGameLoadProgress(player, protoBase);
                    break;
                case "FrameData":
                    BattleMsgHandler.UpdateInput(player, protoBase);
                    break;
                case "EndGame":
                    BattleMsgHandler.EndGame(player, protoBase);
                    break;
                case "HeartBeat":
                    ConnMsgHandler.HeartBeat(conn, protoBase);
                    break;
            }
        }
    }
}
