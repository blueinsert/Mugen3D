using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class World
    {
        private static World mInstance;
        public static World Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new World();
                    mInstance.Init();
                }
                return mInstance;
            }
        }
        private World() { }
        private void Init() { }

        private Dictionary<PlayerId, Player> mPlayers = new Dictionary<PlayerId, Player>();
        public Dictionary<PlayerId, Player> Players { get { return mPlayers; } }

        public void AddPlayer(PlayerId id, Player p){
            mPlayers[id] = p;
        }

        public Player GetPlayer(PlayerId id)
        {
            return mPlayers[id];
        }
    }
}
