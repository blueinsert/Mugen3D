using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Core
{
    public class Game
    {
        private int fps;
        private Number deltaTime;
        public World world { get; private set; }
        public MatchManager matchManager { get; private set; }

        public Game(WorldConfig worldConfig, int fps) {
            this.fps = fps;
            this.deltaTime = new Number(1000 / fps) / new Number(1000);
            Time.Clear();
            world = new World(worldConfig);    
        }

        public void Step()
        {
            Time.Update(deltaTime);
            if (matchManager != null)
                matchManager.Update();
        }

        public void StartGame(MatchInfo info)
        {
            switch (info.mode)
            {
                case MatchMode.SingleVS:
                    matchManager = new SingleVS(world, info);
                    break;
                default:
                    matchManager = new SingleVS(world, info);
                    break;
            }
            matchManager.StartMatch(0);
            world.matchManager = matchManager;
        }
    }
}

