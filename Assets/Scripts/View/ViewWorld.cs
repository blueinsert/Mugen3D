using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public class ViewWorld
    {
        public delegate EntityView ViewCreater(Core.Entity entity);
        private Dictionary<Type, ViewCreater> m_viewCreater = new Dictionary<Type, ViewCreater>();
        private GameObject m_rootScene;

        public ViewWorld()
        {
            m_viewCreater[typeof(Core.Character)] = OnCreateCharacter;
            m_viewCreater[typeof(Core.CameraController)] = OnCreateCamera;
        }

        public void CreateBattleScene(GameObject rootScene)
        {

            this.m_rootScene = rootScene;
            GameObject stage = new GameObject("Stage");
            stage.transform.parent = rootScene.transform;
            GameObject players = new GameObject("Players");
            players.transform.parent = rootScene.transform;
        }

        private EntityView OnCreateCharacter(Core.Entity entity)
        {
            var c = entity as Core.Character;
            UnityEngine.Object prefab = ResourceLoader.Load("Chars/" + c.characterName + c.config.modelFile);
            GameObject go = GameObject.Instantiate(prefab, m_rootScene.transform.Find("Players")) as GameObject;
            var view = go.AddComponent<CharView>();
            view.Init(c);
            return view;
        }

        private EntityView OnCreateCamera(Core.Entity entity)
        {
            Core.CameraController camCtl = entity as Core.CameraController;
            UnityEngine.Object prefab = ResourceLoader.Load("Prefabs/Scene/BattleCamera");
            GameObject go = GameObject.Instantiate(prefab, m_rootScene.transform) as GameObject;
            var view = go.AddComponent<CameraView>();
            view.Init(camCtl);
            return view;
        }

        public void OnCreateEntity(Core.Entity e)
        {
            ViewCreater creater;
            if (m_viewCreater.TryGetValue(e.GetType(), out creater))
            {
                creater(e);
            } else {
                Debug.LogError("can get view creater");
            }
        }

       
    }

}