using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class PhysicsEngine : BaseEngine
    {
        private List<MoveCtrl> m_moveCtrls = new List<MoveCtrl>();

        public PhysicsEngine(BattleWorld world):base(world)
        {
   
        }

        protected override void OnAddEntity(Entity e)
        {
            if(e is Unit)
            {
                var u = e as Unit;
                m_moveCtrls.Add(u.moveCtr);
            }
        }

        protected override void OnRemoveEntity(Entity e)
        {
            if (e is Unit)
            {
                var u = e as Unit;
                m_moveCtrls.Remove(u.moveCtr);
            }
        }

        public override void Update()
        {
            //update dynamic
            foreach(var m in m_moveCtrls)
            {
                m.Update();
            }
            //collide solve
            for(int loop = 0; loop < 1; loop++)
            {
                for(int i = 0; i< m_moveCtrls.Count;i++)
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

                    for (int j = i + 1; j < m_moveCtrls.Count; j++)
                    {
                        var m2 = m_moveCtrls[j];
                        if (m != m2) {
                            ContactInfo contactInfo;
                            if (m.collider.IsIntersect(m2.collider, out contactInfo))
                            {
                                m.PosAdd(contactInfo.recoverDir * contactInfo.depth*(m2.mass)/(m.mass + m2.mass));
                                m2.PosAdd(-contactInfo.recoverDir * contactInfo.depth * (m.mass) / (m.mass + m2.mass));
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
       
    }
}
