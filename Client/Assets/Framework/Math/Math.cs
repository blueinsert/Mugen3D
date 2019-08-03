
using System;

namespace bluebean.UGFramework {

/// <summary>
/// Contains common math operations.
/// </summary>
public sealed class Math {

    /// <summary>
    /// PI constant.
    /// </summary>
    public static Number Pi = Number.Pi;

    /**
    *  @brief PI over 2 constant.
    **/
    public static Number PiOver2 = Number.PiOver2;

    /// <summary>
    /// A small value often used to decide if numeric 
    /// results are zero.
    /// </summary>
    public static Number Epsilon = Number.Epsilon;

    /**
    *  @brief Degree to radians constant.
    **/
    public static Number Deg2Rad = Number.Deg2Rad;

    /**
    *  @brief Radians to degree constant.
    **/
    public static Number Rad2Deg = Number.Rad2Deg;

    /// <summary>
    /// Gets the square root.
    /// </summary>
    /// <param name="number">The number to get the square root from.</param>
    /// <returns></returns>
#region public static Number Sqrt(Number number)
    public static Number Sqrt(Number number) {
        return Number.Sqrt(number);
    }
#endregion

    /// <summary>
    /// Gets the maximum number of two values.
    /// </summary>
    /// <param name="val1">The first value.</param>
    /// <param name="val2">The second value.</param>
    /// <returns>Returns the largest value.</returns>
#region public static Number Max(Number val1, Number val2)
    public static Number Max(Number val1, Number val2) {
        return (val1 > val2) ? val1 : val2;
    }
#endregion

    /// <summary>
    /// Gets the minimum number of two values.
    /// </summary>
    /// <param name="val1">The first value.</param>
    /// <param name="val2">The second value.</param>
    /// <returns>Returns the smallest value.</returns>
#region public static Number Min(Number val1, Number val2)
    public static Number Min(Number val1, Number val2) {
        return (val1 < val2) ? val1 : val2;
    }
#endregion

    /// <summary>
    /// Gets the maximum number of three values.
    /// </summary>
    /// <param name="val1">The first value.</param>
    /// <param name="val2">The second value.</param>
    /// <param name="val3">The third value.</param>
    /// <returns>Returns the largest value.</returns>
#region public static Number Max(Number val1, Number val2,Number val3)
    public static Number Max(Number val1, Number val2, Number val3) {
        Number max12 = (val1 > val2) ? val1 : val2;
        return (max12 > val3) ? max12 : val3;
    }
#endregion

    /// <summary>
    /// Returns a number which is within [min,max]
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped value.</returns>
#region public static Number Clamp(Number value, Number min, Number max)
    public static Number Clamp(Number value, Number min, Number max) {
        value = (value > max) ? max : value;
        value = (value < min) ? min : value;
        return value;
    }
#endregion

    /// <summary>
    /// Changes every sign of the matrix entry to '+'
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <param name="result">The absolute matrix.</param>
#region public static void Absolute(ref JMatrix matrix,out JMatrix result)
    //public static void Absolute(ref TSMatrix matrix, out TSMatrix result) {
    //    result.M11 = Number.Abs(matrix.M11);
    //    result.M12 = Number.Abs(matrix.M12);
    //    result.M13 = Number.Abs(matrix.M13);
    //    result.M21 = Number.Abs(matrix.M21);
    //    result.M22 = Number.Abs(matrix.M22);
    //    result.M23 = Number.Abs(matrix.M23);
    //    result.M31 = Number.Abs(matrix.M31);
    //    result.M32 = Number.Abs(matrix.M32);
    //    result.M33 = Number.Abs(matrix.M33);
    //}
#endregion

    /// <summary>
    /// Returns the sine of value.
    /// </summary>
    public static Number Sin(Number value) {
        return Number.Sin(value);
    }

    /// <summary>
    /// Returns the cosine of value.
    /// </summary>
    public static Number Cos(Number value) {
        return Number.Cos(value);
    }

    /// <summary>
    /// Returns the tan of value.
    /// </summary>
    public static Number Tan(Number value) {
        return Number.Tan(value);
    }

    /// <summary>
    /// Returns the arc sine of value.
    /// </summary>
    public static Number Asin(Number value) {
        return Number.Asin(value);
    }

    /// <summary>
    /// Returns the arc cosine of value.
    /// </summary>
    public static Number Acos(Number value) {
        return Number.Acos(value);
    }

    /// <summary>
    /// Returns the arc tan of value.
    /// </summary>
    public static Number Atan(Number value) {
        return Number.Atan(value);
    }

    /// <summary>
    /// Returns the arc tan of coordinates x-y.
    /// </summary>
    public static Number Atan2(Number y, Number x) {
        return Number.Atan2(y, x);
    }

    /// <summary>
    /// Returns the largest integer less than or equal to the specified number.
    /// </summary>
    public static Number Floor(Number value) {
        return Number.Floor(value);
    }

    /// <summary>
    /// Returns the smallest integral value that is greater than or equal to the specified number.
    /// </summary>
    public static Number Ceiling(Number value) {
        return value;
    }

    /// <summary>
    /// Rounds a value to the nearest integral value.
    /// If the value is halfway between an even and an uneven value, returns the even value.
    /// </summary>
    public static Number Round(Number value) {
        return Number.Round(value);
    }

    /// <summary>
    /// Returns a number indicating the sign of a Fix64 number.
    /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
    /// </summary>
    public static int Sign(Number value) {
        return Number.Sign(value);
    }

    /// <summary>
    /// Returns the absolute value of a Fix64 number.
    /// Note: Abs(Fix64.MinValue) == Fix64.MaxValue.
    /// </summary>
    public static Number Abs(Number value) {
        return Number.Abs(value);                
    }

    public static Number Barycentric(Number value1, Number value2, Number value3, Number amount1, Number amount2) {
        return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
    }

    public static Number CatmullRom(Number value1, Number value2, Number value3, Number value4, Number amount) {
        // Using formula from http://www.mvps.org/directx/articles/catmull/
        // Internally using Numbers not to lose precission
        Number amountSquared = amount * amount;
        Number amountCubed = amountSquared * amount;
        return (Number)(Number.Half * (2 * value2 +
                             (value3 - value1) * amount +
                             (2 * value1 - 5 * value2 + 4 * value3 - value4) * amountSquared +
                             (3 * value2 - value1 - 3 * value3 + value4) * amountCubed));
    }

    public static Number Distance(Number value1, Number value2) {
        return Number.Abs(value1 - value2);
    }

    public static Number Hermite(Number value1, Number tangent1, Number value2, Number tangent2, Number amount) {
        // All transformed to Number not to lose precission
        // Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
        Number v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
        Number sCubed = s * s * s;
        Number sSquared = s * s;

        if (amount == 0)
            result = value1;
        else if (amount == 1)
            result = value2;
        else
            result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
                     (3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
                     t1 * s +
                     v1;
        return (Number)result;
    }

    public static Number Lerp(Number value1, Number value2, Number amount) {
        return value1 + (value2 - value1) * amount;
    }

    public static Number SmoothStep(Number value1, Number value2, Number amount) {
        // It is expected that 0 < amount < 1
        // If amount < 0, return value1
        // If amount > 1, return value2
        Number result = Clamp(amount, 0, 1);
        result = Hermite(value1, 0, value2, 0, result);
        return result;
    }

}

}