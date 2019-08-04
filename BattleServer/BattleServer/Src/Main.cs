using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            ServerNetMgr serverNet = new ServerNetMgr();
            serverNet.Start("127.0.0.1", 1234);
            while (true)
            {
                RoomMgr.Instance.Update();
                /*
                string str = Console.ReadLine();
                switch (str)
                {
                    case "quit":
                        serverNet.Close();
                        return;
                }
                 */
            }
        }
    }
}
