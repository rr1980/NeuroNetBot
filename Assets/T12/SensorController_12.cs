using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SensorController_12 : MonoBehaviour
{
    public float ScanRange = 20;

    private Bot_12 bot;

    void Start()
    {
        bot = GetComponent<Bot_12>();

        foreach (var item in bot.Sensors)
        {
            item.bot = bot;
            item.sc = this;
        }
    }

    void Update()
    {
        foreach (var item in bot.Sensors)
        {
            item.Update();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var item in bot.Sensors)
        {
            OnDrawAngle(item);
        }
    }

    public void OnDrawAngle(Sensor_12 sensor)
    {
        Vector3 from = Helper_5.GetSecLine(gameObject, sensor.AngleFrom, ScanRange);
        Vector3 to = Helper_5.GetSecLine(gameObject, sensor.AngleTo, ScanRange);
        from.y = 0;
        to.y = 0;

        Debug.DrawLine(gameObject.transform.position, from + gameObject.transform.position, Color.white);
        Debug.DrawLine(gameObject.transform.position, to + gameObject.transform.position, Color.white);
    }
}