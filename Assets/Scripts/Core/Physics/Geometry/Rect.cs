using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    [System.Serializable]
    public class Rect : Geometry
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

    }
}

