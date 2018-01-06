using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;
namespace Mugen3D
{
    public class World
    {
        public int gameTime = -1;
        public float deltaTime;
        private Dictionary<PlayerId, Player> mPlayers = new Dictionary<PlayerId, Player>();
        public Dictionary<PlayerId, Player> Players { get { return mPlayers; } }

        public static World mInstance;
        public static World Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new World();
                }
                return mInstance;
            }
        }

        private World()
        {
            Init();
        }

        private void Init() { }

        public void AddPlayer(PlayerId id, Player p)
        {
            mPlayers[id] = p;
        }

        public Player GetPlayer(PlayerId id)
        {
            if (mPlayers.ContainsKey(id))
                return mPlayers[id];
            else
                return null;
        }

        public void Update(float _deltaTime)
        {
            gameTime++;
            deltaTime = _deltaTime;
            foreach (var p in Players)
            {
                p.Value.OnUpdate();
            }
            UpdateFacing();
        }

        private void UpdateFacing()
        {
            var p1 = GetPlayer(PlayerId.P1);
            var p2 = GetPlayer(PlayerId.P2);
            if (p1 == null || p2 == null)
                return;
            if (p1.transform.position.x > p2.transform.position.x)
            {
                p1.ChangeFacing(-1);
                p2.ChangeFacing(1);
            }
            else
            {
                p1.ChangeFacing(1);
                p2.ChangeFacing(-1);
            }
        }
    }
}
