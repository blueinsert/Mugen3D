using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class StateManager
    {
        public MyDictionary<int, PlayerStateDef> States { get; set; }

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

    }//class
}//namespace
