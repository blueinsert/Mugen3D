using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class MoveSystem : SystemBase
    {
        public MoveSystem(WorldBase world) : base(world) { }

        protected override bool Filter(Entity e)
        {
            return e.GetComponent<MoveComponent>() != null && e.GetComponent<TransformComponent>()!=null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            foreach(var e in entities)
            {
                var move = e.GetComponent<MoveComponent>();
                var transform = e.GetComponent<TransformComponent>();
                var velocity = move.Velocity + Number.D60 * move.Acceler;
                var deltaPos = velocity * Number.D60;
                deltaPos.x *= transform.Facing;
                transform.PosAdd(deltaPos);
                move.VelSet(velocity);
            }
        }
    }
}
