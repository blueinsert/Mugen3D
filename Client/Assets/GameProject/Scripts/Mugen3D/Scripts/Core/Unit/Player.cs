using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class Player
    {
        public string playerName { get; private set; }
        public Character character { get; private set; }

        public Player(string playerName, Character character)
        {
            this.playerName = playerName;
            this.character = character;
        }
    }
}
