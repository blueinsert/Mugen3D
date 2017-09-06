using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class PlayerSetting
    {
        public string name;
        public string prefabName;
        public string commandFile;
        public string commonStateFile;
        public List<string> stateFiles;
        public string paramsSetFile;
    }

    public class PlayersConfig
    {
        private static PlayersConfig mInstance;
        public static PlayersConfig Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new PlayersConfig();
                    mInstance.Init();
                }
                return mInstance;
            }
        }

        private Dictionary<string, PlayerSetting> mPlayers;

        private void Init()
        {
            mPlayers = new Dictionary<string, PlayerSetting>();
            PlayerSetting mike = new PlayerSetting();
            string pre;
            //add Mike
            mike.name = "Mike";
            pre = Application.dataPath + "/Chars/" + "Mike/";
            mike.prefabName = pre + "Mike.prefab";
            mike.commandFile = pre + "Mike.cmd";
            mike.commonStateFile = "";
            mike.stateFiles = new List<string>();
            mike.stateFiles.Add(pre + "Mike.def");
            mike.paramsSetFile = "";

            mPlayers.Add(mike.name, mike);
        }

        public PlayerSetting GetPlayerSetting(string playerName)
        {
            if (mPlayers.ContainsKey(playerName))
            {
                return mPlayers[playerName];
            }
            else
            {
                return null;
            }
        }
    }
}
