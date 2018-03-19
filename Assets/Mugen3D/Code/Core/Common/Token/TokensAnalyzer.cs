using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class TokensAnalyzer
    {
        private List<Token> m_tokens;
        private int m_curPos;
        private int m_curLine;
        private int m_curColumnInLine;

        public TokensAnalyzer(List<Token> tokens)
        {
            m_tokens = tokens;
            m_curPos = 0;
            m_curLine = 1;
            m_curColumnInLine = 1;
        }

        public List<Token> GetNextLine()
        {
            return null;
        }

        public Token GetNext()
        {
            return null;
        }
    }
}
