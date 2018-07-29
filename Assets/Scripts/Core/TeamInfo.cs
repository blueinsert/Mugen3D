using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class TeamInfo
    {
        private Dictionary<int, Character> m_chars = new Dictionary<int, Character>();

        public List<Character> chars
        {
            get
            {
                return new List<Character>(m_chars.Values);
            }
        }

        public void AddCharacter(Character c){
            m_chars.Add(c.slot, c);
        }

        public Character GetCharacter(int slot)
        {
            return m_chars[slot];
        }

        public Character GetEnemy(Character c)
        {
            foreach (var character in m_chars.Values)
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
