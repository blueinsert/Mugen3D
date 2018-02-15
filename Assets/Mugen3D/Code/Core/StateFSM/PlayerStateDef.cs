using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Mugen3D
{
    public class PlayerStateDef
    {
        public Action onFinish;
        private Player owner;
        private StateEvent curEvent;

        public int stateId;
        private Dictionary<string, TokenList> InitParams;
        public List<StateEvent> events;

        public void Init() {
            foreach (var e in events)
            {
                e.Init();
            }
        }

        public PlayerStateDef()
        {
            events = new List<StateEvent>();
            InitParams = new Dictionary<string, TokenList>();
        }

        public void SetOwner(Player p)
        {
            owner = p;
        }

        public void AddInitParam(string key, TokenList tokens)
        {
            if (InitParams == null)
                InitParams = new Dictionary<string, TokenList>();    
            InitParams[key] = tokens;
        }

        public void OnEnter() {
            if (InitParams.ContainsKey("anim"))
            {
                Dictionary<string, TokenList> param = new Dictionary<string, TokenList> {
                    {"value", InitParams["anim"]}
                };
                Controllers.Instance.ChangeAnim(this.owner, param);
            }
            if (InitParams.ContainsKey("physics"))
            {
                Dictionary<string, TokenList> param = new Dictionary<string, TokenList> {
                    {"value", InitParams["physics"]}
                };
                Controllers.Instance.PhysicsSet(this.owner, param);
            }
            if (InitParams.ContainsKey("ctrl"))
            {
                Dictionary<string, TokenList> param = new Dictionary<string, TokenList> {
                    {"value", InitParams["ctrl"]}
                };
                Controllers.Instance.CtrlSet(this.owner, param);
            }
            if (InitParams.ContainsKey("moveType"))
            {
                Dictionary<string, TokenList> param = new Dictionary<string, TokenList> {
                    {"value", InitParams["moveType"]}
                };
                Controllers.Instance.SetMoveType(this.owner, param);
            }
        }

        public void OnExit()
        {
            if (onFinish != null)
            {
                onFinish();
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
                if (e.triggerOnce == false || (e.triggerOnce && e.isTriggered == false))
                {
                    Controllers.Instance.ExeController(owner, e.type, e.parameters, () => { e.isTriggered = true; });
                }
            }
        }

        bool CheckOptionalTriggerLists(Dictionary<int, List<Expression>> exDic)
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
                double result = owner.CalcExpressionInRuntime(e); 
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