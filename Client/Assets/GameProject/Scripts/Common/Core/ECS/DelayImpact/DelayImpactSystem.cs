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
                var moveComponent = entity.GetComponent<MoveComponent>();
                if (delayImpactComponent.NewFacing != 0)
                {
                    if (moveComponent != null)
                    {
                        moveComponent.ChangeFacing(delayImpactComponent.NewFacing);
                    }
                }
                if (delayImpactComponent.NewAnimNo != -1)
                {
                    var animComponent = entity.GetComponent<AnimationComponent>();
                    if (animComponent != null)
                    {
                        animComponent.ChangeAnim(delayImpactComponent.NewAnimNo);
                    }
                }
                if (delayImpactComponent.NewVel != null)
                {
                    if (moveComponent != null)
                    {
                        moveComponent.VelSet(delayImpactComponent.NewVel.Value);
                    }
                }
                if (delayImpactComponent.NewVelDelta != null)
                {
                    if (moveComponent != null)
                    {
                        moveComponent.VelAdd(delayImpactComponent.NewVelDelta.Value);
                    }
                }
                if (delayImpactComponent.NewPos != null)
                {
                    if (moveComponent != null)
                    {
                        moveComponent.VelAdd(delayImpactComponent.NewPos.Value);
                    }
                }
                if (delayImpactComponent.NewPosDelta != null)
                {
                    if (moveComponent != null)
                    {
                        moveComponent.VelAdd(delayImpactComponent.NewPosDelta.Value);
                    }
                }
                //todo more
                delayImpactComponent.Clear();
            }
        }
    }
}
