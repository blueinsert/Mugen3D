using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public class ViewWorld
    {
       
        private delegate EntityView ViewCreater(Core.Entity entity);
        private Dictionary<Type, ViewCreater> m_viewCreater = new Dictionary<Type, ViewCreater>() { };

        public Core.World world { get; private set; }
        private Dictionary<int, EntityView> m_entityViews = new Dictionary<int, EntityView>();
        public CameraController cameraController { get; private set; }
        private GameObject m_rootScene;

        private void OnCreateEntity(Core.Entity e)
        {
            ViewCreater creater;
            if (m_viewCreater.TryGetValue(e.GetType(), out creater))
            {
                var view = creater(e);
                m_entityViews.Add(e.id, view);
            }
            else
            {
                Debug.LogError("can get view creater");
            }
        }

        private void Init()
        {
            m_viewCreater[typeof(Core.Character)] = OnCreateCharacter;
        }

        public ViewWorld(Core.World world)
        {
            Init();
            this.world = world;
            world.onCreateEntity += OnCreateEntity;
        }

        public EntityView GetView(int id)
        {
            return m_entityViews[id];
        }

        private EntityView OnCreateCharacter(Core.Entity entity)
        {
            var c = entity as Core.Character;
            UnityEngine.Object prefab = ResourceLoader.Load((c.config as Core.UnitConfig).prefab);
            GameObject go = GameObject.Instantiate(prefab, m_rootScene.transform.Find("Players")) as GameObject;
            var view = go.AddComponent<CharacterView>();
            view.Init(c);
            return view;
        }

        public void InitScene(GameObject rootScene)
        {
            this.m_rootScene = rootScene;
            GameObject stage = new GameObject("Stage");
            stage.transform.parent = rootScene.transform;
            GameObject players = new GameObject("Players");
            players.transform.parent = rootScene.transform;
        }

        public void CreateStage(string stage)
        {
            UnityEngine.Object prefabStage = Resources.Load<UnityEngine.Object>("Prefabs/Stage/" + stage);
            GameObject goStage = GameObject.Instantiate(prefabStage, m_rootScene.transform.Find("Stage")) as GameObject;
        }

        public void CreateCamera(Core.CameraConfig config, Core.Character p1, Core.Character p2)
        {
            UnityEngine.Object prefab = ResourceLoader.Load("Prefabs/BattleCamera");
            GameObject go = GameObject.Instantiate(prefab, m_rootScene.transform) as GameObject;
            cameraController = go.AddComponent<CameraController>();
            cameraController.Init(config, p1, p2);
        }
    
    }

}