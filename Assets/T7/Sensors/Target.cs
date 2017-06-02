using UnityEngine;

[System.Serializable]
public class Target
{
    public string Name;
    public string SensorName;
    internal GameObject goTarget;

    private GameObject goSelf;
    private Sensor_8 Sensor;

    public float Distance
    {
        get
        {
            return Vector3.Distance(goTarget.transform.position, goSelf.transform.position);
        }
    }

    public Vector3 Position
    {
        get
        {
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

    public Target(Sensor_8 sen,GameObject goS, GameObject goT)
    {
        this.Sensor = sen;
        goSelf = goS;
        goTarget = goT;
        Name = goTarget.name;
        SensorName = sen.gameObject.name;
    }
}