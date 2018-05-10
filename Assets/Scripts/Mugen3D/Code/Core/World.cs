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
        }

        private void Init() {
            collisionWorld = new CollisionWorld();
        }

        public CollisionWorld collisionWorld;
        public int gameTime = -1;
        public float deltaTime;
        public List<Entity> entities
        {
            get{
                return m_entities;
            }
        }

        private List<Entity> m_addedEntities = new List<Entity>();
        private List<Entity> m_destroyedEntities = new List<Entity>();
        private List<Entity> m_entities = new List<Entity>();

        public void AddEntity(Entity e)
        {
            m_addedEntities.Add(e);
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
            foreach (var ent in m_destroyedEntities)
            {
                DoRemoveEntity(ent);
            }
            m_destroyedEntities.Clear();      
        }

    }
}
