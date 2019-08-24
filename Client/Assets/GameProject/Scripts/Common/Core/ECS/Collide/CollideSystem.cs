using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class CollideSystem : SystemBase
    {
        public CollideSystem(WorldBase world) : base(world) { }

        protected override bool Filter(Entity e)
        {
            return e.GetComponent<CollideComponent>() != null && e.GetComponent<AnimationComponent>() != null && e.GetComponent<MoveComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            //更新collideComponent
            foreach (var entity in entities)
            {
                var animationComponent = entity.GetComponent<AnimationComponent>();
                var collideComponent = entity.GetComponent<CollideComponent>();
                var moveComponent = entity.GetComponent<MoveComponent>();
                collideComponent.Update(animationComponent.CurActionFrame.clsns, moveComponent != null ? moveComponent.Position : Vector.zero, moveComponent != null ? moveComponent.Facing : -1);
            }
            //碰撞求解，得到相互不穿叉的位置
            int loopMax = 1;
            for (int loop = 0; loop < loopMax; loop++)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    var entity1 = entities[i];
                    var moveComponent1 = entity1.GetComponent<MoveComponent>();
                    var collideComponent1 = entity1.GetComponent<CollideComponent>();
                    var mass1 = moveComponent1.Mass;
                    for (int j = i + 1; j < entities.Count; j++)
                    {
                        var entity2 = entities[j];
                        var moveComponent2 = entity2.GetComponent<MoveComponent>();
                        var mass2 = moveComponent2.Mass;
                        var collideComponent2 = entity1.GetComponent<CollideComponent>();
                        ContactInfo contactInfo;
                        if (collideComponent1.Collider.IsIntersect(collideComponent2.Collider, out contactInfo))
                        {
                            moveComponent1.PosAdd(contactInfo.recoverDir * contactInfo.depth * (mass2) / (mass1 + mass2));
                            moveComponent2.PosAdd(-contactInfo.recoverDir * contactInfo.depth * (mass1) / (mass1 + mass2));
                        }
                    }
                }

            }
            for (int i = 0; i < entities.Count; i++)
            {
                var entity1 = entities[i];
                var moveComponent1 = entity1.GetComponent<MoveComponent>();
                var pos = moveComponent1.Position;
                //摄像机视口限制
                var cameraComponent = CameraComponent.Instance;
                if (cameraComponent != null)
                {
                    var viewPort = cameraComponent.ViewPort;
                    Number playerWidth = new Number(5) / new Number(10);
                    pos.x = Math.Clamp(pos.x, viewPort.XMin + playerWidth, viewPort.XMax - playerWidth);
                }
                //舞台限制
                var stageComponent = StageComponent.Instance;
                if (stageComponent != null)
                {
                    pos.x = Math.Clamp(pos.x, stageComponent.BorderXMin, stageComponent.BorderXMax);
                    pos.y = Math.Clamp(pos.y, stageComponent.BorderYMin, stageComponent.BorderYMax);
                }
                moveComponent1.PosSet(pos.x, pos.y);
            }
        }
    }
}
