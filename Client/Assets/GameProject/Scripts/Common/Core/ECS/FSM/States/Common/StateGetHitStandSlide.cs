using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateGetHitStandSlide:StateBase
    {
        public StateGetHitStandSlide(Entity e) : base(e) { }

        public override void OnEnter()
        {
            PhysicsSet(PhysicsType.Stand);
            CtrlSet(false);
            ChangeAnim(5000);
            var beHitData = BeHitData;
            VelSet(beHitData.groundVel.x,beHitData.groundVel.y);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            if (LeftAnimTime <= 0)
            {
                ChangeAnim(5005);
            }
            if (StateTime >= HitSlideTime)
            {
                ChangeState(0);
            }
        }
    }
}
