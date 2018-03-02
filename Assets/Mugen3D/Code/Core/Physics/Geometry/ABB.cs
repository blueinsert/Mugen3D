using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D{

[System.Serializable]
public class ABB : BoundBox  {
    public Vector3 offset;
    public Vector3 size;

    public override Matrix4x4 GetTransformMatrix()
    {
        Vector3 translate1 = parent == null ? Vector3.zero : parent.position;
        Vector3 translate2 = offset;
        Matrix4x4 m = new Matrix4x4();
        m.SetTRS(translate1 + translate2, Quaternion.identity, size);
        return m;
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

}
}
