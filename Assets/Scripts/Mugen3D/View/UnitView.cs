using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;

namespace Mugen3D
{
    public class UnitView : EntityView
    {
        private Animation m_anim;
        private Unit m_unit;
        private AnimationController m_animCtl;
        private Vector lastPosition;
        private Vector lastScale;

        public void Init(Unit u)
        {
            base.Init(u);
            m_anim = this.GetComponent<Animation>();
            foreach (AnimationState state in this.m_anim)
            {
                state.enabled = false;
            }
            this.m_unit = u;
            this.m_animCtl = u.animCtr;  
        }

        void SampleAnim()
        {
            m_anim[m_animCtl.curAction.animName].enabled = true;
            m_anim[m_animCtl.curAction.animName].normalizedTime = m_animCtl.curAction.frames[m_animCtl.animElem].normalizeTime.AsFloat();
            m_anim[m_animCtl.curAction.animName].weight = 1;
            m_anim.Sample();
            m_anim[m_animCtl.curAction.animName].enabled = false;
        }

        public override void Update()
        {
            DebugDraw();
            if (lastPosition != m_unit.position)
            {
                lastPosition = m_unit.position;
                this.transform.position = new Vector3(lastPosition.x.AsFloat(), lastPosition.y.AsFloat(), 0);
            }
            if (!lastScale.Equals(m_unit.scale))
            {
                lastScale = new Vector(m_unit.scale.x, m_unit.scale.y, m_unit.scale.z);
                this.transform.localScale = new Vector3(lastScale.x.AsFloat(), lastScale.y.AsFloat(), lastScale.z.AsFloat());
            }
            SampleAnim();
        }

        private void DebugDraw()
        {
            var curAction = m_animCtl.curAction;
            if (curAction.frames.Count != 0)
            {
                var curActionFrame = curAction.frames[m_animCtl.animElem];
                foreach (var clsn in curActionFrame.clsns)
                {
                    var pos = m_unit.position;
                    Core.Rect rect = new Core.Rect(new Vector(clsn.x1 * m_unit.GetFacing(), clsn.y1, 0) + pos, new Vector(clsn.x2 * m_unit.GetFacing(), clsn.y2, 0) + pos);
                    Color c = Color.blue;
                    if (clsn.type == 1)
                    {
                        c = Color.blue;
                    }
                    else if (clsn.type == 2)
                    {
                        c = Color.red;
                    }
                    Log.DrawRect(rect.LeftUp.ToVector2(), rect.RightUp.ToVector2(), rect.RightDown.ToVector2(), rect.LeftDown.ToVector2(), c, UnityEngine.Time.deltaTime);
                }
            }
        }

    }
}
