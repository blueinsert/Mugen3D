using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public abstract class BoundBox : Geometry
    {
        public const int LEFT = 1;
        public const int RIGHT = 1<<1;
        public const int TOP = 1<<2;
        public const int DOWN = 1<<3;
        public const int FRONT = 1<<4;
        public const int BACK = 1<<5;

       protected static Vector4[] _points = new Vector4[8] { 
            new Vector4(-0.5f, -0.5f, -0.5f, 1),
            new Vector4(-0.5f, -0.5f, 0.5f, 1),
            new Vector4(0.5f, -0.5f, 0.5f, 1),
            new Vector4(0.5f, -0.5f, -0.5f, 1),
            new Vector4(-0.5f, 0.5f, -0.5f, 1),
            new Vector4(-0.5f, 0.5f, 0.5f, 1),
            new Vector4(0.5f, 0.5f, 0.5f, 1),
            new Vector4(0.5f, 0.5f, -0.5f, 1)
        };

       public abstract Vector3 GetCenter();
   
    }
}