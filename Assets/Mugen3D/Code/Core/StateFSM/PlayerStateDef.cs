using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Mugen3D
{
    public class PlayerStateDef
    {
        private Player owner;
        private StateEvent curEvent;

        public int stateId;
        private MyDictionary<string, string> InitParams;
        public MyList<StateEvent> events;

        public void Init() {
            foreach (var e in events)
            {
                e.Init();
            }
        }

        public PlayerStateDef()
        {
            events = new MyList<StateEvent>();
            InitParams = new MyDictionary<string, string>();
        }

        public void SetOwner(Player p)
        {
            owner = p;
        }

        public void AddInitParam(string key, Token[] tokens)
        {
            if (InitParams == null)
                InitParams = new MyDictionary<string, string>();
            string v = "";
            for (int i = 0; i < tokens.Length; i++)
            {
                v += tokens[i].value;
            }
            InitParams[key] = v;
        }

        public void OnEnter() {
            if (InitParams.ContainsKey("anim"))
            {
                Dictionary<string, string> param = new Dictionary<string,string> {
                    {"value", InitParams["anim"]}
                };
                Controllers.Instance.ChangeAnim(this.owner, param);
            }
            if (InitParams.ContainsKey("physics"))
            {
                Dictionary<string, string> param = new Dictionary<string, string> {
                    {"value", InitParams["physics"]}
                };
                Controllers.Instance.PhysicsSet(this.owner, param);
            }
        }

        public void OnUpdate()
        {
            for (int i = 0; i < events.Count; i++)
            {
                StateEvent e = events[i];
                curEvent = e;
                if (curEvent.type == StateEventType.Null)
                    continue;
                bool checkRequired = true;
                bool checkOptional = true;
                //check requiredTriggerList
                if (e.requiredTriggerList != null && e.requiredTriggerList.Count != 0)
                {
                    checkRequired = true;
                }else{
                    checkRequired = false;
                }
                if(e.optionalTriggerDic!=null && e.optionalTriggerDic.Count!=0){
                    checkOptional = true;
                }else{
                    checkOptional = false;
                }
                bool passed = (checkRequired ? CheckTriggerList(e.requiredTriggerList) : true) 
                    && (checkOptional?CheckOptionalTriggerLists(e.optionalTriggerDic):true);
                if (!passed)
                    continue;
                Dictionary<string, string> param = new Dictionary<string, string>();
                foreach (var p in e.parameters)
                {
                    string v = "";
                    for (int j = 0; j < p.Value.Count; j++)
                    {
                        v += p.Value[j].value;
                    }
                    param[p.Key] = v;
                }
                if (e.triggerOnce == false || (e.triggerOnce && e.isTriggered == false))
                {
                    Controllers.Instance.ExeController(owner, e.type, param, () => { e.isTriggered = true; });
                }
            }
        }

        bool CheckOptionalTriggerLists(MyDictionary<int, MyList<Expression>> exDic)
        {
            foreach (var kv in exDic)
            {
                if (CheckTriggerList(kv.Value))
                {
                    return true;
                }
            }
            return false;
        }

        bool CheckTriggerList(List<Expression> expressions) {
            bool passed = true;
            foreach (var e in expressions) {
                VirtualMachine vm = new VirtualMachine();
                vm.SetOwner(this.owner);
                vm.SetDebugInfo(new DebugInfo { stateNo = this.stateId, eventNo = curEvent.eventNumber });
                double result = vm.Execute(e);
                if (result == 0)
                {
                    passed = false;
                    break;
                }

            }
            return passed;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("id:").Append(stateId).Append(",");
            sb.Append("initParams:").Append(InitParams.ToString()).Append(",");
            sb.Append("events:").Append(events.ToString()).Append(",");
            sb.Append("}");
            return sb.ToString();
        }
    }
}