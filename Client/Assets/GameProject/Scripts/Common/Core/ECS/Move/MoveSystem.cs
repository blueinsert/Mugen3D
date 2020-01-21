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
            return e.GetComponent<MoveComponent>() != null && e.GetComponent<CommandComponent>()!=null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            foreach(var e in entities)
            {
                var moveComponent = e.GetComponent<MoveComponent>();
                //测试代码
                var command = e.GetComponent<CommandComponent>();
                if (command.CommandIsActive("holdfwd"))
                {
                    moveComponent.VelSet(2, 0);
                }else if (command.CommandIsActive("holdback"))
                {
                    moveComponent.VelSet(-2, 0);
                }
                else
                {
                    moveComponent.VelSet(0, 0);
                }
                moveComponent.Update(Number.D60);
            }
        }
    }
}
