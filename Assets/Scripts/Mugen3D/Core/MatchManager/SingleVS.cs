using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class SingleVS : MatchManager
    {
        public Dictionary<int, int> winCount = new Dictionary<int, int>();
        private readonly int MAX_WIN_COUNT = 3;

        public SingleVS(World world, MatchInfo matchInfo) : base(world, matchInfo)
        {
            
        }

        protected override void OnRoundStart()
        {
            base.OnRoundStart();
            p1.SetHP(p1.GetMaxHP());
            p2.SetHP(p2.GetMaxHP());
            p1.SetPosition(new Vector(world.config.stageConfig.initPos[0].x, world.config.stageConfig.initPos[0].y, 0));
            p2.SetPosition(new Vector(world.config.stageConfig.initPos[1].x, world.config.stageConfig.initPos[1].y, 0));
        }

        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if(p1.GetHP() > p2.GetHP())
            {
                winCount[p1.slot]++;
            }else if(p2.GetHP() > p1.GetHP())
            {
                winCount[p2.slot]++;
            }
            if(winCount[p1.slot] >= MAX_WIN_COUNT || winCount[p2.slot] >= MAX_WIN_COUNT)
            {
                StopMatch();
            }else
            {
                StartRound(this.roundNo + 1);
            }
        }

        protected override void OnMatchStart()
        {
            base.OnMatchStart();
            p1 = EntityFactory.CreateCharacter(matchInfo.p1Characters[0], 0, true);
            p2 = EntityFactory.CreateCharacter(matchInfo.p2Characters[0], 1, false);
            world.AddCharacter(p1);
            world.AddCharacter(p2); 
            winCount[p1.slot] = 0;
            winCount[p2.slot] = 0;
        }

        protected override void OnMatchEnd()
        {
            base.OnMatchEnd();
            //world.RemoveCharacter(0);
            //world.RemoveCharacter(1);
        }
    }
}
