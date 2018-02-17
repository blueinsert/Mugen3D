using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Mugen3D
{
    public enum StateEventType
    {
        Null,
        VelSet,
        CtrlSet,
        ChangeAnim,
        ChangeState,
        DestroySelf,
        PhysicsSet,
        PosSet,
        VarSet,
        HitDef,
        Helper,
        Pause,
    }

    public class StateEvent
    {
        public int eventNo;
        public int stateNo;
        public StateEventType type;
        public bool triggerOnce = false;
        public bool isTriggered = false;
        public List<Expression> requiredTriggerList;
        public Dictionary<int, List<Expression>> optionalTriggerDic;
        public Dictionary<string, TokenList> parameters;

        public void Init()
        {
            if (parameters.ContainsKey("triggerOnce"))
            {
                var value = parameters["triggerOnce"].asStr;
                if (value == "true")
                {
                    triggerOnce = true;
                }
                else
                {
                    triggerOnce = false;
                }
            }
            else
            {
                triggerOnce = false;
            }
            isTriggered = false;
        }

        public StateEvent()
        {
            requiredTriggerList = new List<Expression>();
            optionalTriggerDic = new Dictionary<int, List<Expression>>();
            parameters = new Dictionary<string, TokenList>();
        }

        public void AddOptionalTrigger(int group,Expression e)
        {
            if (!optionalTriggerDic.ContainsKey(group))
            {
                optionalTriggerDic[group] = new List<Expression>();
                optionalTriggerDic[group].Add(e);
            }
            else
            {
                optionalTriggerDic[group].Add(e);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("eventNum:").Append(eventNo).Append(",");
            sb.Append("type:").Append(type.ToString()).Append(",");
            sb.Append("requiredTriggerList:").Append(requiredTriggerList.ToString()).Append(",");
            sb.Append("optionalTriggerDic").Append(optionalTriggerDic.ToString()).Append(",");
            sb.Append("parameters:").Append(parameters.ToString()).Append(",");
            sb.Append("}");
            return sb.ToString();
        }
    }
}
