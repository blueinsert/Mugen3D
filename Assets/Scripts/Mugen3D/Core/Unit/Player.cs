using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class Player : Character
    {
        public string playerName { get; private set; }
        public Player(string playerName, string characterName, CharacterConfig config, int slot, bool isLocal)
            : base(characterName, config, slot, isLocal)
        {
            this.playerName = playerName;
        }
    }
}
