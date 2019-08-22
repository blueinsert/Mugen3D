﻿using System;
using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class Entity
    {
        public int ID { get { return m_id; } }

        private int m_id;

        private readonly Dictionary<string, ComponentBase> m_componentDic = new Dictionary<string, ComponentBase>();

        public Entity(int id)
        {
            m_id = id;
        }

        public T GetComponent<T>() where T : ComponentBase
        {
            ComponentBase res;
            if(m_componentDic.TryGetValue(typeof(T).Name, out res))
            {
                return res as T;
            }
            return null;
        }

        public T AddComponent<T>() where T : ComponentBase,new()
        {
            var component = new T();
            m_componentDic.Add(component.GetType().Name, component);
            return component;
        }
    }
}