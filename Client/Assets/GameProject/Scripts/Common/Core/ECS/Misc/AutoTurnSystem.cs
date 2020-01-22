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
                var move = e.GetComponent<MoveComponent>();
                center += move.Position;
            }
            center /= entities.Count;

            foreach (var e in entities)
            {
                var move = e.GetComponent<MoveComponent>();
                var basic = e.GetComponent<BasicInfoComponent>();
                int facing = move.Position.x < center.x ? 1 : -1;
                if (basic.Facing != facing&&basic.Ctrl)
                {
                    basic.SetFacing(facing);
                }
            }
        }
    }
}
