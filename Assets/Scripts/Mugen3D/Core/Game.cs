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

        public Game(MatchInfo matchInfo, WorldConfig worldConfig, int fps) {

            this.fps = fps;
            this.deltaTime = new Number(1000 / fps) / new Number(1000);
            Time.Clear();
            world = new World(worldConfig);
            switch (matchInfo.mode)
            {
                case MatchMode.SingleVS:
                    matchManager = new SingleVS(world, matchInfo);
                    break;
                default:
                    matchManager = new SingleVS(world, matchInfo);
                    break;
            }
            world.matchManager = matchManager;
        }

        public void Step()
        {
            Time.Update(deltaTime);
            if (matchManager != null)
                matchManager.Update();
            if(matchManager != null && matchManager.matchState == MatchState.Running)
            {
                world.Update();
            }
        }

        public void StartGame()
        {
            matchManager.StartMatch(0);
        }
    }
}

