using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    [System.Serializable]
    public class Rect : Geometry
    {
        public Vector2 position;
        public float width;
        public float height;

        public Rect(Vector2 p, float width, float height)
        {
            this.position = p;
            this.width = width;
            this.height = height;
        }

        public void DrawGizmos(Color c, int side = -1)
        {
            Gizmos.color = c;
            var vertexes = GetVertexArray();
            Gizmos.DrawLine(vertexes[0], vertexes[1]);
            Gizmos.DrawLine(vertexes[1], vertexes[2]);
            Gizmos.DrawLine(vertexes[2], vertexes[3]);
            Gizmos.DrawLine(vertexes[3], vertexes[0]);
        }

        public override List<Vector3> GetVertexArray()
        {
            Vector3 bottomLeft = new Vector3(position.x, position.y, 0) + new Vector3(-width / 2, -height / 2, 0);
            Vector3 topLeft = new Vector3(position.x, position.y, 0) + new Vector3(-width / 2, height / 2, 0);
            Vector3 topRight = new Vector3(position.x, position.y, 0) + new Vector3(width / 2, height / 2, 0);
            Vector3 bottomRight = new Vector3(position.x, position.y, 0) + new Vector3(width / 2, -height / 2, 0);
            List<Vector3> result = new List<Vector3>();
            result.Add(bottomLeft);
            result.Add(topLeft);
            result.Add(topRight);
            result.Add(bottomRight);
            return result;
        }
    }
}

