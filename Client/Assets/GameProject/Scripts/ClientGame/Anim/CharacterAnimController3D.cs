using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public class CharacterAnimController3D : ICharacterAnimController
    {
        private Animation m_animation;

        public void Init(Animation animation)
        {
            m_animation = animation;
            foreach (AnimationState state in this.m_animation)
            {
                state.enabled = false;
            }
        }

        public void UpdateAnimSample(string animName, int frameNo)
        {
            //do nothing
        }

        public void UpdateAnimSample(string animName, float normalizedTime)
        {
            m_animation[animName].enabled = true;
            m_animation[animName].normalizedTime = normalizedTime;
            m_animation[animName].weight = 1;
            m_animation.Sample();
            m_animation[animName].enabled = false;
        }
    }

}
