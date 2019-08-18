using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 打击系统
    /// </summary>
    public class HitSystem : SystemBase
    {
        protected override bool Filter(Entity e)
        {
            return e.GetComponent<CollideComponent>() != null && e.GetComponent<FSMComponent>() != null;
        }
    }
}
