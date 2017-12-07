using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class PlayerConfig
    {
        private Dictionary<int, int> cfg = new Dictionary<int, int>();

        public PlayerConfig(TextAsset def)
        {
            Parse(def);
        }

        public int GetConfig(int key)
        {
            if (cfg.ContainsKey(key))
            {
                return cfg[key];
            }
            else
            {
                Log.Error("can't get playerConfit, key:" + key);
                return 0;
            }
        }

        private void Parse(TextAsset def)
        {
            Tokenizer tokenizer = new Tokenizer();
            List<Token> tokens = tokenizer.GetTokens(def);
            int tokenSize = tokens.Count;
            int pos = 0;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == ":")
                {
                    string key = tokens[pos - 2].value;
                    int value = int.Parse(tokens[pos++].value);
                    cfg[key.GetHashCode()] = value;
                }
            }
        }

    }//class   
}//namespace

