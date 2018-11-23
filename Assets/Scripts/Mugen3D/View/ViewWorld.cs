﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public class ViewWorld
    { 
        public Core.World world { get; private set; }
        private Dictionary<int, EntityView> m_entityViews = new Dictionary<int, EntityView>();
        public CameraController cameraController { get; private set; }
        private GameObject m_rootScene;

        public ViewWorld(Core.World world)
        {
            this.world = world;
            world.onAddEntity += (e) => {
                var entityView = ViewCreater.CreateView(e, m_rootScene.transform.Find("Players"));
                if (entityView != null)
                {
                    this.m_entityViews.Add(e.id, entityView);
                }
            };
            world.onRemoveEntity += (e) => { 
                EntityView view;
                if (m_entityViews.TryGetValue(e.id, out view))
                {
                    GameObject.Destroy(view.gameObject);
                }
            };
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
            var cameraController = go.AddComponent<CameraController>();
            cameraController.Init(config, p1, p2);
            
        }

        public EntityView GetView(int id)
        {
            return m_entityViews[id];
        }
       
    
    }

}