using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D.Tools
{
    enum AnimState
    {
        Playing,
        Stop,
    }
    
    public class ActionsEditorAnimController
    {
        public Animation m_anim;

        protected Mugen3D.Action action;
        public int animTime;
        public int animElem;
        public int animElemTime;

        private AnimState state = AnimState.Stop;

        private void Init()
        {
            foreach (AnimationState state in this.m_anim)
            {
                state.enabled = false;
            }
        }

        public ActionsEditorAnimController(Animation anim)
        {
            this.m_anim = anim;  
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
            Sample(action.animName, action.frames[animElem].normalizeTime);
        }

        public void Sample(string nameName, float normalizeTime)
        {
            m_anim[nameName].enabled = true;
            m_anim[nameName].normalizedTime = normalizeTime;
            m_anim[nameName].weight = 1;
            m_anim.Sample();
            m_anim[nameName].enabled = false;
        }

        public void Play(Mugen3D.Action action)
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
