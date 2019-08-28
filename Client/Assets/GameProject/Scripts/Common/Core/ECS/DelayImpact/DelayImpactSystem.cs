using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class DelayImpactSystem : SystemBase
    {
        public DelayImpactSystem(WorldBase world) : base(world)
        {

        }

        protected override bool Filter(Entity e)
        {
            return e.GetComponent<DelayImpactComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            foreach(var entity in entities)
            {
                DelayImpactComponent delayImpactComponent = entity.GetComponent<DelayImpactComponent>();
                if (delayImpactComponent.NewStateNo != -1)
                {
                    var fsmComponent = entity.GetComponent<FSMComponent>();
                    if (fsmComponent != null)
                    {
                        fsmComponent.ChangeState(delayImpactComponent.NewStateNo);
                    }
                }
                //todo more
                delayImpactComponent.Clear();
            }
        }
    }
}
