using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    [System.Serializable]
    public class AABB : Geometry
    {
        public Vector3 center = Vector3.zero;
        public Vector3 size = Vector3.one;
     
        public override List<Vector3> GetVertexArray()
        {
            List<Vector3> result = new List<Vector3>();
            float length = size.x;
            float height = size.y;
            float width = size.z;
            Vector3 dirX = new Vector3(1, 0, 0);
            Vector3 dirY = new Vector3(0, 1, 0);
            Vector3 dirZ = new Vector3(0, 0, 1);
            result.Add(center - length / 2 * dirX - height / 2 * dirY - width / 2 * dirZ);
            result.Add(center - length / 2 * dirX - height / 2 * dirY + width / 2 * dirZ);
            result.Add(center + length / 2 * dirX - height / 2 * dirY + width / 2 * dirZ);
            result.Add(center + length / 2 * dirX - height / 2 * dirY - width / 2 * dirZ);
            result.Add(center - length / 2 * dirX + height / 2 * dirY - width / 2 * dirZ);
            result.Add(center - length / 2 * dirX + height / 2 * dirY + width / 2 * dirZ);
            result.Add(center + length / 2 * dirX + height / 2 * dirY + width / 2 * dirZ);
            result.Add(center + length / 2 * dirX + height / 2 * dirY - width / 2 * dirZ);
            return result;
        }
    }
}
