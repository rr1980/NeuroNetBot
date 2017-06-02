using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{

    public static Vector3 GetSecLine(float angle, float range)
    {
        angle = angle - 90;
        var to = (AngelToVector(angle) * range);
        to.y = 1;
        return to;
    }

    //public static Vector3 GetSecLine(GameObject obj,float angle, float range)
    //{
    //    angle = angle - 90;
    //    var to = Mathf.Asin(Vector3.Cross(cam.forward, weapon.forward).y) * Mathf.Rad2Deg;
    //    to.y = 1;
    //    return to;
    //}

    public static Vector3 AngelToVector(float angle)
    {
        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 1, Mathf.Cos(Mathf.Deg2Rad * angle));
    }

    public static float GetAngleToTarget(GameObject self, Vector3 item)
    {
        var localTarget = self.transform.InverseTransformPoint(item);
        var angleToTarget = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        if (angleToTarget < 0)
        {
            angleToTarget += 360;
        }
        //angleToTarget = angleToTarget + 90;
        return angleToTarget;
    }

    public static float GetAngleToTarget(GameObject self, GameObject item)
    {
        return GetAngleToTarget(self, item.transform.position);
    }

}
