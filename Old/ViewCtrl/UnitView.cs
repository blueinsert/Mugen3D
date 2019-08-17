using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.Mugen3D.Core;
using bluebean.UGFramework.Log;
using FixPointMath;

namespace bluebean.Mugen3D.ClientGame
{
    public class UnitView : EntityView
    {
        private Animation m_anim;
        private Unit m_unit;
        public Unit unit { get { return m_unit; } }
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
            u.onEvent += ProcessEvent;
        }

        private void ProcessEvent(Core.Event evt) {
            switch(evt.type)
            {
                case Core.EventType.SampleAnim:
                    SampleAnim();break;
                case Core.EventType.PlayEffect:
                    PlayEffect(evt.data as EffectDef);
                    break;
                case Core.EventType.PlaySound:
                    PlaySound(evt.data as SoundDef);
                    break;
            }
        }

        void PlayEffect(Core.EffectDef effect)
        {
            //EffectPool.Instance.Play(effect, this);
        }

        void PlaySound(Core.SoundDef sound)
        {
            //SoundPlayer.Instance.Play(sound.name, sound.delay.AsFloat(), sound.volume.AsFloat());
        }

        void SampleAnim()
        {
            m_anim[m_animCtl.curAction.animName].enabled = true;
            m_anim[m_animCtl.curAction.animName].normalizedTime = m_animCtl.curActionFrame.normalizeTime.AsFloat();
            m_anim[m_animCtl.curAction.animName].weight = 1;
            m_anim.Sample();
            m_anim[m_animCtl.curAction.animName].enabled = false;
        }
  
        private void DebugDraw()
        { 
            var collider = m_unit.moveCtr.collider;
            for(int i = 0; i < collider.attackClsnsLength; i++)
            {
                var clsn = collider.attackClsns[i];
                bluebean.UGFramework.Log.Debug.DrawRect(clsn.LeftUp.ToVector2(), clsn.RightUp.ToVector2(), clsn.RightDown.ToVector2(), clsn.LeftDown.ToVector2(), Color.red, UnityEngine.Time.deltaTime);
            }
            for (int i = 0; i < collider.defenceClsnsLength; i++)
            {
                var clsn = collider.defenceClsns[i];
                bluebean.UGFramework.Log.Debug.DrawRect(clsn.LeftUp.ToVector2(), clsn.RightUp.ToVector2(), clsn.RightDown.ToVector2(), clsn.LeftDown.ToVector2(), Color.blue, UnityEngine.Time.deltaTime);
            }
            for (int i = 0; i < collider.collideClsnsLength; i++)
            {
                var clsn = collider.collideClsns[i];
                bluebean.UGFramework.Log.Debug.DrawRect(clsn.LeftUp.ToVector2(), clsn.RightUp.ToVector2(), clsn.RightDown.ToVector2(), clsn.LeftDown.ToVector2(), Color.black, UnityEngine.Time.deltaTime);
            }
        }

        public override void Update()
        {
            if (lastPosition != m_unit.Position)
            {
                lastPosition = m_unit.Position;
                this.transform.position = new Vector3(lastPosition.x.AsFloat(), lastPosition.y.AsFloat());
            }
            if (!lastScale.Equals(m_unit.scale))
            {
                lastScale = new Vector(m_unit.scale.x, m_unit.scale.y);
                this.transform.localScale = new Vector3(lastScale.x.AsFloat(), lastScale.y.AsFloat(),1);
            }
            DebugDraw();
        }

    }
}
