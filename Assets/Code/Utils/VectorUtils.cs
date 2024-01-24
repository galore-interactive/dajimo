using UnityEngine;

public static class VectorUtils
{
    public static bool IsDifferenceGreaterThanTolerance(Vector3 difference, float tolerance)
    {
        return (difference.x > tolerance || difference.y > tolerance || difference.z > tolerance);
    }

    public static Vector3 GetDifferenceBetweenTwoVector3(Vector3 vectorA, Vector3 vectorB)
    {
        Vector3 difference = Vector3.zero;
        difference.x = Mathf.Abs(vectorA.x - vectorB.x);
        difference.y = Mathf.Abs(vectorA.y - vectorB.y);
        difference.z = Mathf.Abs(vectorA.z - vectorB.z);
        return difference;
    }
}
