using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.Core
{
    public class WorldBase
    {
        int frameCount = 0;
        private int m_maxEntityId = 0;
        private readonly List<Entity> m_toAddedEntities = new List<Entity>();
        private readonly List<Entity> m_toDestroyedEntities = new List<Entity>();
        private readonly List<Entity> m_entities = new List<Entity>();

        /// <summary>
        /// 获取所有系统
        /// </summary>
        protected virtual List<SystemBase> AllSystem { get; }

        public void AddEntity(Entity e)
        {
            m_toAddedEntities.Add(e);
            e.SetEntityId(m_maxEntityId++);
            OnAddEntity(e);
        }

        protected virtual void OnAddEntity(Entity e)
        {

        }

        protected virtual void OnRemoveEntity(Entity e)
        {

        }

        private void DoAddEntity(Entity e)
        {
            m_entities.Add(e);
            OnAddEntity(e);
        }

        private void DoRemoveEntity(Entity e)
        {
            m_entities.Remove(e);
            OnRemoveEntity(e);
        }

        protected virtual void OnAfterStep()
        {
            foreach (var e in m_toAddedEntities)
            {
                DoAddEntity(e);
            }
            m_toAddedEntities.Clear();
            foreach (var e in m_entities)
            {
                e.OnUpdate();
                if (e.isDestroyed)
                {
                    m_toDestroyedEntities.Add(e);
                }
            }
            foreach (var ent in m_toDestroyedEntities)
            {
                DoRemoveEntity(ent);
            }
            m_toDestroyedEntities.Clear();
        }

        public void Step()
        {
            frameCount++;
            OnStep();
            OnAfterStep();
        }

        protected virtual void OnStep()
        {
            foreach(var system in AllSystem)
            {
                system.Process(this);
            }
        }

        /// <summary>
        /// 在这一帧中有新加入的实体或者system
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDirty()
        {
            return true;
        }

        /// <summary>
        /// 获得所有实体
        /// </summary>
        public List<Entity> GetAllEntities()
        {
            return m_entities;
        }

        
       
    }
}
