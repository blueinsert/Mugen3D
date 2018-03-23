using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    
    public class AnimationsDef
    {     
        private Dictionary<int, string> anims = new Dictionary<int,string>();

        public void Init(TextAsset animDef)
        {
            Parse(animDef);
        }

        public string GetAnimName(int no)
        {
            if (anims.ContainsKey(no))
                return anims[no];
            else
                return "none";
        }

        void Parse(TextAsset animDef)
        {
            Tokenizer tokenizer = new Tokenizer();
            List<Token> tokens = tokenizer.GetTokens(animDef);
            int tokenSize = tokens.Count;
            int pos = 0;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == ":")
                {
                    int key = int.Parse(tokens[pos - 2].value);
                    string value = tokens[pos++].value;
                    anims[key] = value;
                }
            }
        }
    }
}
