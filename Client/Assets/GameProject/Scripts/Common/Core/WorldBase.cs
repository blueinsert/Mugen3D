using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.Core
{
    public class WorldBase
    {
        int frameCount = 0;
        private int m_maxEntityId = 0;

        public readonly Dictionary<int, Entity> m_entityDic = new Dictionary<int, Entity>();
        /// <summary>
        /// 获取所有系统
        /// </summary>
        protected virtual List<SystemBase> AllSystem { get; }

        protected void AddEntity(Entity e)
        {
            m_entityDic.Add(e.ID,e);
        }

        protected void RemoveEntity(int id)
        {
            m_entityDic.Remove(id);
        }

        protected void RemoveEntity(Entity entity)
        {
            RemoveEntity(entity.ID);
        }

        public Entity GetEntity(int id)
        {
            return m_entityDic[id];
        }

        /// <summary>
        /// 获得所有实体
        /// </summary>
        public List<Entity> GetAllEntities()
        {
            return new List<Entity>(m_entityDic.Values);
        }

        protected virtual void OnAfterStep()
        {
           
        }

        protected virtual void OnStep()
        {
            foreach (var system in AllSystem)
            {
                system.Process(this);
            }
        }

        public void Step()
        {
            frameCount++;
            OnStep();
            OnAfterStep();
        }

        /// <summary>
        /// 在这一帧中有新加入的实体或者system
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDirty()
        {
            return true;
        }

    }
}
