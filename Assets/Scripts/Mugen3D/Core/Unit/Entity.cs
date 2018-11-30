using System;
using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public enum EventType
    {
        Dead = 1,
        SampleAnim,
        PlayEffect,
        PlaySound,
    }

    public class Event
    {
        public EventType type;
        public object data;
    }

    public class Entity
    {
        public int id { get; private set; }
        public Vector position { get; private set; }
        public Vector scale { get; private set; }
        public World world { get; private set; }
        public EntityConfig config { get; private set; }
        public bool isDestroyed { get; private set; }
        public Action<Event> onEvent;

        public Entity()
        {
            this.scale = Vector.one;
        }

        public void SetEntityId(int id)
        {
            this.id = id;
        }

        public void SetPosition(Vector pos)
        {
            this.position = pos;
        }

        public void SetScale(Vector scale)
        {
            this.scale = scale;
        }

        public void SetWorld(World w)
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

        public virtual void OnUpdate(Number deltaTime) { }

        public void Destroy()
        {
            isDestroyed = true;
        }     
    }
}
