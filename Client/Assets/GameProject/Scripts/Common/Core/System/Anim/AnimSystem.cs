using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class AnimSystem : SystemBase
    {

        protected override bool Filter(Entity e)
        {
            return e.GetComponent<AnimationComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            foreach(var entity in entities)
            {
                var animation = entity.GetComponent<AnimationComponent>();
                UpdateSample(animation);
            }
        }

        private void UpdateSample(AnimationComponent animation)
        {
            animation.UpdateSample();
        }

    }
}