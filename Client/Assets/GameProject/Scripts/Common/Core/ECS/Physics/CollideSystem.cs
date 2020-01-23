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
            return e.GetComponent<CollideComponent>() != null && e.GetComponent<TransformComponent>()!=null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            
            //碰撞检测和决议
            //从动画数据中复制判定框到碰撞组件
            foreach (var entity in entities)
            {
                var animationComponent = entity.GetComponent<AnimationComponent>();
                var collideComponent = entity.GetComponent<CollideComponent>();
                var transform = entity.GetComponent<TransformComponent>();
                if (animationComponent == null)
                {
                    collideComponent.Update(null, transform.Position, transform.Facing);
                }
                else
                {
                    collideComponent.Update(animationComponent.CurActionFrame.clsns, transform.Position, transform.Facing);
                }
            }
            
            //碰撞求解，得到相互不穿叉的位置
            int loopMax = 1;
            for (int loop = 0; loop < loopMax; loop++)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    var entity1 = entities[i];
                    var transform1 = entity1.GetComponent<TransformComponent>();
                    var collideComponent1 = entity1.GetComponent<CollideComponent>();
                    var physics1 = entity1.GetComponent<PhysicsComponent>();
                    var mass1 = physics1.Mass;

                    for (int j = i + 1; j < entities.Count; j++)
                    {
                        var entity2 = entities[j];
                        var transform2 = entity2.GetComponent<TransformComponent>();
                        var physics2 = entity2.GetComponent<PhysicsComponent>();
                        var mass2 = physics2.Mass;
                        var collideComponent2 = entity2.GetComponent<CollideComponent>();

                        ContactInfo contactInfo;
                        if (PhysicsUtils.ComplexColliderIntersectTest(collideComponent1.Collider, collideComponent2.Collider, out contactInfo))
                        {
                            transform1.PosAdd(contactInfo.recoverDir * contactInfo.depth * (mass2) / (mass1 + mass2));
                            transform2.PosAdd(-contactInfo.recoverDir * contactInfo.depth * (mass1) / (mass1 + mass2));
                        }
                    }
                }

            }
            
            for (int i = 0; i < entities.Count; i++)
            {
                var entity1 = entities[i];
                var transform = entity1.GetComponent<TransformComponent>();

                var pos = transform.Position;
                //摄像机视口限制
                var cameraComponent = m_world.GetSingletonComponent<CameraComponent>();
                if (cameraComponent != null)
                {
                    var viewPort = cameraComponent.ViewPort;
                    Number playerWidth = new Number(5) / new Number(10);
                    pos.x = Math.Clamp(pos.x, viewPort.XMin + playerWidth, viewPort.XMax - playerWidth);
                }
                //舞台限制
                var stageComponent = m_world.GetSingletonComponent<StageComponent>();
                if (stageComponent != null)
                {
                    pos.x = Math.Clamp(pos.x, stageComponent.BorderXMin, stageComponent.BorderXMax);
                    pos.y = Math.Clamp(pos.y, stageComponent.BorderYMin, stageComponent.BorderYMax);
                }
                transform.PosSet(pos.x, pos.y);
            }
            
        }
    }
}
