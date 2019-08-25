using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.Core
{
    public class WorldBase
    {
        protected int frameCount = 0;
        private int m_maxEntityId = 0;

        /// <summary>
        /// 实体字典
        /// </summary>
        private readonly Dictionary<int, Entity> m_entityDic = new Dictionary<int, Entity>();
        /// <summary>
        /// 系统列表
        /// </summary>
        private readonly List<SystemBase> m_systemList = new List<SystemBase>();
        /// <summary>
        /// 单例组件字典
        /// </summary>
        private readonly Dictionary<string, ComponentBase> m_singletonComponentDic = new Dictionary<string, ComponentBase>();

        /// <summary>
        /// 添加System
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddSystem<T>() where T : SystemBase
        {
            var ins = System.Activator.CreateInstance(typeof(T),new object[] { this}) as SystemBase;
            m_systemList.Add(ins);
        }

        /// <summary>
        /// 获取系统
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSystem<T>()where T : SystemBase
        {
            foreach(var system in m_systemList)
            {
                var res = system as T;
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }

        /// <summary>
        /// 添加单例组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected T AddSingletonComponent<T>() where T : ComponentBase,new()
        {
            T res = GetSingletonComponent<T>();
            if (res != null)
            {
                return res;
            }
            else
            {
                var component = new T();
                m_singletonComponentDic.Add(component.GetType().Name, component);
                return component;
            }
        }
        
        /// <summary>
        /// 获取单例组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSingletonComponent<T>() where T : ComponentBase, new()
        {
            ComponentBase res;
            if (m_singletonComponentDic.TryGetValue(typeof(T).Name, out res))
            {
                return res as T;
            }
            return null;
        }

        protected Entity AddEntity()
        {
            var entity = new Entity(m_maxEntityId++, this);
            m_entityDic.Add(entity.ID, entity);
            return entity;
        }

        public Entity GetEntity(int id)
        {
            return m_entityDic[id];
        }

        protected void RemoveEntity(int id)
        {
            m_entityDic.Remove(id);
        }

        protected void RemoveEntity(Entity entity)
        {
            RemoveEntity(entity.ID);
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
            foreach (var system in m_systemList)
            {
                system.Process();
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
