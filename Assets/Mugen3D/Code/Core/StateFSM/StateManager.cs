using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class StateManager
    {
        const int AI_STATE_NO = -4;
        const int CMD_STATE_NO = -3;

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

        public void ChangeState(int id, Action onFinish = null) {
            if (!States.ContainsKey(id))
            {
                Log.Error("states con't contain id:" + id);
                return;
            }
            isChangingState = true;

            var lastState = currentState;
            lastState.OnExit();
            historyStates.Add(lastState.stateId);

            currentState = States[id];
            currentState.Init();
            currentState.onFinish = onFinish;
            currentState.OnEnter();

            stateTime = -1;
            if (historyStates.Count > MaxHistoryStateNum)
            {
                historyStates.RemoveAt(0);
            }
            isChangingState = false;

            Update();
        }

        public void Update() {
            if (!isReady)
                return;
            if (isChangingState)
            {
                //Log.Info("changing state when update");
                return;
            }
            stateTime++;
            if (this.owner.AiLevel == -1)
            {
                if (States.ContainsKey(CMD_STATE_NO))
                {
                    States[CMD_STATE_NO].OnUpdate();
                }
                if (States.ContainsKey(-1))
                {
                    States[-1].OnUpdate();
                }
            }
            else
            {
                if (States.ContainsKey(AI_STATE_NO))
                {
                    States[AI_STATE_NO].OnUpdate();
                }
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
