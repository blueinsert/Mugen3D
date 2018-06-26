using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class FsmManager
    {
        public XLua.LuaTable fsm;
        private delegate void DelegateFsmUpdate(XLua.LuaTable self);
        private delegate void DelegateFsmChangeState(XLua.LuaTable self, int stateNo);
        private DelegateFsmUpdate funcFsmUpdate;
        private DelegateFsmChangeState funcFsmChangeState;

        public FsmManager(string fsmFile, Unit owner)
        {
            this.fsm = LuaMgr.Instance.Env.DoString(string.Format("return (require('{0}')).new(CS.Mugen3D.EntityLoader.curUnit)", fsmFile))[0] as XLua.LuaTable;
            funcFsmUpdate = this.fsm.Get<string, DelegateFsmUpdate>("update");
            funcFsmChangeState = this.fsm.Get<string, DelegateFsmChangeState>("changeState"); 
        }

        public void Update()
        {
            if (funcFsmUpdate != null)
            {
                funcFsmUpdate(this.fsm);
            }
        }

        public void ChangeState(int stateNo)
        {
            if (funcFsmChangeState != null)
            {
                funcFsmChangeState(fsm, stateNo);
            }
        }


    }

}
