using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class TeamInfo
    {
        private List<Character> m_chars = new List<Character>();
        public List<Character> chars
        {
            get
            {
                return m_chars;
            }
        }

        public void AddCharacter(Character c){
            m_chars.Add(c);
        }


        
    }
}
