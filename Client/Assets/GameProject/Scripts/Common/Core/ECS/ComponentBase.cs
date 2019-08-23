using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public abstract class ComponentBase
    {
        public WorldBase World { get { return m_world; } }
        protected WorldBase m_world;

        public ComponentBase(WorldBase world)
        {
            m_world = world;
        }
    }
}