using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixPointMath;

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

            HitDefData hitData = new HitDefData();
            hitData.hitPauseTime = new int[] { 9, 18 };
            hitData.hitSlideTime = 16;
            hitData.groundVel = new FixPointMath.Vector(-5, 0);
            hitData.guardPauseTime = new int[] { 9, 18 };
            hitData.guardSlideTime = 16;
            hitData.guardVel = new FixPointMath.Vector(-5, 0); 
            hitData.guardFlag = GuardFlag.Normal;
            hitData.groundCornerPush =  Number.One/ 2;
            hitData.airCornerPush = Number.One / 2;
            SetHitDefData(hitData, 100);
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
            if(StateTime == 0)
            {
                
            }  
        }
    }
}
