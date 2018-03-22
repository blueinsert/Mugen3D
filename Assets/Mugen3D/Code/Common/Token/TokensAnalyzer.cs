using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class TokensAnalyzer
    {
        public int curPos { get { return m_curPos; } }

        private List<Token> m_tokens;
        private int m_tokenSize;
        private int m_curPos;
       
        public TokensAnalyzer(List<Token> tokens)
        {
            m_tokens = tokens;
            m_tokenSize = tokens.Count;
            m_curPos = 0;
        }

        public List<Token> GetLine()
        {
            List<Token> line = new List<Token>();
            while (m_curPos < m_tokenSize)
            {
                var t = m_tokens[m_curPos++];
                if (t.value != "\n")
                {
                    line.Add(t);
                }else{
                    break;
                }  
            }
            return line;
        }

        public Token GetToken()
        {
            if (m_curPos < m_tokenSize)
            {
                return m_tokens[m_curPos++];
            }
            else
            {
                return null;
            }
        }

        public Token GetTokenAt(int pos)
        {
            return m_tokens[pos];
        }

        public void GotoNextline()
        {
            while (m_curPos < m_tokenSize)
            {
                Token t = m_tokens[m_curPos++];
                if (t.value == "\n")
                {
                    break;
                }
            }
        }

        public bool IsAtEnd(){
            return m_curPos >= m_tokenSize;
        }
    }
}
