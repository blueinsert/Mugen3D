using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.Mugen3D.Core;

namespace Mugen3D.Tools
{
    enum AnimState
    {
        Playing,
        Stop,
    }
    
    [RequireComponent(typeof(Animation))]
    public class AnimController : MonoBehaviour
    {
        public Animation anim { get { return m_anim; } }
        private Animation m_anim;
        private ActionDef action;
        private int animTime;
        private int animElem;
        private int animElemTime;
        private AnimState state = AnimState.Stop;

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
            if(state == AnimState.Playing)
                UpdateSample();
        }

        private void UpdateSample()
        {
            animTime++;
            animElemTime++;
            var animElemDuration = action.frames[animElem].duration;
            if (animElemTime > animElemDuration)
            {
                if (animElem >= action.frames.Count - 1 && action.loopStartIndex != -1)
                {
                    animElem = action.loopStartIndex;
                    animElemTime = 0;
                }
                else if (animElem >= action.frames.Count - 1 && action.loopStartIndex == -1)
                {
                    //do nothing
                }
                else
                {
                    animElem++;
                    animElemTime = 0;
                }
            }
            Sample(action.animName, action.frames[animElem].normalizeTime.AsFloat());
        }

        public void Sample(string animName, float normalizeTime)
        {
            m_anim[animName].enabled = true;
            m_anim[animName].normalizedTime = normalizeTime;
            m_anim[animName].weight = 1;
            m_anim.Sample();
            m_anim[animName].enabled = false;
        }

        public void Play(ActionDef action)
        {
            this.action = action;
            state = AnimState.Playing;
            animElem = 0;
            animElemTime = 0;
            animTime = 0;
        }

        public void Stop()
        {
            this.state = AnimState.Stop;
        }

    }

}
