using System.Collections;
using System.Collections.Generic;
using bluebean.Mugen3D.Core;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public class ParticleEffectObj : EffectObj
    {
        public ParticleSystem particle;

        public override bool IsAlive()
        {
            return particle.IsAlive(true);
        }

        public override void Init(int id, EffectDef def, UnitView owner)
        {
            base.Init(id, def, owner);
        }

        public override void Play()
        {
            base.Play();
            particle.Play(true);
        }

        public override void Update()
        {
            base.Update();
        }

    }
}