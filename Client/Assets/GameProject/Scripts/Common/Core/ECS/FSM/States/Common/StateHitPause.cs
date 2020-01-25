using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateHitPause:StateBase
    {
        int m_pauseTime = 0;

        public StateHitPause(Entity e) : base(e) { }

        public override void OnEnter()
        {
            base.OnEnter();
            var hitData = HitData;
            VelSet(0, 0);
            if (MoveContact)
            {
                if (MoveHit)
                {
                    m_pauseTime = hitData.hitPauseTime[0];
                }
                else
                {
                    m_pauseTime = hitData.guardPauseTime[0];
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            ChangeAnim(Anim, AnimElem);
            if(StateTime >= m_pauseTime)
            {
                PopStateLayer();
                HitSystem.ProcessCornerPush(m_entity);
            }
        }
    }
}
