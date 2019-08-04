using System.Collections;
using System.Collections.Generic;
using BattleServer.Protocol;

namespace BattleServer
{
    public class BattleMsgHandler
    {

        public static void FindMatch(Player player, Protocol.ProtocolBase proto)
        {
            RoomMgr.Instance.FindMatch(player);
        }

        public static void CancelFindMatch(Player player, Protocol.ProtocolBase proto)
        {
            if (player.curRoom != null)
            {
                player.curRoom.RemovePlayer(player);
            }
        }

        public static void UpdateGameLoadProgress(Player player, Protocol.ProtocolBase proto)
        {
            ProtocolBytes protocol = (ProtocolBytes)proto;
            int start = 0;
            string protoName = protocol.GetString(start, ref start);
            int progress = protocol.GetInt(start, ref start);
            if (player.curRoom != null)
            {
                player.curRoom.UpdateGameLoadProgress(player, progress);
            }
            else
            {
                System.Console.WriteLine("ERROR player'room null when update load progress");
            }
        }

        public static void UpdateInput(Player player, Protocol.ProtocolBase proto)
        {
            ProtocolBytes protocol = (ProtocolBytes)proto;
            FrameData frameData = new FrameData(protocol.bytes);
            if (player.curRoom != null)
            {
                player.curRoom.UpdateInput(player, frameData.input);
            }
            else
            {
                System.Console.WriteLine("ERROR player'room null when update input");
            }
        }

        public static void EndGame(Player player, Protocol.ProtocolBase proto)
        {
            System.Console.WriteLine(player.conn.GetAdress() + " self endGame");
            if (player.curRoom != null && player.curRoom.status == RoomStatus.Battling)
            {
                player.curRoom.EndGame();
            }
        }
    }
}
