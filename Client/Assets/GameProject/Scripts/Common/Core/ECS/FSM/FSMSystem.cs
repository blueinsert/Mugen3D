using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 角色状态机系统
    /// </summary>
    public class FSMSystem : SystemBase
    {
        protected override bool Filter(Entity e)
        {
            return e.GetComponent<FSMComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            foreach(var entity in entities)
            {
                var fsmComponent = entity.GetComponent<FSMComponent>();
                fsmComponent.Update();
            }
        }
    }
}
