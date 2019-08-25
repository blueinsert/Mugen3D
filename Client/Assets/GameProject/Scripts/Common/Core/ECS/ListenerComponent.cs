using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class ListenerComponent : ComponentBase, IBattleWorldListener
    {
        public void OnMatchEnd(int matchNo)
        {
            throw new System.NotImplementedException();
        }

        public void OnMatchStart(int matchNo)
        {
            throw new System.NotImplementedException();
        }

        public void OnCreateCharacter(Entity character)
        {
            throw new System.NotImplementedException();
        }

        public void OnDestroyCharacter(Entity character)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlaySound(string soundName)
        {
            throw new System.NotImplementedException();
        }

        public void OnRoundEnd(int roundNo)
        {
            throw new System.NotImplementedException();
        }

        public void OnRoundStart(int roundNo)
        {
            throw new System.NotImplementedException();
        }
    }
}
