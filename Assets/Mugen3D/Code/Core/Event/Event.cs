using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public enum EventType
    {
        Dead = 1,
    }

    public class Event
    {
        public EventType type;
        public object data;     
    }
}
