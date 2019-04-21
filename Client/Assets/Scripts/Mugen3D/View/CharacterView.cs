using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D
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
