using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mathy helper functions not found in Mathf
public class MathTGIT {
    // Parabolic arc with start, end, and height. (From this thread https://forum.unity.com/threads/generating-dynamic-parabola.211681/)
    public static Vector3 ParabolicArc(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        Vector3 travelDirection = end - start;
        Vector3 result = start + t * travelDirection;
        result.y += (-parabolicT * parabolicT + 1) * height;
        return result;
    }
}
