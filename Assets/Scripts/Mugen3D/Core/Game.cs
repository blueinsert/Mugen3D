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
            {
                matchManager.Update();
                world.Update();
            }  
        }

        private int ModifyInput(int input, int facing)
        {
            if(facing < 0)
            {
                if((input & Utility.GetKeycode(KeyNames.KEY_LEFT)) != 0){
                    input = input - Utility.GetKeycode(KeyNames.KEY_LEFT);
                    input = input | Utility.GetKeycode(KeyNames.KEY_RIGHT);
                }else if((input & Utility.GetKeycode(KeyNames.KEY_RIGHT)) != 0)
                {
                    input = input - Utility.GetKeycode(KeyNames.KEY_RIGHT);
                    input = input | Utility.GetKeycode(KeyNames.KEY_LEFT);
                }
            }
            return input;
        }

        public void UpdateInput(int p1InputCode, int p2InputCode)
        {
            if(matchManager.p1 != null)
            {
                matchManager.p1.UpdateInput(ModifyInput(p1InputCode, matchManager.p1.GetFacing()));
            }
            if(matchManager.p2 != null)
            {
                matchManager.p2.UpdateInput(ModifyInput(p2InputCode, matchManager.p2.GetFacing()));
            }
        }

        public void StartGame()
        {
            matchManager.StartMatch(0);
        }
    }
}

