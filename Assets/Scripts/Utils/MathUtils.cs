using System.Numerics;
using UnityEngine;

public static class MathUtils
{
    /// <summary>
    /// Translates a value from one range (xMin, xMax) to another (yMin, yMax).
    /// </summary>
    /// <param name="value">The value to translate.</param>
    /// <param name="xMin">The minimum value of the starting range.</param>
    /// <param name="xMax">The maximum value of the starting range.</param>
    /// <param name="yMin">The minimum value of the target range.</param>
    /// <param name="yMax">The maximum value of the target range.</param>
    /// <returns>The value translated to the new range.</returns>
    public static float Translate(float value, float xMin, float xMax, float yMin, float yMax)
    {
        var leftHand = (value - xMin) * (yMax - yMin);
        var rightHand = xMax - xMin;
    
        return rightHand == 0.0f
            ? leftHand + yMin
            : (leftHand / rightHand) + yMin;
    }
}
