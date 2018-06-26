using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;
namespace Mugen3D
{
    public class World
    {  
        public static World mInstance;
        public static World Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new World();
                }
                return mInstance;
            }
        }

        private World()
        {
            Init();
            config = new WorldConfig() { borderXMax = 400, borderXMin = -400, borderYMin = 0, borderYMax = 100 };
        }

        private void Init() {
        }

        public int gameTime = -1;
        public float deltaTime;
        public WorldConfig config;
        public List<Entity> entities
        {
            get{
                return m_entities;
            }
        }

        private List<Entity> m_addedEntities = new List<Entity>();
        private List<Entity> m_destroyedEntities = new List<Entity>();
        private List<Entity> m_entities = new List<Entity>();
        public CameraController camCtl;
        public TeamInfo teamInfo = new TeamInfo();

        public void AddEntity(Entity e)
        {
            m_addedEntities.Add(e);
            if (e is Character)
            {
                teamInfo.AddCharacter(e as Character);
            }
        }

        private void DoAddEntity(Entity e){
            m_entities.Add(e);    
        }

        private void DoRemoveEntity(Entity e)
        {
            m_entities.Remove(e);           
            GameObject.Destroy(e.gameObject);
        }

        public void Clear()
        {
            m_entities.Clear();
        }

        public void Update(float _deltaTime)
        {
            gameTime++;
            deltaTime = _deltaTime;

            foreach (var e in m_addedEntities)
            {
                DoAddEntity(e);
            }
            m_addedEntities.Clear();
            foreach (var e in m_entities)
            {
                e.OnUpdate();
                if (e.isDestroyed)
                {
                    m_destroyedEntities.Add(e);
                }
            }
            camCtl.Update();
            foreach (var ent in m_destroyedEntities)
            {
                DoRemoveEntity(ent);
            }
            m_destroyedEntities.Clear();      
        }

    }
}
