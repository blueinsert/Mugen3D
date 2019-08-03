using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class BaseEngine
    {
        protected World world;

        public BaseEngine(World world)
        {
            this.world = world;
            world.onAddEntity += OnAddEntity;
            world.onRemoveEntity += OnRemoveEntity;
        }

        protected virtual void OnAddEntity(Entity e)
        {
       
        }

        protected virtual void OnRemoveEntity(Entity e)
        {
         
        }

        public virtual void Update()
        {

        }
    }
}
