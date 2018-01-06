using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Rect
    {
        public static readonly int FULL = -1;
        public static readonly int LEFT = 1 << 0;
        public static readonly int RIGHT = 1 << 1;
        public static readonly int TOP = 1 << 2;
        public static readonly int DOWN = 1 << 3;

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
            Vector3 bottomLeft = new Vector3(position.x, position.y, 0) + new Vector3(-width / 2, -height / 2, 0);
            Vector3 topLeft = new Vector3(position.x, position.y, 0) + new Vector3(-width / 2, height / 2, 0);
            Vector3 topRight = new Vector3(position.x, position.y, 0) + new Vector3(width / 2, height / 2, 0);
            Vector3 bottomRight = new Vector3(position.x, position.y, 0) + new Vector3(width / 2, -height / 2, 0);
            if ((side & Rect.LEFT) != 0) Gizmos.DrawLine(bottomLeft, topLeft);
            if ((side & Rect.TOP) != 0) Gizmos.DrawLine(topLeft, topRight);
            if ((side & Rect.RIGHT) != 0) Gizmos.DrawLine(topRight, bottomRight);
            if ((side & Rect.DOWN) != 0) Gizmos.DrawLine(bottomRight, bottomLeft);
        }
    }
}

