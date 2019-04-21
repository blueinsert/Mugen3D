using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D
{
    [RequireComponent(typeof(Animation))]
    public class HelperView : UnitView
    {
        private Helper m_helper;

        public void Init(Helper h)
        {
            base.Init(h);
            this.m_helper = h;
        }

        public override void Update()
        {
            base.Update();
        }

    }

}
