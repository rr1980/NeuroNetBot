using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Sensor_12
{
    public string Name;
    public float AngleFrom;
    public float AngleTo;
    public List<Target_12> Targets = new List<Target_12>();

    internal Bot_12 bot;
    internal SensorController_12 sc;

    public void Update()
    {
        Targets = new List<Target_12>();
        Scan();
    }

    private void Scan()
    {
        var from = AngleFrom;
        var to = AngleTo > from ? AngleTo : 360;
        //var to = AngleTo > from ? AngleTo + AngleFrom > 360 ? 360 : AngleFrom + AngleTo : 360;

        for (float i = from; i <= to; i++)
        {
            Vector3 toTarget = Helper_5.GetSecLine(bot.gameObject, i, sc.ScanRange);
            toTarget.y = 0;

            RaycastHit hit;
            if (Physics.Raycast(bot.transform.position, toTarget, out  hit, sc.ScanRange))
            {
                if (hit.transform.root.gameObject != bot.transform.root.gameObject)
                {
                    //hits.Add(hit);
                    if (Targets.FirstOrDefault(t => t.goTarget == hit.transform.gameObject) == null)
                    {
                        Targets.Add(new Target_12(this, bot.gameObject, hit.transform.gameObject));
                    }
                }
            }

            if (i == 360)
            {
                i = 0;
                to = AngleTo;
            }
        }
    }
}
