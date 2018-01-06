using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public interface Collideable
    {
        Collider[] GetCollider();
    }
}
