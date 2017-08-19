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
                        t = tokens[pos++];
                        int eventNum;
                        Utility.Assert(int.TryParse(t.value, out eventNum), "a event number must be int");
                        curParseStateEvent = new StateEvent();
                        curParseStateEvent.eventNumber = eventNum;
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
                if (t.value == "physics")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after physics");
                    t = tokens[pos++];
                    switch (t.value)
                    {
                        case "S":
                            curParseState.physicsType = PhysicsType.Stand;
                            break;
                        case "C":
                            curParseState.physicsType = PhysicsType.Croch;
                            break;
                        case "A":
                            curParseState.physicsType = PhysicsType.Air;
                            break;
                        default:
                            curParseState.physicsType = PhysicsType.None;
                            break;
                    }
                }
                else if (t.value == "movetype")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after movetype");
                    t = tokens[pos++];
                    switch (t.value)
                    {
                        case "A":
                            curParseState.moveType = MoveType.Attack;
                            break;
                        case "D":
                            curParseState.moveType = MoveType.Defence;
                            break;
                        case "I":
                            curParseState.moveType = MoveType.Idle;
                            break;
                        default:
                            curParseState.moveType = MoveType.Idle;
                            break;
                    }
                }
                else if (t.value == "anim")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after anim");
                    t = tokens[pos++];
                    curParseState.anim = t.value;
                }
                else if (t.value == "velset")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after velset");
                    float vx, vy;
                    Utility.Assert(float.TryParse(tokens[pos++].value, out vx), "velset vel_x should be float");
                    Utility.Assert(tokens[pos++].value == ",", "should be ',' between vel components");
                    Utility.Assert(float.TryParse(tokens[pos++].value, out vy), "velset vel_y should be float");
                    curParseState.vel = new DVector3(0, vy, vx);
                }
                else if (t.value == "ctrl")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after velset");
                    t = tokens[pos++];
                    if (t.value == "true")
                    {
                        curParseState.ctrl = true;
                    }
                    else
                    {
                        curParseState.ctrl = false;
                    }
                }
                else if (t.value == "[")
                {
                    pos--;
                    break;
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
                if (t.value == "ChangeState")
                {
                    curParseStateEvent.type = StateEventType.ChangeState;
                    ParseEvent_ChangeState(tokens, ref pos);
                }
                else if (t.value == "VelSet")
                {
                    curParseStateEvent.type = StateEventType.VelSet;
                    ParseEvent_VelSet(tokens, ref pos);
                }
                else if (t.value == "ChangeAnim")
                {
                    curParseStateEvent.type = StateEventType.ChangeAnim;
                    ParseEvent_ChangeAnim(tokens, ref pos);
                }
            }
        }

        void OnParseEventDone()
        {
            Utility.Assert(curParseStateEvent != null, "cur event can be null after parse");
            curParseState.events.Add(curParseStateEvent);
            curParseStateEvent = null;
        }

        void ParseEvent_ChangeState(List<Token> tokens, ref int pos)
        {
            int tokenSize = tokens.Count;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == "value")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeState's value");
                    curParseStateEvent.parameters["value"] = new MyList<Token>();
                    while (pos < tokenSize && (t = tokens[pos++]).value != "\n")
                    {
                        curParseStateEvent.parameters["value"].Add(t);
                    }
                }
                else if (t.value == "triggerall")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeAnim's triggerall");
                    curParseStateEvent.requiredTriggerList.Add(Parse_Expression(tokens, ref pos));
                }
                else if (t.value == "trigger1")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeAnim's trigger1");
                    curParseStateEvent.AddOptionalTrigger(0, Parse_Expression(tokens, ref pos));
                }
                else if (t.value == "trigger2")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeAnim's trigger2");
                    curParseStateEvent.AddOptionalTrigger(1, Parse_Expression(tokens, ref pos));
                }
                else if (t.value == "[")
                {
                    pos--;
                    break;
                }
            }
            OnParseEventDone();
        }

        void ParseEvent_VelSet(List<Token> tokens, ref int pos)
        {
            int tokenSize = tokens.Count;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == "x")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after VelSet's x");
                    curParseStateEvent.parameters["x"] = new MyList<Token>();
                    while (pos < tokenSize && (t = tokens[pos++]).value != "\n")
                    {
                        curParseStateEvent.parameters["x"].Add(t);
                    }
                }
                else if (t.value == "y")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after VelSet's y");
                    curParseStateEvent.parameters["y"] = new MyList<Token>();
                    while (pos < tokenSize && (t = tokens[pos++]).value != "\n")
                    {
                        curParseStateEvent.parameters["y"].Add(t);
                    }
                }
                else if (t.value == "triggerall")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeAnim's triggerall");
                    curParseStateEvent.requiredTriggerList.Add(Parse_Expression(tokens, ref pos));
                }
                else if (t.value == "trigger1")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeAnim's trigger1");
                    curParseStateEvent.AddOptionalTrigger(0, Parse_Expression(tokens, ref pos));
                }
                else if (t.value == "trigger2")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeAnim's trigger2");
                    curParseStateEvent.AddOptionalTrigger(1, Parse_Expression(tokens, ref pos));
                }
                else if (t.value == "[")
                {
                    pos--;
                    break;
                }
            }
            OnParseEventDone();
        }

        void ParseEvent_ChangeAnim(List<Token> tokens, ref int pos)
        {
            int tokenSize = tokens.Count;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == "value")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeAnim's value");
                    curParseStateEvent.parameters["value"] = new MyList<Token>();
                    while (pos < tokenSize && (t = tokens[pos++]).value != "\n")
                    {
                        curParseStateEvent.parameters["value"].Add(t);
                    }
                }
                else if (t.value == "triggerall")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeAnim's triggerall");
                    curParseStateEvent.requiredTriggerList.Add(Parse_Expression(tokens, ref pos));
                }
                else if (t.value == "trigger1")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeAnim's trigger1");
                    curParseStateEvent.AddOptionalTrigger(0, Parse_Expression(tokens, ref pos));
                }
                else if (t.value == "trigger2")
                {
                    Utility.Assert(tokens[pos++].value == "=", "should be = after ChangeAnim's trigger2");
                    curParseStateEvent.AddOptionalTrigger(1, Parse_Expression(tokens, ref pos));
                }
                else if (t.value == "[")
                {
                    pos--;
                    break;
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
                pos++;
            }
            pos++;
            Expression expression = new Expression(expressionTokens, false);
            return expression;
        }

    }
}
