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
        public MatchMode mode { get; private set; }
        public MatchManager matchManager { get; private set; }

        public Game(MatchMode mode, WorldConfig worldConfig, int fps) {
            this.fps = fps;
            this.deltaTime = new Number(1000 / fps) / new Number(1000);
            Time.Clear();
            world = new World(worldConfig);
            matchManager = new MatchManager(world);
        }

        public void Step()
        {
            Time.Update(deltaTime);
            matchManager.Update(); 
        }

        public void StartGame()
        {

        }
    }
}

