using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class GuardSystem : SystemBase
    {
        public GuardSystem(WorldBase world) : base(world) { }

        protected override bool Filter(Entity e)
        {
            return e.GetComponent<FSMComponent>() != null && e.GetComponent<BasicInfoComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            foreach(var e in entities)
            {
                var basic = e.GetComponent<BasicInfoComponent>();
                if (!basic.Ctrl)
                    continue;
                var command = e.GetComponent<CommandComponent>();
                if(command.CommandIsActive("holdback") && UtilityFuncs.GetP2MoveType(e) == MoveType.Attack)
                {
                    var fsm = e.GetComponent<FSMComponent>();
                    if (fsm.StateNo != StateConst.StateNo_GuardStart)
                        fsm.ChangeState(StateConst.StateNo_GuardStart);
                }
            }
        }
    }
}
