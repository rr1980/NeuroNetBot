using UnityEngine;
[System.Serializable]
public class Target_12
{
    public string Tag;
    public string SensorName;
    internal GameObject goTarget;

    private GameObject goSelf;
    private Sensor_12 Sensor;

    public float Distance
    {
        get
        {
            if (goTarget != null && goSelf != null)
            {
                return Vector3.Distance(goTarget.transform.position, goSelf.transform.position);
            }
            else
            {
                return -10;
            }
        }
    }

    public float DistanceValue
    {
        get
        {
            if (goTarget != null && goSelf != null)
            {
                var v = Sensor.sc.ScanRange - Vector3.Distance(goTarget.transform.position, goSelf.transform.position);
                return v * 2;
                //return v;
            }
            else
            {
                return -goSelf.GetComponentInChildren<SensorController_12>().ScanRange;
            }
        }
    }

    public Vector3 Position
    {
        get
        {
            if (goTarget == null)
            {
                return Vector3.zero;
            }
            return goTarget.transform.position;
        }
    }

    public float Angle
    {
        get
        {
            return Helper_5.GetAngleFromVector(goSelf, goTarget.transform.position);
        }
    }

    public Target_12(Sensor_12 sen, GameObject goS, GameObject goT)
    {
        Sensor = sen;
        goSelf = goS;
        goTarget = goT;
        Tag = goTarget.tag;
        SensorName = sen.Name;
    }
}