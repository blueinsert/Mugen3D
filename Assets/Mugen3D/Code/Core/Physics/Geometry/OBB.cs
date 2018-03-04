using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D
{
    [System.Serializable]
    public class OBB : BoundBox
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public override Matrix4x4 GetTransformMatrix()
        {
            Matrix4x4 m1 = parent == null ? Matrix4x4.identity : parent.localToWorldMatrix;
            Quaternion qua = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            Matrix4x4 m2 = new Matrix4x4();
            m2.SetTRS(position, qua, scale);
            return m1 * m2;
        }

        public override List<Vector3> GetVertexArray()
        {
            List<Vector3> array = new List<Vector3>();
            Matrix4x4 m = GetTransformMatrix();
            for (int i = 0; i < _points.Length; i++)
            {
                Vector3 p = m * _points[i];
                array.Add(p);
            }
            return array;
        }

        public override Vector3 GetCenter()
        {
            return GetTransformMatrix() * new Vector3(0, 0, 0);
        }
    }

}
