using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    class StateGuardStart : StateBase
    {
        public StateGuardStart(Entity e) : base(e)
        {

        }

        public override void OnEnter()
        {
            MoveTypeSet(MoveType.Defence);
            CtrlSet(false);
            ChangeAnim(120);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            if (LeftAnimTime <= 0)
            {
                ChangeState(StateConst.StateNo_Guarding);
            }
        }
    }

    class StateGuarding:StateBase
    {
        public StateGuarding(Entity e) : base(e) { }

        public override void OnEnter()
        {
            CtrlSet(false);
            ChangeAnim(130);
            VelSet(0, 0);
            MoveTypeSet(MoveType.Defence);
            PhysicsSet(PhysicsType.Stand);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            if(!CommandIsActive("holdback") || P2MoveType!= MoveType.Attack)
            {
                ChangeState(StateConst.StateNo_GuardEnd);
            }
        }
    }

    class StateGuardEnd : StateBase
    {
        public StateGuardEnd(Entity e) : base(e)
        {

        }

        public override void OnEnter()
        {
            ChangeAnim(140);
            MoveTypeSet(MoveType.Defence);
            CtrlSet(false);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            if (LeftAnimTime <= 0)
            {
                ChangeState(0);
            }
        }
    }

     class StateGuardingShake : StateBase
    {
        public StateGuardingShake(Entity e) : base(e) { }

        public override void OnEnter()
        {
            CtrlSet(false);
            VelSet(0, 0);
            MoveTypeSet(MoveType.Defence);
            PhysicsSet(PhysicsType.Stand);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            ChangeAnim(130);
            if(StateTime >= GuardShakeTime)
            {
                ChangeState(StateConst.StateNo_GuardingSlide);
            }
        }
    }

    class StateGuardingSlide:StateBase
    {
        public StateGuardingSlide(Entity e) : base(e) { }

        public override void OnEnter()
        {
            CtrlSet(false);
            VelSet(BeHitData.guardVel.x, BeHitData.guardVel.y);
            MoveTypeSet(MoveType.Defence);
            PhysicsSet(PhysicsType.Stand);
            ChangeAnim(130);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            if(StateTime >= GuardSlideTime)
            {
                ChangeState(StateConst.StateNo_Guarding);
            }
        }
    }

}
