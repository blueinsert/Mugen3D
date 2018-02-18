using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class HelperMoveCtrl : MoveCtrl
    {
        public HelperMoveCtrl(Unit u): base(u)
        {

        }

        public override void Update()
        {
            base.Update();
        }

        protected override void HandleHitUp(RaycastHit hit) { }
        protected override void HandleHitBelow(RaycastHit hit) { }
        protected override void HandleHitLeft(RaycastHit hit) { }
        protected override void HandleHitRight(RaycastHit hit) { }

    }
}