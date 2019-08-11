using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math = FixPointMath.Math;
using Vector = FixPointMath.Vector;
using Number = FixPointMath.Number;

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

    public static Vector2 ToVector2(this Vector v)
    {
        return new Vector2(v.x.AsFloat(), v.y.AsFloat());
    }

    public static Vector3 ToVector3(this Vector v)
    {
        return new Vector3(v.x.AsFloat(), v.y.AsFloat(), 0);
    }

    
    public static Number ToNumber(this float v)
    {
        return new Number((int)(v * 1000)) / new Number(1000);
    }

    public static Number ToNumber(this double v)
    {
        return new Number((int)(v * 1000)) / new Number(1000);
    }

    public static Number X(this Number[] array)
    {
        if (array.Length >= 1)
            return array[0];
        return 0;
    }

    public static Number Y(this Number[] array)
    {
        if (array.Length >= 2)
            return array[1];
        return 0;
    }

    public static Number Z(this Number[] array)
    {
        if (array.Length >= 3)
            return array[2];
        return 0;
    }

    public static UnityEngine.Vector3 AsVector3(this Number[] array)
    {
        Vector3 v = Vector3.zero;
        for(int i = 0; i < array.Length; i++)
        {
            switch (i)
            {
                case 0:
                    v.x = array[i].AsFloat();break;
                case 1:
                    v.y = array[i].AsFloat();break;
                case 2:
                    v.z = array[i].AsFloat();break;
            }
        }
        return v;
    }

    public static Vector AsVector(this Number[] array)
    {
        Vector v = Vector.zero;
        for (int i = 0; i < array.Length; i++)
        {
            switch (i)
            {
                case 0:
                    v.x = array[i]; break;
                case 1:
                    v.y = array[i]; break;
            }
        }
        return v;
    }

}
