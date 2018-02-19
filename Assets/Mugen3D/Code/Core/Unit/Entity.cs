using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public abstract class Entity : MonoBehaviour
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

        public void Destroy()
        {
            isDestroyed = true;
        }

        public abstract Collider GetCollider(); 
    }
}
