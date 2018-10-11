using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public class ViewWorld
    {
        public delegate EntityView ViewCreater(Core.Entity entity);
        private Dictionary<Type, ViewCreater> m_viewCreater = new Dictionary<Type, ViewCreater>() { };
        public Core.World world { get; private set; }
        private GameObject m_rootScene;

        private void OnCreateEntity(Core.Entity e)
        {
            ViewCreater creater;
            if (m_viewCreater.TryGetValue(e.GetType(), out creater))
            {
                creater(e);
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

        private void InitScene(GameObject rootScene)
        {
            this.m_rootScene = rootScene;
            GameObject stage = new GameObject("Stage");
            stage.transform.parent = rootScene.transform;
            GameObject players = new GameObject("Players");
            players.transform.parent = rootScene.transform;
        }

        public ViewWorld(Core.World world, GameObject root)
        {
            Init();
            this.world = world;
            world.onCreateEntity += OnCreateEntity;
            world.onCreateWorld += OnCreateStage;
            world.onCreateCameraController += OnCreateCamera;
            InitScene(root);
        }

        private EntityView OnCreateCharacter(Core.Entity entity)
        {
            var c = entity as Core.Character;
            UnityEngine.Object prefab = ResourceLoader.Load((c.config as Core.UnitConfig).prefab);
            GameObject go = GameObject.Instantiate(prefab, m_rootScene.transform.Find("Players")) as GameObject;
            var view = go.AddComponent<CharView>();
            view.Init(c);
            return view;
        }

        public void OnCreateStage(Core.WorldConfig config)
        {
            UnityEngine.Object prefabStage = Resources.Load<UnityEngine.Object>("Prefabs/Stage/" + config.stageConfig.stage);
            GameObject goStage = GameObject.Instantiate(prefabStage, m_rootScene.transform.Find("Stage")) as GameObject;
        }

        private void OnCreateCamera(Core.CameraController camCtl)
        {
            UnityEngine.Object prefab = ResourceLoader.Load("Prefabs/BattleCamera");
            GameObject go = GameObject.Instantiate(prefab, m_rootScene.transform) as GameObject;
            var view = go.AddComponent<CameraView>();
            view.Init(camCtl);
        }

        

       
    }

}