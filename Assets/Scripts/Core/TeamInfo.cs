using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
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

        public Character GetEnemy(Character c)
        {
            foreach (var character in m_chars)
            {
                if (character.slot != c.slot)
                {
                    return character;
                }
            }
            return null;
        }
        
    }
}
