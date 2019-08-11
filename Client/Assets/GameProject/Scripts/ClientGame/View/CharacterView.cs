using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.Mugen3D.Core;
using FixPointMath;

namespace bluebean.Mugen3D.ClientGame
{
    [RequireComponent(typeof(Animation))]
    public class CharacterView : UnitView
    {
        private Character m_char;     

        public void Init(Character c)
        {
            base.Init(c);
            this.m_char = c;
            this.GetComponentInChildren<Renderer>().sortingOrder = 1;
        }

        public override void Update()
        {
            base.Update();
        }

    }

}
