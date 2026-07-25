using UnityEngine;

public static class FacingCheck
{
    public static bool IsFacing(Transform viewer, Vector3 targetPosition, float maxAngle)
    {
        Vector3 direction = (targetPosition - viewer.position).normalized;
        float angle = Vector3.Angle(viewer.forward, direction);
        return angle <= maxAngle;
    }
}