using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.Mugen3D.Core;
using FixPointMath;

namespace bluebean.Mugen3D.ClientGame
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
