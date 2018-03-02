using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public abstract class Geometry
    {
        public Transform parent;

        public abstract List<Vector3> GetVertexArray();
        public abstract Matrix4x4 GetTransformMatrix();
        
    }
}
