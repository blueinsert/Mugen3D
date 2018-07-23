using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;

namespace Mugen3D
{

    public class ViewWorld
    {
        public GameObject rootScene;

        public void CreateBattleScene(GameObject rootScene)
        {

            this.rootScene = rootScene;
            GameObject stage = new GameObject("Stage");
            stage.transform.parent = rootScene.transform;
            GameObject players = new GameObject("Players");
            players.transform.parent = rootScene.transform;
        }

        private void OnCreateCharacter(Character c)
        {
            UnityEngine.Object prefab = ResourceLoader.Load("Chars/" + c.characterName + c.config.modelFile);
            GameObject go = GameObject.Instantiate(prefab, rootScene.transform.Find("Players")) as GameObject;
            var view = go.AddComponent<CharView>();
            view.Init(c);
        }

        private void OnCreateCamera(CameraController camCtl)
        {
            UnityEngine.Object prefab = ResourceLoader.Load("Prefabs/Scene/BattleCamera");
            GameObject go = GameObject.Instantiate(prefab, rootScene.transform) as GameObject;
            var view = go.AddComponent<CameraView>();
            view.Init(camCtl);
        }

        public void OnCreateEntity(Entity e)
        {
            if (e is Character)
            {
                OnCreateCharacter(e as Character);
            }
            else if (e is CameraController)
            {
                OnCreateCamera(e as CameraController);
            }
        }

       
    }

}