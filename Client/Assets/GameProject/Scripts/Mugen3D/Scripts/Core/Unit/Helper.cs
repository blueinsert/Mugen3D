using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class Helper : Unit
    {
        public Character owner { get; private set; }

        public Helper(HelperConfig config, Character owner) : base(config)
        {
            this.owner = owner;
            /*
            SetConfig(config);
            moveCtr = new MoveCtrl(this);
            animCtr = new AnimationController(config.actions, this);
            fsmMgr = new FsmManager(config.fsm, this);
            */
        }
    }

}
