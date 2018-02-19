using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public abstract class Collider : MonoBehaviour
    {
        public string tag;
        [HideInInspector]
        public Entity owner;
        public Color color = Color.red;

        public abstract Geometry GetGeometry();       
    }
}
