
using System;

namespace bluebean.UGFramework
{

    public struct Vector : IEquatable<Vector>
    {

        #region Private Fields

        private static Vector zeroVector = new Vector(0, 0);
        private static Vector oneVector = new Vector(1, 1);

        private static Vector rightVector = new Vector(1, 0);
        private static Vector leftVector = new Vector(-1, 0);

        private static Vector upVector = new Vector(0, 1);
        private static Vector downVector = new Vector(0, -1);

        #endregion Private Fields


        public Number x { get; set; }
        public Number y { get; set; }


        #region Properties

        public static Vector zero
        {
            get { return zeroVector; }
        }

        public static Vector one
        {
            get { return oneVector; }
        }

        public static Vector right
        {
            get { return rightVector; }
        }

        public static Vector left
        {
            get { return leftVector; }
        }

        public static Vector up
        {
            get { return upVector; }
        }

        public static Vector down
        {
            get { return downVector; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor foe standard 2D vector.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Single"/>
        /// </param>
        /// <param name="y">
        /// A <see cref="System.Single"/>
        /// </param>
        public Vector(Number x, Number y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor for "square" vector.
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.Single"/>
        /// </param>
        public Vector(Number value)
        {
            x = value;
            y = value;
        }

        public void Set(Number x, Number y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion Constructors

        #region Public Methods

        public static void Reflect(ref Vector vector, ref Vector normal, ref Vector result)
        {
            Number dot = Dot(vector, normal);
            result.x = vector.x - ((2 * dot) * normal.x);
            result.y = vector.y - ((2 * dot) * normal.y);
        }

        public static Vector Reflect(Vector vector, Vector normal)
        {
            Vector result = default(Vector);
            Reflect(ref vector, ref normal, ref result);
            return result;
        }

        public static Vector Add(Vector value1, Vector value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static void Add(ref Vector value1, ref Vector value2, ref Vector result)
        {
            result.x = value1.x + value2.x;
            result.y = value1.y + value2.y;
        }

        public static Vector Barycentric(Vector value1, Vector value2, Vector value3, Number amount1, Number amount2)
        {
            return new Vector(
                Math.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                Math.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static void Barycentric(ref Vector value1, ref Vector value2, ref Vector value3, Number amount1,
                                        Number amount2, ref Vector result)
        {
            result = new Vector(
                Math.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                Math.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static Vector CatmullRom(Vector value1, Vector value2, Vector value3, Vector value4, Number amount)
        {
            return new Vector(
                Math.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                Math.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static void CatmullRom(ref Vector value1, ref Vector value2, ref Vector value3, ref Vector value4,
                                        Number amount, ref Vector result)
        {
            result = new Vector(
                Math.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                Math.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static Vector Clamp(Vector value1, Vector min, Vector max)
        {
            return new Vector(
                Math.Clamp(value1.x, min.x, max.x),
                Math.Clamp(value1.y, min.y, max.y));
        }

        public static void Clamp(ref Vector value1, ref Vector min, ref Vector max, ref Vector result)
        {
            result = new Vector(
                Math.Clamp(value1.x, min.x, max.x),
                Math.Clamp(value1.y, min.y, max.y));
        }

        /// <summary>
        /// Returns Number precison distanve between two vectors
        /// </summary>
        /// <param name="value1">
        /// A <see cref="Vector"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="Vector"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Single"/>
        /// </returns>
        public static Number Distance(Vector value1, Vector value2)
        {
            Number result = default(Number);
            DistanceSquared(ref value1, ref value2, ref result);
            return (Number)Number.Sqrt(result);
        }


        public static void Distance(ref Vector value1, ref Vector value2, ref Number result)
        {
            DistanceSquared(ref value1, ref value2, ref result);
            result = (Number)Number.Sqrt(result);
        }

        public static Number DistanceSquared(Vector value1, Vector value2)
        {
            Number result = default(Number);
            DistanceSquared(ref value1, ref value2, ref result);
            return result;
        }

        public static void DistanceSquared(ref Vector value1, ref Vector value2, ref Number result)
        {
            result = (value1.x - value2.x) * (value1.x - value2.x) + (value1.y - value2.y) * (value1.y - value2.y);
        }

        /// <summary>
        /// Devide first vector with the secund vector
        /// </summary>
        /// <param name="value1">
        /// A <see cref="Vector"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="Vector"/>
        /// </param>
        /// <returns>
        /// A <see cref="Vector"/>
        /// </returns>
        public static Vector Divide(Vector value1, Vector value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }

        public static void Divide(ref Vector value1, ref Vector value2, ref Vector result)
        {
            result.x = value1.x / value2.x;
            result.y = value1.y / value2.y;
        }

        public static Vector Divide(Vector value1, Number divider)
        {
            Number factor = 1 / divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        public static void Divide(ref Vector value1, Number divider, ref Vector result)
        {
            Number factor = 1 / divider;
            result.x = value1.x * factor;
            result.y = value1.y * factor;
        }

        public static Number Dot(Vector value1, Vector value2)
        {
            return value1.x * value2.x + value1.y * value2.y;
        }

        public static void Dot(ref Vector value1, ref Vector value2, ref Number result)
        {
            result = value1.x * value2.x + value1.y * value2.y;
        }

        public override bool Equals(object obj)
        {
            return (obj is Vector) ? this == ((Vector)obj) : false;
        }

        public bool Equals(Vector other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return (int)(x + y);
        }

        public static Vector Hermite(Vector value1, Vector tangent1, Vector value2, Vector tangent2, Number amount)
        {
            Vector result = new Vector();
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, ref result);
            return result;
        }

        public static void Hermite(ref Vector value1, ref Vector tangent1, ref Vector value2, ref Vector tangent2,
                                    Number amount, ref Vector result)
        {
            result.x = Math.Hermite(value1.x, tangent1.x, value2.x, tangent2.x, amount);
            result.y = Math.Hermite(value1.y, tangent1.y, value2.y, tangent2.y, amount);
        }

        public Number magnitude
        {
            get
            {
                Number result = default(Number);
                DistanceSquared(ref this, ref zeroVector, ref result);
                return Number.Sqrt(result);
            }
        }

        public static Vector ClampMagnitude(Vector vector, Number maxLength)
        {
            return Normalize(vector) * maxLength;
        }

        public Number LengthSquared()
        {
            Number result = default(Number);
            DistanceSquared(ref this, ref zeroVector, ref result);
            return result;
        }

        public static Vector Lerp(Vector value1, Vector value2, Number amount)
        {
            amount = Math.Clamp(amount, 0, 1);

            return new Vector(
                Math.Lerp(value1.x, value2.x, amount),
                Math.Lerp(value1.y, value2.y, amount));
        }

        public static Vector LerpUnclamped(Vector value1, Vector value2, Number amount)
        {
            return new Vector(
                Math.Lerp(value1.x, value2.x, amount),
                Math.Lerp(value1.y, value2.y, amount));
        }

        public static void LerpUnclamped(ref Vector value1, ref Vector value2, Number amount, ref Vector result)
        {
            result = new Vector(
                Math.Lerp(value1.x, value2.x, amount),
                Math.Lerp(value1.y, value2.y, amount));
        }

        public static Vector Max(Vector value1, Vector value2)
        {
            return new Vector(
                Math.Max(value1.x, value2.x),
                Math.Max(value1.y, value2.y));
        }

        public static void Max(ref Vector value1, ref Vector value2, ref Vector result)
        {
            result.x = Math.Max(value1.x, value2.x);
            result.y = Math.Max(value1.y, value2.y);
        }

        public static Vector Min(Vector value1, Vector value2)
        {
            return new Vector(
                Math.Min(value1.x, value2.x),
                Math.Min(value1.y, value2.y));
        }

        public static void Min(ref Vector value1, ref Vector value2, ref Vector result)
        {
            result.x = Math.Min(value1.x, value2.x);
            result.y = Math.Min(value1.y, value2.y);
        }

        public void Scale(Vector other)
        {
            x = x * other.x;
            y = y * other.y;
        }

        public static Vector Scale(Vector value1, Vector value2)
        {
            Vector result = default(Vector);
            result.x = value1.x * value2.x;
            result.y = value1.y * value2.y;

            return result;
        }

        public static Vector Multiply(Vector value1, Vector value2)
        {
            value1.x *= value2.x;
            value1.y *= value2.y;
            return value1;
        }

        public static Vector Multiply(Vector value1, Number scaleFactor)
        {
            value1.x *= scaleFactor;
            value1.y *= scaleFactor;
            return value1;
        }

        public static void Multiply(ref Vector value1, Number scaleFactor, ref Vector result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
        }

        public static void Multiply(ref Vector value1, ref Vector value2, ref Vector result)
        {
            result.x = value1.x * value2.x;
            result.y = value1.y * value2.y;
        }

        public static Vector Negate(Vector value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }

        public static void Negate(ref Vector value, ref Vector result)
        {
            result.x = -value.x;
            result.y = -value.y;
        }

        public void Normalize()
        {
            Normalize(ref this, ref this);
        }

        public static Vector Normalize(Vector value)
        {
            Normalize(ref value, ref value);
            return value;
        }

        public Vector normalized
        {
            get
            {
                Vector result = default(Vector);
                Vector.Normalize(ref this, ref result);

                return result;
            }
        }

        public static void Normalize(ref Vector value, ref Vector result)
        {
            Number factor = default(Number);
            DistanceSquared(ref value, ref zeroVector, ref factor);
            factor = (Number)1 / (Number)Number.Sqrt(factor);
            result.x = value.x * factor;
            result.y = value.y * factor;
        }

        public static Vector SmoothStep(Vector value1, Vector value2, Number amount)
        {
            return new Vector(
                Math.SmoothStep(value1.x, value2.x, amount),
                Math.SmoothStep(value1.y, value2.y, amount));
        }

        public static void SmoothStep(ref Vector value1, ref Vector value2, Number amount, ref Vector result)
        {
            result = new Vector(
                Math.SmoothStep(value1.x, value2.x, amount),
                Math.SmoothStep(value1.y, value2.y, amount));
        }

        public static Vector Subtract(Vector value1, Vector value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static void Subtract(ref Vector value1, ref Vector value2, ref Vector result)
        {
            result.x = value1.x - value2.x;
            result.y = value1.y - value2.y;
        }

        public static Number Angle(Vector a, Vector b)
        {
            return Number.Acos(a.normalized * b.normalized) * Number.Rad2Deg;
        }

        public override string ToString()
        {
            return string.Format("({0:f1}, {1:f1})", x.AsFloat(), y.AsFloat());
        }

        #endregion Public Methods

        #region Operators

        public static Vector operator -(Vector value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }


        public static bool operator ==(Vector value1, Vector value2)
        {
            return value1.x == value2.x && value1.y == value2.y;
        }


        public static bool operator !=(Vector value1, Vector value2)
        {
            return value1.x != value2.x || value1.y != value2.y;
        }


        public static Vector operator +(Vector value1, Vector value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }


        public static Vector operator -(Vector value1, Vector value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }


        public static Number operator *(Vector value1, Vector value2)
        {
            return Vector.Dot(value1, value2);
        }


        public static Vector operator *(Vector value, Number scaleFactor)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static Vector operator *(Number scaleFactor, Vector value)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static Vector operator /(Vector value1, Vector value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }


        public static Vector operator /(Vector value1, Number divider)
        {
            Number factor = 1 / divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        #endregion Operators

        public static Vector Rotate(Vector origin, Vector p, Number angle)
        {
            angle = angle * Number.Deg2Rad;
            var s = Number.Sin(angle);
            var c = Number.Cos(angle);
            p -= origin;
            Vector result = default(Vector);
            result.x = p.x * c - p.y * s;
            result.y = p.x * s + p.y * c;
            return result + origin;
        }

    }

}