using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;

namespace Mugen3D
{

    public class ViewWorld
    {
        private Transform rootCharacter;

        public void SetRootCharacter(Transform r)
        {
            this.rootCharacter = r;
        }

        private void OnCreateCharacter(Character c)
        {
            UnityEngine.Object prefab = ResourceLoader.Load("Chars/" + c.characterName + c.config.modelFile);
            GameObject go = GameObject.Instantiate(prefab, rootCharacter) as GameObject;
            var view = go.AddComponent<CharView>();
            view.Init(c);
        }

        public void OnCreateEntity(Entity e)
        {
            if (e is Character)
            {
                OnCreateCharacter(e as Character);
            }
        }

       
    }

}