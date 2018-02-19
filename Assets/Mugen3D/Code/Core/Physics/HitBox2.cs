using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public enum HitPart
    {
        Hand_L = 1 << 1,
        Hand_R = 1 << 2,
        Foot_L = 1 << 3,
        Foot_R = 1 << 4,
    }

    [System.Serializable]
    public class HitBox2
    {
        public HitPart hitPart;
        public OBBCollider collider;
    }
}
