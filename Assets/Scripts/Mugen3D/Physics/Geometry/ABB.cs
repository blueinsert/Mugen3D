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

    public override Vector3 GetCenter()
    {
        Vector3 translate1 = parent == null ? Vector3.zero : parent.position;
        Vector3 translate2 = offset;
        return translate1 + translate2;
    }

    public List<Vector3> GetVertexes(int side){
        Dictionary<int, int> indexes = new Dictionary<int, int>();
        if ((side & BoundBox.LEFT) != 0)
        {
            indexes[0] = 0;
            indexes[1] = 0;
            indexes[5] = 0;
            indexes[4] = 0;
        }
        if ((side & BoundBox.RIGHT) != 0)
        {
            indexes[3] = 0;
            indexes[7] = 0;
            indexes[2] = 0;
            indexes[6] = 0;
        }
        if ((side & BoundBox.TOP) != 0)
        {
            indexes[4] = 0;
            indexes[5] = 0;
            indexes[6] = 0;
            indexes[7] = 0;
        }
        if ((side & BoundBox.DOWN) != 0)
        {
            indexes[0] = 0;
            indexes[1] = 0;
            indexes[2] = 0;
            indexes[3] = 0;
        }
        if ((side & BoundBox.FRONT) != 0)
        {
            indexes[1] = 0;
            indexes[2] = 0;
            indexes[6] = 0;
            indexes[5] = 0;
        }
        if ((side & BoundBox.BACK) != 0)
        {
            indexes[0] = 0;
            indexes[3] = 0;
            indexes[7] = 0;
            indexes[4] = 0;
        }
        List<Vector3> result = new List<Vector3>();
        Matrix4x4 m = GetTransformMatrix();
        foreach(var kv in indexes)
        {
            Vector3 p = m * _points[kv.Key];
            result.Add(p);
        }
        return result;
    }
}
}
