using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class ConfigReader
    {
        private Config m_config;

        public  Config GetConfig(string data)
        {
            Parse(data);
            return m_config;
        }

        private void Parse(string data)
        {
            m_config = new Config();
            Tokenizer tokenizer = new Tokenizer();
            List<Token> tokens = tokenizer.GetTokens(data);
            int tokenSize = tokens.Count;
            int pos = 0;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == ":")
                {
                    string key = tokens[pos - 2].value;
                    float value = float.Parse(tokens[pos++].value);
                    cfg[key.GetHashCode()] = value;
                }
            }
        }

    }//class 
}
