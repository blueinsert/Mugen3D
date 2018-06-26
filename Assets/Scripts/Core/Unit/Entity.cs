using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public abstract class Entity : MonoBehaviour
    {
        public int id;
        public Action<Entity, Event> onEvent;

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
    }
}
