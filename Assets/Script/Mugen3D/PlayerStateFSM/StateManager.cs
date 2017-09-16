using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class StateManager
    {
        private Player owner;
        private MyDictionary<int, PlayerStateDef> States = new MyDictionary<int,PlayerStateDef>();
        private List<PlayerStateDef> historyStates = new List<PlayerStateDef>();

        private int MaxHistoryStateNum = 10;

        public PlayerStateDef currentState;

        public int stateTime;

        private bool isReady = false;

        public StateManager(Player p)
        {
            owner = p;
        }

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
                    kv.Value.SetOwner(owner);
                    States.Add(kv.Key, kv.Value);
                }
            }
            currentState = States[0];
            isReady = true;
        }

        public void ChangeState(int id) {
            currentState = States[id];
            currentState.OnEnter();
            stateTime = -1;
            historyStates.Add(currentState);
            if (historyStates.Count > MaxHistoryStateNum)
            {
                historyStates.RemoveAt(0);
            }
        }

        public void Update() {
            if (!isReady)
                return;
            stateTime++;
            if (States.ContainsKey(9999))
            {
                States[9999].OnUpdate();
            }
            currentState.OnUpdate();
        }

        public int GetPrevStateNo()
        {
            if (historyStates.Count >= 2)
            {
                return historyStates[historyStates.Count - 1].stateId;
            }
            else
            {
                return 0;
            }
        }

    }//class
}//namespace
