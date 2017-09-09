using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class StateManager
    {
        private MyDictionary<int, PlayerStateDef> States = new MyDictionary<int,PlayerStateDef>();
        public List<PlayerStateDef> historyStates = new List<PlayerStateDef>();

        private int MaxHistoryStateNum = 10;

        public PlayerStateDef currentState;
        public int CurrentStateId { get; set; }

        private int mStateTime;

        public void ReadStateDefFile(TextAsset[] files)
        {
            foreach (var f in files)
            {
                Tokenizer t = new Tokenizer();
                List<Token> tokens = t.GetTokens(f);
                StateParse p = new StateParse();
                p.Parse(tokens);
                MyDictionary<int, PlayerStateDef> tmp = p.States;
                foreach (var kv in tmp)
                {
                    States.Add(kv.Key, kv.Value);
                }
            }
        }

        public void ChangeState(int id) {
            CurrentStateId = id;
            currentState = States[id];
            currentState.OnEnter();
            mStateTime = 0;
            historyStates.Add(currentState);
            if (historyStates.Count > MaxHistoryStateNum)
            {
                historyStates.RemoveAt(0);
            }
        }

        public void Update() {
            mStateTime++;
            currentState.OnUpdate();
        }

    }//class
}//namespace
