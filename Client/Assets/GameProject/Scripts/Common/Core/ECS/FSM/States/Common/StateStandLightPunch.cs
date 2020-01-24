using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateStandLightPunch:StateBase
    {
        public StateStandLightPunch(Entity e) : base(e) { }

        public override void OnEnter()
        {
            PhysicsSet(PhysicsType.Stand);
            CtrlSet(false);
            ChangeAnim(200);
            MoveTypeSet(MoveType.Attack);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (LeftAnimTime <= 0)
            {
                ChangeState(StateConst.StateNo_Stand);
            }
            var hitData = HitData;
            hitData.hitPauseTime = new int[] { 9, 18 };
            hitData.hitSlideTime = 16;
            HitData.groundVel = new FixPointMath.Number[] { -4, 0 };
        }
    }
}
