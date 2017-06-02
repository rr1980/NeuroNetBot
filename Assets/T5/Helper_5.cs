using UnityEngine;
public static class Helper_5
{
    public static Vector3 GetSecLine(GameObject go,float angleFrom, float scanRange)
    {
        var d = go.transform.TransformDirection(Quaternion.AngleAxis(angleFrom, Vector3.up) * Vector3.forward) * scanRange;
        d.y = 1;
        return d; 
    }

    public static float GetAngleFromVector(GameObject go,Vector3 target)
    {
        var localTarget = go.transform.InverseTransformPoint(target);
        var angleToTarget = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        if (angleToTarget < 0)
        {
            angleToTarget += 360;
        }
        return angleToTarget;
    }
}