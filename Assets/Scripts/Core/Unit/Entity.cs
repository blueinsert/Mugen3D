using System;
using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{

    public abstract class Entity
    {
        public int id;
        public Vector position;
        public Vector scale;
        public World world;
        public Action<Entity, Event> onEvent;

        public abstract void OnUpdate(Number deltaTime);

        public bool isDestroyed = false;

        public Entity()
        {
          
        }

        public void SetEntityId(int id)
        {
            this.id = id;
        }

        public void SetWorld(World w)
        {
            this.world = w;
        }

        public void SendEvent(Event e)
        {
            if (onEvent != null)
            {
                onEvent(this, e);
            }
        }

        public void Destroy()
        {
            isDestroyed = true;
        }
    }
}
