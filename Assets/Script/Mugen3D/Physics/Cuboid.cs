using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D
{
    [System.Serializable]
    public class Cuboid
    {
        static Vector4[] _points = new Vector4[8] { 
            new Vector4(-0.5f, -0.5f, -0.5f, 1),
            new Vector4(-0.5f, -0.5f, 0.5f, 1),
            new Vector4(0.5f, -0.5f, 0.5f, 1),
            new Vector4(0.5f, -0.5f, -0.5f, 1),
            new Vector4(-0.5f, 0.5f, -0.5f, 1),
            new Vector4(-0.5f, 0.5f, 0.5f, 1),
            new Vector4(0.5f, 0.5f, 0.5f, 1),
            new Vector4(0.5f, 0.5f, -0.5f, 1)
        };

        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public Transform parent;

        public Matrix4x4 TransformMatrix
        {
            get
            {
                Matrix4x4 m1 = parent == null ? Matrix4x4.identity : parent.localToWorldMatrix;
                Quaternion qua = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
                Matrix4x4 m2 = new Matrix4x4();
                m2.SetTRS(position, qua, scale);
                return m1 * m2;
            }
        }

        public Cuboid()
        {
            position = Vector3.zero;
            rotation = Vector3.zero;
            scale = Vector3.one;
        }

        public List<Vector3> GetVertexArray()
        {
            List<Vector3> array = new List<Vector3>();
            Matrix4x4 m = TransformMatrix;
            for (int i = 0; i < _points.Length; i++)
            {
                Vector3 p = m * _points[i];
                array.Add(p);
            }
            return array;
        }

    }

}
