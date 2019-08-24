using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.Core
{
    public class MatchSystem : SystemBase
    {
        public MatchSystem(WorldBase world) : base(world) { }

        protected override bool Filter(Entity e)
        {
            return base.Filter(e);
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            base.ProcessEntity(entities);
        }

        private bool IsCharactersReady()
        {
            //return m_p1.fsmMgr.stateNo == 0 && m_p2.fsmMgr.stateNo == 0;
            return false;
        }

        private bool IsCharactersSteady()
        {
            // return (!m_p1.IsAlive() || (m_p1.IsAlive() && m_p1.fsmMgr.stateNo == 0)) && (!m_p2.IsAlive() || (m_p2.IsAlive() && m_p2.fsmMgr.stateNo == 0));
            return false;
        }

        private bool IsRoundEnd()
        {
            /*
            if (!m_p1.IsAlive() || !m_p2.IsAlive())
                return true;
            if (m_roundStateTimer <= 0)
                return true;
            return false;
            */
            return false;
        }
    }
}
