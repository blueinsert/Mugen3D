using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 自动转向系统
    /// </summary>
    class AutoTurnSystem : SystemBase
    {
        public AutoTurnSystem(WorldBase world) : base(world) { }

        protected override bool Filter(Entity e)
        {
            return e.GetComponent<MoveComponent>() != null && e.GetComponent<BasicInfoComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            Vector center = Vector.zero;
            foreach(var e in entities)
            {
                var transform = e.GetComponent<TransformComponent>();
                center += transform.Position;
            }
            center /= entities.Count;

            foreach (var e in entities)
            {
                var transform = e.GetComponent<TransformComponent>();
                var basic = e.GetComponent<BasicInfoComponent>();
                int facing = transform.Position.x < center.x ? 1 : -1;
                if (transform.Facing != facing&&basic.Ctrl)
                {
                    transform.SetFacing(facing);
                }
            }
        }
    }
}
