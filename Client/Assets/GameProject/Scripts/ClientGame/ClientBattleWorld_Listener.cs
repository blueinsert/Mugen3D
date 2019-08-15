using bluebean.Mugen3D.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public partial class ClientBattleWorld
    {
        public void OnBattleStart(int matchNo)
        {
            throw new NotImplementedException();
        }

        public void OnBattleEnd(int matchNo)
        {
            throw new NotImplementedException();
        }

        public void OnRoundStart(int roundNo)
        {
            throw new NotImplementedException();
        }

        public void OnRoundEnd(int roundNo)
        {
            throw new NotImplementedException();
        }

        public void OnCreateCharacter(Character character)
        {
            var config = character.Config;
            CharacterActor characterActor = new CharacterActor(GetAsset<GameObject>(config.Prefab), m_rootPlayers);
            characterActor.SetPosition(character.Position.x.AsFloat(), character.Position.y.AsFloat());
            characterActor.SetFacing(character.Facing);
            m_characterActorDic.Add(character.slot, characterActor);
        }

        public void OnDestroyCharacter(Character character)
        {
            throw new NotImplementedException();
        }

        public void OnPlaySound(string soundName)
        {
            throw new NotImplementedException();
        }
    }
}
