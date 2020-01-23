using System;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 负责约束物体的速度，加速度，避免相互穿越等情况，使之呈现物理合理性
    /// </summary>
    class PhysicsSystem : SystemBase
    {

        public PhysicsSystem(WorldBase world) : base(world) { }
        protected override bool Filter(Entity e)
        {
            return e.GetComponent<MoveComponent>() != null && e.GetComponent<PhysicsComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            //更新物体加速度
            foreach (var entity in entities)
            {
                var physics = entity.GetComponent<PhysicsComponent>();
                var move = entity.GetComponent<MoveComponent>();
                var curVel = move.Velocity;

                Vector acceler = Vector.zero;
                switch (physics.PhysicsType)
                {
                    case PhysicsType.Air:
                        acceler = new Vector(0, PhysicsComponent.G) + physics.ExternalForce / physics.Mass;
                        break;
                    case PhysicsType.Stand:
                    case PhysicsType.Crouch:
                        acceler = (Number.Abs(PhysicsComponent.G) * physics.Mass + physics.ExternalForce.y) / physics.Mass * PhysicsComponent.Friction * (-curVel.normalized) + physics.ExternalForce / physics.Mass;
                        break;
                    case PhysicsType.None:
                        break;
                }
                move.AccelerateSet(acceler);
            }
  
        }
    }
}
