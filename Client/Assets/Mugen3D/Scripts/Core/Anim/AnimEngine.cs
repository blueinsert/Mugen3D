using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class AnimEngine : BaseEngine
    {
        private List<AnimationController> m_animCtls = new List<AnimationController>();

        public AnimEngine(World world) : base(world)
        {
    
        }

        protected override void OnAddEntity(Entity e)
        {
            if (e is Unit)
            {
                var u = e as Unit;
                m_animCtls.Add(u.animCtr);
            }
        }

        protected override void OnRemoveEntity(Entity e)
        {
            if (e is Unit)
            {
                var u = e as Unit;
                m_animCtls.Remove(u.animCtr);
            }
        }

        public override void Update()
        {
            foreach(var animCtl in m_animCtls)
            {
                if (!animCtl.owner.IsPause())
                {
                    animCtl.Update();
                    animCtl.owner.moveCtr.collider.SetCollider(animCtl.curActionFrame.clsns);
                }   
            }
        }
    }
}