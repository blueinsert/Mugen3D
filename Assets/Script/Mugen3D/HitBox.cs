using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public enum HitBoxType
    {
        Attack,
        Defence,
        Collide
    }

    [System.Serializable]
    public class HitBox
    {
        public HitBoxType type;
        public Cuboid cuboid;
    }
}
