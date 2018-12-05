using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class SinglePlay : MatchManager
    {
        public SinglePlay(World world, MatchInfo info) : base(world, info)
        {
        }

        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
        }
    }

}