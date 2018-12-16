using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class PhysicsEngine 
    {
        private World m_world;
        private List<MoveCtrl> m_moveCtrls = new List<MoveCtrl>();

        public PhysicsEngine(World world)
        {
            m_world = world;
            world.onAddEntity += OnAddEntity;
            world.onRemoveEntity += OnRemoveEntity;
        }

        private void OnAddEntity(Entity e)
        {
            if(e is Unit)
            {
                var u = e as Unit;
                m_moveCtrls.Add(u.moveCtr);
            }
        }

        private void OnRemoveEntity(Entity e)
        {
            if (e is Unit)
            {
                var u = e as Unit;
                m_moveCtrls.Remove(u.moveCtr);
            }
        }

        public void Update()
        {
            foreach(var m in m_moveCtrls)
            {
                var posBefore = m.position;
                m.Update();
                m.collider.SetCollider()
                if(posBefore.y > m_world.config.stageConfig.borderYMin && m.position.y < m_world.config.stageConfig.borderYMin)
                {
                    m.justOnGround = true;
                }
            }
            while (HasIntersection())
            {
                foreach (var m in m_moveCtrls)
                {
                    if (m.IntersectWithScreenBound())
                    {
                        Rect screenBound = m_world.cameraController.viewPort;
                        Number playerWidth = new Number(5) / new Number(10);
                        m.position.x = Math.Clamp(m.position.x, screenBound.xMin + playerWidth, screenBound.xMax - playerWidth);
                    }
                    if (m.IntersectWithWorldBound())
                    {
                        m.position.x = Math.Clamp(m.position.x, m_world.config.stageConfig.borderXMin, m_world.config.stageConfig.borderXMax);
                        m.position.y = Math.Clamp(m.position.y, m_world.config.stageConfig.borderYMin, m_world.config.stageConfig.borderYMax);
                    }
                    foreach(var m2 in m_moveCtrls)
                    {
                        if (m != m2) {
                            ContactInfo contactInfo;
                            if (m.collider.IsIntersect(m2.collider, out contactInfo))
                            {
                                m.position += contactInfo.recoverDir * contactInfo.depth;
                            }
                        }
                        
                    }
                }
            }
            foreach(var m in m_moveCtrls)
            {
                m.ApplyPosition();
            }
        }

        private bool HasIntersection()
        {
            bool res = false;
            foreach (var m1 in m_moveCtrls)
            {
                if (m1.IntersectWithWorldBound())
                    return true;
                if (m1.IntersectWithScreenBound())
                    return true;
                foreach (var m2 in m_moveCtrls)
                {
                    if (m1 != m2)
                    {
                        ContactInfo contactInfo;
                        if (m1.collider.IsIntersect(m2.collider, out contactInfo))
                            return true;
                    }
                }
            }
            return res;
        }
    }
}
