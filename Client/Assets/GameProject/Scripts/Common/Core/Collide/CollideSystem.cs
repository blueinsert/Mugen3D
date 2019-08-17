using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class CollideSystem : SystemBase
    {
        protected override bool Filter(Entity e)
        {
            return e.GetComponent<CollideComponent>() != null && e.GetComponent<AnimationComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            List<CollideComponent> collideComponents = new List<CollideComponent>();
            foreach(var entity in entities)
            {
                var animationComponent = entity.GetComponent<AnimationComponent>();
                var collideComponent = entity.GetComponent<CollideComponent>();
                var moveComponent = entity.GetComponent<MoveComponent>();
                collideComponent.Update(animationComponent.CurActionFrame.clsns, moveComponent != null ? moveComponent.Positon : Vector.zero, moveComponent != null ? moveComponent.Facing : -1);
                collideComponents.Add(collideComponent);
            }
            for (int i = 0; i < collideComponents.Count; i++)
            {
                var m = m_moveCtrls[i];
                var pos = m.position;
                //
                Rect screenBound = world.cameraController.viewPort;
                Number playerWidth = new Number(5) / new Number(10);
                pos.x = Math.Clamp(pos.x, screenBound.xMin + playerWidth, screenBound.xMax - playerWidth);
                //
                pos.x = Math.Clamp(pos.x, world.config.stageConfig.borderXMin, world.config.stageConfig.borderXMax);
                pos.y = Math.Clamp(pos.y, world.config.stageConfig.borderYMin, world.config.stageConfig.borderYMax);
                m.PosSet(pos);

                for (int j = i + 1; j < collideComponents.Count; j++)
                {
                    var m2 = m_moveCtrls[j];
                    if (m != m2)
                    {
                        ContactInfo contactInfo;
                        if (m.collider.IsIntersect(m2.collider, out contactInfo))
                        {
                            m.PosAdd(contactInfo.recoverDir * contactInfo.depth * (m2.mass) / (m.mass + m2.mass));
                            m2.PosAdd(-contactInfo.recoverDir * contactInfo.depth * (m.mass) / (m.mass + m2.mass));
                        }
                    }
                }
            }
    }
}
