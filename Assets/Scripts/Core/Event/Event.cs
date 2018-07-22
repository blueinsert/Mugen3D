using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public enum EventType
    {
        Dead = 1,
        SampleAnim = 2,
    }

    public class Event
    {
        public EventType type;
        public object data;     
    }
}
