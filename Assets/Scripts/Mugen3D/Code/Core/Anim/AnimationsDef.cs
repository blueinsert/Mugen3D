using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    
    public class AnimationsDef
    {     
        private Dictionary<int, string> m_anims = new Dictionary<int,string>();

        public AnimationsDef(string data)
        {
            Parse(data);
        }

        public string GetAnimName(int no)
        {
            if (m_anims.ContainsKey(no))
                return m_anims[no];
            else
                return "none";
        }

        void Parse(string data)
        {
            Tokenizer tokenizer = new Tokenizer();
            List<Token> tokens = tokenizer.GetTokens(data);
            int tokenSize = tokens.Count;
            int pos = 0;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == ":")
                {
                    int key = int.Parse(tokens[pos - 2].value);
                    string value = tokens[pos++].value;
                    m_anims[key] = value;
                }
            }
        }
    }
}
