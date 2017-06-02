using UnityEngine;
[System.Serializable]
public class Target_9
{
    public string Tag;
    public string SensorName;
    internal GameObject goTarget;

    private GameObject goSelf;
    private Sensor_9 Sensor;

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
                var v = Sensor.SensorBank.ScanRange - Vector3.Distance(goTarget.transform.position, goSelf.transform.position);
                return v * 2;
                //return v;
            }
            else
            {
                return -goSelf.GetComponentInChildren<SensorBank_9>().ScanRange;
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

    public Target_9(Sensor_9 sen, GameObject goS, GameObject goT)
    {
        Sensor = sen;
        goSelf = goS;
        goTarget = goT;
        Tag = goTarget.tag;
        SensorName = sen.gameObject.name;
    }
}