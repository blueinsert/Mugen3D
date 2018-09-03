
using System;

namespace Mugen3D.Core
{

public struct Vector : IEquatable<Vector> {

#region Private Fields

    private static Vector zeroVector = new Vector(0, 0, 0);
    private static Vector oneVector = new Vector(1, 1, 0);

    private static Vector rightVector = new Vector(1, 0, 0);
    private static Vector leftVector = new Vector(-1, 0, 0);

    private static Vector upVector = new Vector(0, 1, 0);
    private static Vector downVector = new Vector(0, -1, 0);

    private static Vector frontVector = new Vector(0, 0, 1);
    private static Vector backVector = new Vector(0, 0, -1);

#endregion Private Fields

#region Public Fields

    public Number x;
    public Number y;
    public Number z;

#endregion Public Fields

#region Properties

    public static Vector zero {
        get { return zeroVector; }
    }

    public static Vector one {
        get { return oneVector; }
    }

    public static Vector right {
        get { return rightVector; }
    }

    public static Vector left {
        get { return leftVector; }
    }

    public static Vector up {
        get { return upVector; }
    }

    public static Vector down {
        get { return downVector; }
    }

    public static Vector front
    {
        get { return frontVector; }
    }

    public static Vector back
    {
        get
        {
            return backVector;
        }
    }

#endregion Properties

#region Constructors

    public Vector(Number x, Number y, Number z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector(Number value) {
        x = value;
        y = value;
        z = value;
    }

    public void Set(Number x, Number y, Number z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

#endregion Constructors

#region Public Methods
    /*
    public static void Reflect(ref Vector vector, ref Vector normal, out Vector result) {
        Number dot = Dot(vector, normal);
        result.x = vector.x - ((2 * dot) * normal.x);
        result.y = vector.y - ((2 * dot) * normal.y);
    }

    public static Vector Reflect(Vector vector, Vector normal) {
        Vector result;
        Reflect(ref vector, ref normal, out result);
        return result;
    }
    */

    public static Vector Add(Vector value1, Vector value2) {
        value1.x += value2.x;
        value1.y += value2.y;
        value1.z += value2.z;
        return value1;
    }

    public static void Add(ref Vector value1, ref Vector value2, out Vector result) {
        result.x = value1.x + value2.x;
        result.y = value1.y + value2.y;
        result.z = value1.z + value2.z;
    }

    /*
    public static Vector Barycentric(Vector value1, Vector value2, Vector value3, Number amount1, Number amount2) {
        return new Vector(
            Math.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
            Math.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
    }

    public static void Barycentric(ref Vector value1, ref Vector value2, ref Vector value3, Number amount1,
                                   Number amount2, out Vector result) {
        result = new Vector(
            Math.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
            Math.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
    }
    */

    /*
    public static Vector CatmullRom(Vector value1, Vector value2, Vector value3, Vector value4, Number amount) {
        return new Vector(
            Math.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
            Math.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
    }

    public static void CatmullRom(ref Vector value1, ref Vector value2, ref Vector value3, ref Vector value4,
                                  Number amount, out Vector result) {
        result = new Vector(
            Math.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
            Math.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
    }
    */

    public static Vector Clamp(Vector value1, Vector min, Vector max) {
        return new Vector(
            Math.Clamp(value1.x, min.x, max.x),
            Math.Clamp(value1.y, min.y, max.y),
            Math.Clamp(value1.z, min.y, max.y)
        );
    }

    public static void Clamp(ref Vector value1, ref Vector min, ref Vector max, out Vector result) {
        result = new Vector(
            Math.Clamp(value1.x, min.x, max.x),
            Math.Clamp(value1.y, min.y, max.y),
            Math.Clamp(value1.z, min.y, max.y)
       );
    }

    
    public static Number Distance(Vector value1, Vector value2) {
        Number result;
        DistanceSquared(ref value1, ref value2, out result);
        return (Number)Number.Sqrt(result);
    }


    public static void Distance(ref Vector value1, ref Vector value2, out Number result) {
        DistanceSquared(ref value1, ref value2, out result);
        result = (Number)Number.Sqrt(result);
    }

    public static Number DistanceSquared(Vector value1, Vector value2) {
        Number result;
        DistanceSquared(ref value1, ref value2, out result);
        return result;
    }

    public static void DistanceSquared(ref Vector value1, ref Vector value2, out Number result) {
        result = (value1.x - value2.x) * (value1.x - value2.x) + (value1.y - value2.y) * (value1.y - value2.y) + (value1.z - value2.z) * (value1.z - value2.z);
    }

    public static Vector Divide(Vector value1, Vector value2) {
        value1.x /= value2.x;
        value1.y /= value2.y;
        value1.z /= value2.z;
        return value1;
    }

    public static void Divide(ref Vector value1, ref Vector value2, out Vector result) {
        result.x = value1.x / value2.x;
        result.y = value1.y / value2.y;
        result.z = value2.z / value2.z;
    }

    public static Vector Divide(Vector value1, Number divider) {
        Number factor = 1 / divider;
        value1.x *= factor;
        value1.y *= factor;
        value1.z *= factor;
        return value1;
    }

    public static void Divide(ref Vector value1, Number divider, out Vector result) {
        Number factor = 1 / divider;
        result.x = value1.x * factor;
        result.y = value1.y * factor;
        result.z = value1.z * factor;
    }

    public static Number Dot(Vector value1, Vector value2) {
        return value1.x * value2.x + value1.y * value2.y + value1.z * value2.z;
    }

    public static void Dot(ref Vector value1, ref Vector value2, out Number result) {
        result = value1.x * value2.x + value1.y * value2.y + value1.z * value2.z;
    }

    public override bool Equals(object obj) {
        if (obj is Vector)
        {
            var v = (Vector)obj;
            return this.Equals(v);
        }
        else
        {
            return false;
        }
    }

    public bool Equals(Vector other) {
        return this.x == other.x && this.y == other.y && this.z == other.z;
    }

    public override int GetHashCode() {
        return (int)(x + y + z);
    }

    /*
    public static Vector Hermite(Vector value1, Vector tangent1, Vector value2, Vector tangent2, Number amount) {
        Vector result = new Vector();
        Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
        return result;
    }

    public static void Hermite(ref Vector value1, ref Vector tangent1, ref Vector value2, ref Vector tangent2,
                               Number amount, out Vector result) {
        result.x = Math.Hermite(value1.x, tangent1.x, value2.x, tangent2.x, amount);
        result.y = Math.Hermite(value1.y, tangent1.y, value2.y, tangent2.y, amount);
    }
    */

    public Number magnitude {
        get {
            Number result;
            DistanceSquared(ref this, ref zeroVector, out result);
            return Number.Sqrt(result);
        }
    }

    public static Vector ClampMagnitude(Vector vector, Number maxLength) {
        return Normalize(vector) * maxLength;
    }

    public Number LengthSquared() {
        Number result;
        DistanceSquared(ref this, ref zeroVector, out result);
        return result;
    }

    public static Vector Lerp(Vector value1, Vector value2, Number amount) {
        amount = Math.Clamp(amount, 0, 1);
        return new Vector(
            Math.Lerp(value1.x, value2.x, amount),
            Math.Lerp(value1.y, value2.y, amount),
            Math.Lerp(value1.z, value1.z, amount));
    }

    public static Vector LerpUnclamped(Vector value1, Vector value2, Number amount) {
        return new Vector(
            Math.Lerp(value1.x, value2.x, amount),
            Math.Lerp(value1.y, value2.y, amount),
            Math.Lerp(value1.z, value1.z, amount));
    }

    public static void LerpUnclamped(ref Vector value1, ref Vector value2, Number amount, out Vector result) {
        result = new Vector(
            Math.Lerp(value1.x, value2.x, amount),
            Math.Lerp(value1.y, value2.y, amount),
            Math.Lerp(value1.z, value1.z, amount));
    }

    public static Vector Max(Vector value1, Vector value2) {
        return new Vector(
            Math.Max(value1.x, value2.x),
            Math.Max(value1.y, value2.y),
            Math.Max(value1.z, value2.z));
    }

    public static void Max(ref Vector value1, ref Vector value2, out Vector result) {
        result.x = Math.Max(value1.x, value2.x);
        result.y = Math.Max(value1.y, value2.y);
        result.z = Math.Max(value1.z, value2.z);
    }

    public static Vector Min(Vector value1, Vector value2) {
        return new Vector(
            Math.Min(value1.x, value2.x),
            Math.Min(value1.y, value2.y),
            Math.Min(value1.z, value2.z));
    }

    public static void Min(ref Vector value1, ref Vector value2, out Vector result) {
        result.x = Math.Min(value1.x, value2.x);
        result.y = Math.Min(value1.y, value2.y);
        result.z = Math.Min(value1.z, value2.z);
    }

    public void Scale(Vector other) {
        this.x = x * other.x;
        this.y = y * other.y;
        this.z = z * other.z;
    }

    public static Vector Scale(Vector value1, Vector value2) {
        Vector result;
        result.x = value1.x * value2.x;
        result.y = value1.y * value2.y;
        result.z = value1.z * value2.z;
        return result;
    }

    public static Vector Multiply(Vector value1, Vector value2) {
        value1.x *= value2.x;
        value1.y *= value2.y;
        value1.z *= value2.z;
        return value1;
    }

    public static Vector Multiply(Vector value1, Number scaleFactor) {
        value1.x *= scaleFactor;
        value1.y *= scaleFactor;
        value1.z *= scaleFactor;
        return value1;
    }

    public static void Multiply(ref Vector value1, Number scaleFactor, out Vector result) {
        result.x = value1.x * scaleFactor;
        result.y = value1.y * scaleFactor;
        result.z = value1.z * scaleFactor;
    }

    public static void Multiply(ref Vector value1, ref Vector value2, out Vector result) {
        result.x = value1.x * value2.x;
        result.y = value1.y * value2.y;
        result.z = value1.z * value2.z;
    }

    public static Vector Negate(Vector value) {
        value.x = -value.x;
        value.y = -value.y;
        value.z = -value.z;
        return value;
    }

    public static void Negate(ref Vector value, out Vector result) {
        result.x = -value.x;
        result.y = -value.y;
        result.z = -value.z;
    }

    public void Normalize() {
        Normalize(ref this, out this);
    }

    public static Vector Normalize(Vector value) {
        Normalize(ref value, out value);
        return value;
    }

    public Vector normalized {
        get {
            Vector result;
            Vector.Normalize(ref this, out result);
            return result;
        }
    }

    public static void Normalize(ref Vector value, out Vector result) {
        Number factor;
        DistanceSquared(ref value, ref zeroVector, out factor);
        factor = (Number)1 / (Number)Number.Sqrt(factor);
        result.x = value.x * factor;
        result.y = value.y * factor;
        result.z = value.z * factor;
    }

    public static Vector SmoothStep(Vector value1, Vector value2, Number amount) {
        return new Vector(
            Math.SmoothStep(value1.x, value2.x, amount),
            Math.SmoothStep(value1.y, value2.y, amount),
            Math.SmoothStep(value1.z, value2.z, amount));
    }

    public static void SmoothStep(ref Vector value1, ref Vector value2, Number amount, out Vector result) {
        result = new Vector(
            Math.SmoothStep(value1.x, value2.x, amount),
            Math.SmoothStep(value1.y, value2.y, amount),
            Math.SmoothStep(value1.z, value2.z, amount));
    }

    public static Vector Subtract(Vector value1, Vector value2) {
        value1.x -= value2.x;
        value1.y -= value2.y;
        value1.z -= value2.z;
        return value1;
    }

    public static void Subtract(ref Vector value1, ref Vector value2, out Vector result) {
        result.x = value1.x - value2.x;
        result.y = value1.y - value2.y;
        result.z = value1.z - value2.z;
    }

    public static Number Angle(Vector a, Vector b) {
        return Number.Acos(a.normalized * b.normalized) * Number.Rad2Deg;
    }

    public override string ToString() {
        return string.Format("({0:f1}, {1:f1}, {2:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat());
    }

#endregion Public Methods

#region Operators

    public static Vector operator -(Vector value) {
        value.x = -value.x;
        value.y = -value.y;
        value.z = -value.z;
        return value;
    }


    public static bool operator ==(Vector value1, Vector value2) {
        return value1.x == value2.x && value1.y == value2.y && value1.z == value2.z;
    }


    public static bool operator !=(Vector value1, Vector value2) {
        return value1.x != value2.x || value1.y != value2.y || value1.z != value2.z;
    }


    public static Vector operator +(Vector value1, Vector value2) {
        value1.x += value2.x;
        value1.y += value2.y;
        value1.z += value2.z;
        return value1;
    }


    public static Vector operator -(Vector value1, Vector value2) {
        value1.x -= value2.x;
        value1.y -= value2.y;
        value1.z -= value2.z;
        return value1;
    }


    public static Number operator *(Vector value1, Vector value2) {
        return Vector.Dot(value1, value2);
    }


    public static Vector operator *(Vector value, Number scaleFactor) {
        value.x *= scaleFactor;
        value.y *= scaleFactor;
        value.z *= scaleFactor;
        return value;
    }


    public static Vector operator *(Number scaleFactor, Vector value) {
        value.x *= scaleFactor;
        value.y *= scaleFactor;
        value.z *= scaleFactor;
        return value;
    }


    public static Vector operator /(Vector value1, Vector value2) {
        value1.x /= value2.x;
        value1.y /= value2.y;
        value1.z /= value2.z;
        return value1;
    }


    public static Vector operator /(Vector value1, Number divider) {
        Number factor = 1 / divider;
        value1.x *= factor;
        value1.y *= factor;
        value1.z *= factor;
        return value1;
    }

#endregion Operators
    /*
    public static Vector Rotate(Vector origin, Vector p, Number angle) {
        angle = angle * Number.Deg2Rad;
        var s = Number.Sin(angle);
        var c = Number.Cos(angle);
        p -= origin;
        Vector result;
        result.x = p.x * c - p.y * s;
        result.y = p.x * s + p.y * c;
        return result + origin;
    }
    */
}

}