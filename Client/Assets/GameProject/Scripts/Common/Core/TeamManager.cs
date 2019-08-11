using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public enum TeamMode
    {
        Single = 1,//single player
        Team,//kof mode
    }

    public class TeamManager
    {
        private TeamMode m_teamMode = TeamMode.Single;

        private Dictionary<int, Character> m_chars = new Dictionary<int, Character>();

        public Character p1 {
            get {
                if (m_chars.ContainsKey(0))
                    return m_chars[0];
                else
                    return null;
            }
        }

        public Character p2
        {
            get
            {
                if (m_chars.ContainsKey(1))
                    return m_chars[1];
                else
                    return null;
            }
        }

        public List<Character> chars
        {
            get
            {
                return new List<Character>(m_chars.Values);
            }
        }

        public void RemoveCharacter(Character c)
        {
            m_chars.Remove(c.slot);
        }

        public void AddCharacter(Character c){
            m_chars.Add(c.slot, c);
        }

        public Character GetCharacter(int slot)
        {
            return m_chars[slot];
        }

        public Character GetEnemy(Unit u)
        {
            if (u is Character)
            {
                var c = u as Character;
                foreach (var character in m_chars.Values)
                {
                    if (character.slot != c.slot)
                    {
                        return character;
                    }
                }
            }
            else if (u is Helper)
            {
                return GetEnemy((u as Helper).owner);
            }        
            return null;
        }
        
    }
}
