using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class DVector3
    {
        public double x, y, z;

        public DVector3(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public Vector3 ToUnityVector3()
        {
            return new Vector3((float)x, (float)y, (float)z);
        }

    }
}
