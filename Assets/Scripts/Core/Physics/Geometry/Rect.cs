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

        public Vector2 LeftUp
        {
            get
            {
                return position + new Vector2(-width / 2, height / 2);
            }
        }

        public Vector2 RightUp
        {
            get
            {
                return position + new Vector2(width / 2, height / 2);
            }
        }

        public Vector2 RightDown
        {
            get
            {
                return position + new Vector2(width / 2, -height / 2);
            }
        }

        public Vector2 LeftDown
        {
            get
            {
                return position + new Vector2(-width / 2, -height / 2);
            }
        }

        public float xMin
        {
            get
            {
                return position.x - width / 2;
            }
        }

        public float xMax
        {
            get
            {
                return position.x + width / 2;
            }
        }

        public float yMin
        {
            get
            {
                return position.y - height / 2;
            }
        }

        public float yMax
        {
            get
            {
                return position.y + height / 2;
            }
        }

        public Rect(Vector2 p, float width, float height)
        {
            this.position = p;
            this.width = width;
            this.height = height;
        }

        public Rect(Vector2 p1, Vector2 p2)
        {
            this.position = (p1 + p2) / 2;
            this.width = p2.x - p1.x;
            this.height = p2.y - p1.y;
        }

        public Rect(Rect rect)
        {
            this.position = rect.position;
            this.width = rect.width;
            this.height = rect.height;
        }

    }
}

