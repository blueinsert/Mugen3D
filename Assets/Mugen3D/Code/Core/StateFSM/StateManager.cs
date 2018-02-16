using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class StateManager
    {
        private Unit owner;
        private Dictionary<int, PlayerStateDef> m_states = new Dictionary<int, PlayerStateDef>();

        private List<int> historyStates = new List<int>();
        private int MaxHistoryStateNum = 10;

        public PlayerStateDef currentState;

        public int stateTime;

        private bool isReady = false;
        private bool isChangingState = false;

        public StateManager(Unit p)
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
                    m_states[kv.Key] = kv.Value;
                }
            }
            currentState = m_states[0];
            isReady = true;
        }

        public void ChangeState(int id, Action onFinish = null)
        {
            if (!m_states.ContainsKey(id))
            {
                Log.Error("states con't contain id:" + id);
                return;
            }
            isChangingState = true;

            var lastState = currentState;
            lastState.OnExit();
            historyStates.Add(lastState.stateId);

            currentState = m_states[id];
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

        public void Update()
        {
            if (!isReady)
                return;
            if (isChangingState)
            {
                //Log.Info("changing state when update");
                return;
            }
            stateTime++;
            for (int i = -3; i <= -1; i++)
            {
                if(m_states.ContainsKey(i))
                    m_states[i].OnUpdate();
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
