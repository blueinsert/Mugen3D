using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    
    class StateGetHitStandShake : StateBase
    {
        public StateGetHitStandShake(Entity e):base(e)
        {

        }

        public override void OnEnter()
        {
            PhysicsSet(PhysicsType.Stand);
            VelSet(0, 0);
            CtrlSet(false);
            ChangeAnim(5000);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            //冻结动画
            ChangeAnim(Anim);
            if(StateTime > HitShakeTime)
            {
                ChangeState(StateConst.StateNo_GetHitStandSlide);
            }
        }
    }
}
