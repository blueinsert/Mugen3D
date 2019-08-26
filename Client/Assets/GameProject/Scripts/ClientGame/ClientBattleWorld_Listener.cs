using bluebean.Mugen3D.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public partial class ClientBattleWorld
    {
        public void OnMatchStart(int matchNo)
        {
            //todo
        }

        public void OnMatchEnd(int matchNo)
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


        public void OnCreateCharacter(Entity character)
        {
            CreateCharacterActor(character);
        }

        public void OnDestroyCharacter(Entity character)
        {
            throw new NotImplementedException();
        }

        public void OnPlaySound(string soundName)
        {
            throw new NotImplementedException();
        }

        public void OnCameraCreate(CameraComponent cameraComponent)
        {
            CreateCameraController(cameraComponent);
        }
    }
}
