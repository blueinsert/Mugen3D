using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class LuaScriptSystem : SystemBase
    {
        public LuaScriptSystem(WorldBase world) : base(world) { }

        protected override bool Filter(Entity e)
        {
            return e.GetComponent<LuaScriptComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            foreach(var entity in entities)
            {
                var luaScriptComponent = entity.GetComponent<LuaScriptComponent>();
                luaScriptComponent.Update();
            }
        }
    }
}
