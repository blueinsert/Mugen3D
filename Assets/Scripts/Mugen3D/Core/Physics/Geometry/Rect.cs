using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
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
                return position + new Vector(-width / 2, height / 2, position.z);
            }
        }

        public Vector RightUp
        {
            get
            {
                return position + new Vector(width / 2, height / 2, position.z);
            }
        }

        public Vector RightDown
        {
            get
            {
                return position + new Vector(width / 2, -height / 2, position.z);
            }
        }

        public Vector LeftDown
        {
            get
            {
                return position + new Vector(-width / 2, -height / 2, position.z);
            }
        }

        public Number xMin
        {
            get
            {
                return position.x - width / 2;
            }
        }

        public Number xMax
        {
            get
            {
                return position.x + width / 2;
            }
        }

        public Number yMin
        {
            get
            {
                return position.y - height / 2;
            }
        }

        public Number yMax
        {
            get
            {
                return position.y + height / 2;
            }
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
            return !((rect1.xMin > rect2.xMax || rect2.xMin > rect1.xMax) || (rect1.yMin > rect2.yMax || rect2.yMin > rect1.yMax));
        }

        public bool IsOverlap(Rect rect)
        {
            return IsOverlap(this, rect);
        }

    }
}

