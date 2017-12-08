using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class StateManager
    {
        private Player owner;
        private Dictionary<int, PlayerStateDef> States = new Dictionary<int,PlayerStateDef>();

        private List<int> historyStates = new List<int>();
        private int MaxHistoryStateNum = 10;

        public PlayerStateDef currentState;

        public int stateTime;

        private bool isReady = false;
        private bool isChangingState = false;

        public StateManager(Player p)
        {
            owner = p;
        }

        public void ReadStateDefFile(TextAsset[] files)
        {
            Utility.Assert(files != null && files.Length != 0, "cns file is null!");
            foreach (var f in files)
            {
                Tokenizer t = new Tokenizer();
                List<Token> tokens = t.GetTokens(f);
                StateParse p = new StateParse();
                p.Parse(tokens);
                Dictionary<int, PlayerStateDef> tmp = p.States;
                foreach (var kv in tmp)
                {
                    kv.Value.SetOwner(owner);
                    States[kv.Key] = kv.Value;
                }
            }
            currentState = States[0];
            isReady = true;
        }

        public void ChangeState(int id) {
            if (!States.ContainsKey(id))
            {
                Log.Error("states con't contain id:" + id);
            }
            isChangingState = true;
            var tmp = States[id];
            tmp.Init();
          
            historyStates.Add(currentState.stateId);
            currentState = tmp;
            currentState.OnEnter();

            stateTime = -1;
            
            if (historyStates.Count > MaxHistoryStateNum)
            {
                historyStates.RemoveAt(0);
            }
            isChangingState = false;
        }

        public void Update() {
            if (!isReady)
                return;
            if (isChangingState)
            {
                Log.Info("changing state when update");
                return;
            }
            stateTime++;
            if (States.ContainsKey(-3))
            {
                States[-3].OnUpdate();
            }
            if (States.ContainsKey(-1))
            {
                States[-1].OnUpdate();
            }
            currentState.OnUpdate();
        }

        public int GetPrevStateNo()
        {
            if (historyStates.Count >= 2)
            {
                return historyStates[historyStates.Count - 1];
            }
            else
            {
                return 0;
            }
        }

    }//class
}//namespace
