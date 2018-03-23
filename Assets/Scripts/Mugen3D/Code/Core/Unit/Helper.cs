using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Helper : Unit
    {
        public Player master;

        public override void Init()
        {
            base.Init();
            moveCtr = new HelperMoveCtrl(this);
            animCtr = new AnimationController(this.GetComponent<Animation>(), this);
        }

        public override void OnUpdate()
        {
            moveCtr.Update();
            animCtr.Update();
        }

    }
}
