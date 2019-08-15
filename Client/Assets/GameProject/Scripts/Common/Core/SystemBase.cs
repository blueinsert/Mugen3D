using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class SystemBase
    {
        protected virtual void ProcessEntity(IComponentOwner componentOwner)
        {

        }

        protected BattleWorld world;

        public SystemBase(BattleWorld world)
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
