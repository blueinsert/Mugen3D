using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class StateParse
    {
        public MyDictionary<int, PlayerStateDef> mStates = new MyDictionary<int, PlayerStateDef>();
        public MyDictionary<int, PlayerStateDef> States { get { return mStates; } }
        private PlayerStateDef curParseState;
        private int curParseStateId;
        private StateEvent curParseStateEvent;

        

       public void Parse(List<Token> tokens)
        {
            int pos = 0;
            int tokenSize = tokens.Count;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == "[")
                {
                    t = tokens[pos++];
                    if (t.value == "Statedef")
                    {
                        t = tokens[pos++];
                        int stateId;
                        Utility.Assert(int.TryParse(t.value, out stateId), "a statedef must has it's id");
                        curParseState = new PlayerStateDef();
                        curParseStateId = stateId;
                        curParseState.stateId = stateId;
                        mStates[stateId] = curParseState;
                        //skip useless
                        while ((t = tokens[pos++]).value != "\n") { }
                        ParseStateDef(tokens, ref pos);
                    }
                    else if (t.value == "Event")
                    {
                        curParseStateEvent = new StateEvent();
                        t = tokens[pos++];
                        int eventNum;
                        if (int.TryParse(t.value, out eventNum))
                        {
                            curParseStateEvent.eventNumber = eventNum;
                        }
                        else
                        {
                            curParseStateEvent.eventNumber = -1;
                        }                
                        //skip useless
                        while ((t = tokens[pos++]).value != "\n") { }
                        ParseStateEvent(tokens, ref pos);
                    }
                }
            }
        }

        void ParseStateDef(List<Token> tokens, ref int pos)
        {
            int tokenSize = tokens.Count;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == "[")
                {
                    pos--;
                    break;
                }
                else if (t.value == "=")
                {
                    Token tKey = tokens[pos - 2];
                    MyList<Token> value = new MyList<Token>();
                    while (pos < tokenSize && (t = tokens[pos++]).value != "\n")
                    {
                        value.Add(t);
                    }
                    curParseState.AddInitParam(tKey.value, value.ToArray());
                }
            }
        }

        #region parse_event
        void ParseStateEvent(List<Token> tokens, ref int pos)
        {
            Token t = tokens[pos++];
            if (t.value == "type")
            {
                Utility.Assert(tokens[pos++].value == "=", "should be = after event type");
                t = tokens[pos++];
                switch (t.value)
                {
                    case "ChangeState":
                        curParseStateEvent.type = StateEventType.ChangeState; break;
                    case "VelSet":
                        curParseStateEvent.type = StateEventType.VelSet; break;
                    case "ChangeAnim":
                        curParseStateEvent.type = StateEventType.ChangeAnim; break;
                    case "PhysicsSet":
                        curParseStateEvent.type = StateEventType.PhysicsSet; break;
                    case "PosSet":
                        curParseStateEvent.type = StateEventType.PosSet; break;
                    case "VarSet":
                        curParseStateEvent.type = StateEventType.VarSet; break;
                    default :
                        Debug.LogError("event type can not be recognized :" + t.value); Application.Quit(); break;
                }
                while ((t = tokens[pos++]).value != "\n") { }
                OnParseEvent(tokens, ref pos); 
            }
        }

        void OnParseEventDone()
        {
            Utility.Assert(curParseStateEvent != null, "cur event can be null after parse");
            curParseState.events.Add(curParseStateEvent);
            curParseStateEvent = null;
        }

        void OnParseEvent(List<Token> tokens, ref int pos)
        {
             int tokenSize = tokens.Count;
             while (pos < tokenSize)
             {
                 Token t = tokens[pos++];
                 if (t.value == "[")
                 {
                     pos--;
                     break;
                 }
                 else if (t.value == "=")
                 {
                     Token tKey = tokens[pos - 2];
                     if (tKey.value == "triggerall")
                     {
                         curParseStateEvent.requiredTriggerList.Add(Parse_Expression(tokens, ref pos));
                     }
                     else if (tKey.value == "trigger1" || tKey.value == "trigger2")
                     {
                         curParseStateEvent.AddOptionalTrigger(0, Parse_Expression(tokens, ref pos));
                     }
                     else
                     {
                         curParseStateEvent.parameters[tKey.value] = new MyList<Token>();
                         while (pos < tokenSize && (t = tokens[pos++]).value != "\n")
                         {
                             curParseStateEvent.parameters[tKey.value].Add(t);
                         }
                     }
                 }
             }
             OnParseEventDone();
        }
        #endregion

        Expression Parse_Expression(List<Token> tokens, ref int pos)
        {
            List<Token> expressionTokens = new List<Token>();
            while (tokens[pos].value != "\n")
            {
                expressionTokens.Add(tokens[pos]);
                Debug.Log(tokens[pos].type +":"+ tokens[pos].value);
                pos++;
            }
            pos++;
            Expression expression = new Expression(expressionTokens, false);
            return expression;
        }

    }
}
