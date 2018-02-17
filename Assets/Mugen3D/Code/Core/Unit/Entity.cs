using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public abstract class Entity : MonoBehaviour, Collideable
    {
        public Action<Entity, Event> onEvent;

        public abstract void Init();

        public abstract void OnUpdate();

        public bool isDestroyed = false;

        public void SendEvent(Event e)
        {
            if (onEvent != null)
            {
                onEvent(this, e);
            }
        }

        public virtual Collider[] GetColliders()
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            isDestroyed = true;
        }
    }
}
