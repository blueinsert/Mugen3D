using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class ParticleEffectObj : EffectObj
    {
        public ParticleSystem particle;

        public override void Play()
        {
            base.Play();
            particle.Play(true);
        }

        public void Update()
        {
            if(isAlive && particle != null && !particle.IsAlive(true))
            {
                isAlive = false;
                OnFinish();
            }
        }

    }
}