using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class CameraSystem : SystemBase
    {
        protected override bool Filter(Entity e)
        {
            return e.GetComponent<PlayerComponent>() != null && e.GetComponent<MoveComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            List<Vector> posArray = new List<Vector>();
            foreach(var entity in entities)
            {
                var moveComponent = entity.GetComponent<MoveComponent>();
                posArray.Add(moveComponent.Positon);
            }
            var cameraComponent = CameraComponent.Instance;
            if (cameraComponent != null)
            {
                cameraComponent.Update(posArray.ToArray());
            }
        }
    }
}
