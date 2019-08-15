using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class ScriptSystem : SystemBase
    {
        private List<FsmManager> m_fsms = new List<FsmManager>();

        public ScriptSystem(BattleWorld world) : base(world)
        {
       
        }

        protected override void OnAddEntity(Entity e)
        {
            if (e is Unit)
            {
                var u = e as Unit;
                m_fsms.Add(u.fsmMgr);
            }
        }

        protected override void OnRemoveEntity(Entity e)
        {
            if (e is Unit)
            {
                var u = e as Unit;
                m_fsms.Remove(u.fsmMgr);
            }
        }

        public override void Update()
        {
            foreach(var fsm in m_fsms)
            {
                fsm.Update();
            }
        }

        public void PreUpdate()
        {
            foreach (var fsm in m_fsms)
            {
                fsm.ProcessChangeState();
            }
        }
    }
}
