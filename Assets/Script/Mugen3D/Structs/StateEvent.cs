using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Mugen3D
{
    public class StateEvent
    {
        public int eventNumber;
        public StateEventType type;
        public MyList<Expression> requiredTriggerList;
        public MyDictionary<int, MyList<Expression>> optionalTriggerDic;
        public MyDictionary<string, MyList<Token>> parameters;

        public StateEvent()
        {
            requiredTriggerList = new MyList<Expression>();
            optionalTriggerDic = new MyDictionary<int, MyList<Expression>>();
            parameters = new MyDictionary<string, MyList<Token>>();
        }

        public void AddOptionalTrigger(int group,Expression e)
        {
            if (!optionalTriggerDic.ContainsKey(group))
            {
                optionalTriggerDic[group] = new MyList<Expression>();
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
            sb.Append("eventNum:").Append(eventNumber).Append(",");
            sb.Append("type:").Append(type.ToString()).Append(",");
            sb.Append("requiredTriggerList:").Append(requiredTriggerList.ToString()).Append(",");
            sb.Append("optionalTriggerDic").Append(optionalTriggerDic.ToString()).Append(",");
            sb.Append("parameters:").Append(parameters.ToString()).Append(",");
            sb.Append("}");
            return sb.ToString();
        }
    }
}
