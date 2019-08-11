using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class SinglePlay : MatchManager
    {
        public SinglePlay(BattleWorld world, MatchInfo info) : base(world, info)
        {
        }

        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
        }
    }

}