using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.Mugen3D.Core;

namespace Mugen3D.Tools
{

    [RequireComponent(typeof(Animation))]
    public class AnimationController : MonoBehaviour
    {
        public Animation anim { get { return m_anim; } }
        private Animation m_anim;
        private ActionDef action;

        public void Init()
        {
            m_anim = this.GetComponent<Animation>();
            foreach (AnimationState state in this.m_anim)
            {
                state.enabled = false;
            }
        }

        public void Update()
        {
        
        }

        public void Sample(string animName, float normalizeTime)
        {
            m_anim[animName].enabled = true;
            m_anim[animName].normalizedTime = normalizeTime;
            m_anim[animName].weight = 1;
            m_anim.Sample();
            m_anim[animName].enabled = false;
        }

    }

}
