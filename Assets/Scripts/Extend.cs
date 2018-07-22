using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

public static class Extend {

    public static Vector2 ToVector2(this Mugen3D.Core.Vector v)
    {
        return new Vector2(v.x.AsFloat(), v.y.AsFloat());
    }

    public static Mugen3D.Core.Number ToNumber(this float v)
    {
        return new Number((int)(Time.deltaTime * 1000)) / new Number(1000);
    }
}
