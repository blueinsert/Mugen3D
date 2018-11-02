using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

public static class Extension {
    //transform
    public static void SetPositionX(this UnityEngine.Transform transform, float value) {
        transform.position = new Vector3(value, transform.position.y, transform.position.z);
    }

    public static void SetPositionY(this UnityEngine.Transform transform, float value)
    {
        transform.position = new Vector3(transform.position.x, value, transform.position.z);
    }

    public static void SetPositionZ(this UnityEngine.Transform transform, float value)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, value);
    }

    public static Vector2 ToVector2(this Mugen3D.Core.Vector v)
    {
        return new Vector2(v.x.AsFloat(), v.y.AsFloat());
    }

    public static Vector3 ToVector3(this Mugen3D.Core.Vector v)
    {
        return new Vector3(v.x.AsFloat(), v.y.AsFloat(), v.z.AsFloat());
    }

    
    public static Mugen3D.Core.Number ToNumber(this float v)
    {
        return new Number((int)(v * 1000)) / new Number(1000);
    }

    public static Mugen3D.Core.Number ToNumber(this double v)
    {
        return new Number((int)(v * 1000)) / new Number(1000);
    }

    public static Mugen3D.Core.Number X(this Mugen3D.Core.Number[] array)
    {
        if (array.Length >= 1)
            return array[0];
        return 0;
    }

    public static Mugen3D.Core.Number Y(this Mugen3D.Core.Number[] array)
    {
        if (array.Length >= 2)
            return array[1];
        return 0;
    }

    public static Mugen3D.Core.Number Z(this Mugen3D.Core.Number[] array)
    {
        if (array.Length >= 3)
            return array[2];
        return 0;
    }

}
