using System;
using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public enum EventType
    {
        Dead = 1,
        SampleAnim,
        PlayEffect,
        PlaySound,
        HitCountChange,
        //
        OnMatchStart,
        OnMatchEnd,
        OnRoundStart,
        OnRoundEnd,
        OnRoundStateChange,
    }

    public class Event
    {
        public EventType type;
        public object data;
    }

    public class Entity : IComponentOwner
    {
        public int id { get; private set; }
        public Vector Position { get; private set; }
        public Vector scale { get; private set; }
        public BattleWorld world { get; private set; }
        public EntityConfig config { get; private set; }
        public bool isDestroyed { get; private set; }
        public Action<Event> onEvent;

        private readonly Dictionary<string, ComponentBase> m_componentDic = new Dictionary<string, ComponentBase>();

        public Entity()
        {
            this.scale = Vector.one;
        }

        public void SetEntityId(int id)
        {
            this.id = id;
        }

        public virtual void SetPosition(Vector pos)
        {
            this.Position = pos;
        }

        public void SetScale(Vector scale)
        {
            this.scale = scale;
        }

        public void SetWorld(BattleWorld w)
        {
            this.world = w;
        }

        public void SendEvent(Event e)
        {
            if (onEvent != null)
            {
                onEvent(e);
            }
        }

        protected void SetConfig(EntityConfig config)
        {
            this.config = config;
        }

        public virtual void OnUpdate() { }

        public void Destroy()
        {
            isDestroyed = true;
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
