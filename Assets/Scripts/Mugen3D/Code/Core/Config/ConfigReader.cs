using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class ConfigReader
    {
        private Config m_config;
        private string m_curKey;
        private TokensAnalyzer m_analyzer;

        public  Config GetConfig(string data)
        {
            Parse(data);
            return m_config;
        }

        private void Parse(string data)
        {
            m_config = new Config();
            m_analyzer = new TokensAnalyzer((new Tokenizer()).GetTokens(data));
          
            while (!m_analyzer.IsAtEnd())
            {
                Token t = m_analyzer.GetToken();
                if (t.value == ":")
                {
                    m_curKey = m_analyzer.GetTokenAt(m_analyzer.curPos - 2).value;
                    ParseValue();
                }
            }
        }

        private void ParseValue()
        {
            while (!m_analyzer.IsAtEnd())
            {
                Token t = m_analyzer.GetToken();
                if (t.type == TokenType.Num)
                {
                    m_config.AddConfig(m_curKey, t.value);
                    m_analyzer.GotoNextline();
                    break;
                }
                else if (t.type == TokenType.Str)
                {
                    m_config.AddConfig(m_curKey, t.value);
                    m_analyzer.GotoNextline();
                    break;
                }
                else if (t.value == "[")
                {
                    List<string> v = ParseVector();
                    Utility.Assert(v.Count <= 4 && v.Count >=1, "vector size should <= 4 and >=1!");
                    List<string> postfix = new List<string> { "x", "y","z","w"};
                    for (int i = 0; i < v.Count; i++)
                    {
                        m_config.AddConfig(m_curKey + "." + postfix[i], v[i]);
                    }
                    m_analyzer.GotoNextline();
                    break;
                }
                
            }
        }

        private List<string> ParseVector()
        {
            List<string> v = new List<string>();
            while (!m_analyzer.IsAtEnd())
            {
                Token t = m_analyzer.GetToken();
                if (t.type == TokenType.Num)
                {
                    v.Add(t.value);
                }
                else if (t.value == "]")
                {
                    break;
                }
            }
            return v;
        }

    }//class 
}
