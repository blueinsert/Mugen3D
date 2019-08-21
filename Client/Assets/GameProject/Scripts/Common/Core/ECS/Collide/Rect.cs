using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    [System.Serializable]
    public class Rect
    {
        public Vector position;
        public Number width;
        public Number height;

        public Vector LeftUp
        {
            get
            {
                return position + new Vector(-width / 2, height / 2);
            }
        }

        public Vector RightUp
        {
            get
            {
                return position + new Vector(width / 2, height / 2);
            }
        }

        public Vector RightDown
        {
            get
            {
                return position + new Vector(width / 2, -height / 2);
            }
        }

        public Vector LeftDown
        {
            get
            {
                return position + new Vector(-width / 2, -height / 2);
            }
        }

        public Number XMin
        {
            get
            {
                return position.x - width / 2;
            }
        }

        public Number XMax
        {
            get
            {
                return position.x + width / 2;
            }
        }

        public Number YMin
        {
            get
            {
                return position.y - height / 2;
            }
        }

        public Number YMax
        {
            get
            {
                return position.y + height / 2;
            }
        }

        public Rect()
        {
            this.position = Vector.zero;
            this.width = Number.Zero;
            this.height = Number.Zero;
        }

        public Rect(Vector p, Number width, Number height)
        {
            this.position = p;
            this.width = width;
            this.height = height;
        }
     
        public Rect(Vector p1, Vector p2)
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

        public static bool IsOverlap(Rect rect1, Rect rect2){
            return !((rect1.XMin > rect2.XMax || rect2.XMin > rect1.XMax) || (rect1.YMin > rect2.YMax || rect2.YMin > rect1.YMax));
        }

        public bool IsOverlap(Rect rect)
        {
            return IsOverlap(this, rect);
        }

    }
}

