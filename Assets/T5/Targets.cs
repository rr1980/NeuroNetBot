using System;

[System.Serializable]
public class Targets
{
    public string Tag;
    public string Name;
    public int SensorId;

    public Targets(string tag, Sensor_5 sensor)
    {
        Tag = tag;
        Name = sensor.Name;
        SensorId = sensor.Id;
    }

    internal static object FirstOrDefault(Func<object, bool> p)
    {
        throw new NotImplementedException();
    }
}