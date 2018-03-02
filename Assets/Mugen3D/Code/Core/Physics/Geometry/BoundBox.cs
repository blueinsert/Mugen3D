using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public abstract class BoundBox : Geometry
    {
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
    }
}