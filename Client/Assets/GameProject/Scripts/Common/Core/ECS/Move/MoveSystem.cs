﻿using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class MoveSystem : SystemBase
    {
        
        protected override bool Filter(Entity e)
        {
            return e.GetComponent<MoveComponent>() != null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            foreach(var e in entities)
            {
                var moveComponent = e.GetComponent<MoveComponent>();
                moveComponent.Update(Number.D60);
            }
        }
    }
}