using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D
{
    [RequireComponent(typeof(Animation))]
    public class CharView : EntityView
    {
        private Animation m_anim;
        private Character m_char;
        private AnimationController m_animCtl;

        private Vector lastPosition;
        private Vector lastScale;

        public void Awake()
        {
            m_anim = this.GetComponent<Animation>();
            foreach (AnimationState state in this.m_anim)
            {
                state.enabled = false;
            }
        }

        public void Init(Character c)
        {
            this.m_char = c;
            this.m_animCtl = c.animCtr;
            m_char.onEvent += (entity, e) => {
                if (e.type == Core.EventType.SampleAnim)
                {
                    SampleAnim();
                }
            };
        }

        void SampleAnim()
        {
            m_anim[m_animCtl.curAction.animName].enabled = true;
            m_anim[m_animCtl.curAction.animName].normalizedTime = m_animCtl.curAction.frames[m_animCtl.animElem].normalizeTime.AsFloat();
            m_anim[m_animCtl.curAction.animName].weight = 1;
            m_anim.Sample();
            m_anim[m_animCtl.curAction.animName].enabled = false;
        }

        void Update()
        {
            DebugDraw();
            if (lastPosition != m_char.position)
            {
                lastPosition = m_char.position;
                this.transform.position = new Vector3(lastPosition.x.AsFloat(), lastPosition.y.AsFloat(), 0);
            }
            if (!lastScale.Equals(m_char.scale))
            {
                lastScale = new Vector(m_char.scale.x, m_char.scale.y);
                UnityEngine.Debug.Log("lastScale:" + lastScale.ToString());
                this.transform.localScale = new Vector3(lastScale.x.AsFloat(), lastScale.y.AsFloat(), lastScale.y.AsFloat());
            }
        }

        private void DebugDraw()
        {
            if (m_animCtl.m_actions.ContainsKey(m_animCtl.anim))
            {
                var curAction = m_animCtl.m_actions[m_animCtl.anim];
                if (curAction.frames.Count != 0)
                {
                    var curActionFrame = curAction.frames[m_animCtl.animElem];
                    foreach (var clsn in curActionFrame.clsns)
                    {
                        var pos = m_char.position;
                        Core.Rect rect = new Core.Rect(new Vector(clsn.x1, clsn.y1) + pos, new Vector(clsn.x2, clsn.y2) + pos);
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

}
