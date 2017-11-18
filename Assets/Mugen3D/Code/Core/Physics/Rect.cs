using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class Box2D
    {
        public Vector2 center;
        public float width;
        public float height;

        public Box2D(Vector2 center, float w, float h)
        {
            this.center = center;
            this.width = w;
            this.height = h;
        }

        public Vector3[] GetVertex() {
            List<Vector3> vs = new List<Vector3>();
            vs.Add(new Vector3(0, center.y + height / 2, center.x - width / 2));
            vs.Add(new Vector3(0, center.y + height / 2, center.x + width / 2));
            vs.Add(new Vector3(0, center.y - height / 2, center.x + width / 2));
            vs.Add(new Vector3(0, center.y - height / 2, center.x - width / 2));
            return vs.ToArray();
        }
    }
}