using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class FsmManager
    {
        public XLua.LuaTable fsm;
        public delegate XLua.LuaTable DelegateFsmConstruct(Unit u);
        public delegate void DelegateFsmUpdate(XLua.LuaTable self);
        public delegate void DelegateFsmChangeState(XLua.LuaTable self, int stateNo);
        private DelegateFsmConstruct funcFsmConstruct;
        private DelegateFsmUpdate funcFsmUpdate;
        private DelegateFsmChangeState funcFsmChangeState;

        public FsmManager(string fsmFile, Unit owner)
        {
            XLua.LuaTable characterModule = LuaMgr.Instance.Env.DoString(string.Format("return (require('{0}'))", fsmFile))[0] as XLua.LuaTable;
            funcFsmConstruct = characterModule.Get<string, DelegateFsmConstruct>("new");
            this.fsm = funcFsmConstruct(owner);
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
