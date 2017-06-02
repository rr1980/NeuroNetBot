using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Sensor_6
{
    [ReadOnly]
    public int Id;
    public string Name;
    public float AngleFrom;
    public float AngleTo;

    [SerializeField]
    public GameObject NearstTarget
    {
        get
        {
            if (Targets.Any())
            {
                return Targets.OrderBy(t => Vector3.Distance(t.transform.position, Go.transform.position)).First();
            }
            else
            {
                return null;
            }
        }
    }

    [SerializeField]
    public float AngleCenter
    {
        get
        {
            if (AngleFrom < AngleTo)
            {
                return AngleFrom + ((AngleTo - AngleFrom) / 2);
            }
            else
            {
                var a = (AngleFrom + AngleTo);

                if (a == 360)
                {
                    return 0;
                }
                else if (a > 360)
                {
                    return (a - 360) / 2;
                }
                else if (a < 360)
                {
                    a = (a - 360) / 2;
                    a = a * -1;
                }
                return a;
            }
        }
    }

    public List<GameObject> Targets;

    internal GameObject Go;
    internal SensorController_6 Sc;

    public bool Match(float angle)
    {
        if (AngleFrom > AngleTo)
        {
            return (angle > AngleFrom) || (angle < AngleTo);
        }
        else
        {
            return (angle > AngleFrom) && (angle < AngleTo);
        }
    }

    public void OnDrawAngle()
    {
        Vector3 from = Helper_5.GetSecLine(Go, AngleFrom, Sc.ScanRange);
        Vector3 to = Helper_5.GetSecLine(Go, AngleTo, Sc.ScanRange);

        Debug.DrawLine(Go.transform.position, from + Go.transform.position, Color.white);
        Debug.DrawLine(Go.transform.position, to + Go.transform.position, Color.white);
    }

    public void OnDrawToSector()
    {
        if (Targets.Any())
        {
            Color col = Color.yellow;
            //var dist = Vector3.Distance(NearstTarget.transform.position, Go.transform.position);
            Vector3 from = Helper_5.GetSecLine(Go, AngleFrom, Sc.ScanRange);
            Vector3 to = Helper_5.GetSecLine(Go, AngleTo, Sc.ScanRange);

            Debug.DrawLine(Go.transform.position, from + Go.transform.position, col);
            Debug.DrawLine(Go.transform.position, to + Go.transform.position, col);

            Debug.DrawLine(from + Go.transform.position, to + Go.transform.position, col);
        }
    }

    //public void OnDrawToAllTargets()
    //{
        //foreach (var item in Targets)
        //{
        //    Color col = Color.blue;
        //    Vector3 toTarget = Helper_5.GetSecLine(Go, Helper_5.GetAngleFromVector(Go, item.transform.position), Vector3.Distance(item.transform.position, Go.transform.position));
        //    toTarget.y = 0;
        //    Debug.DrawLine(Go.transform.position, toTarget + Go.transform.position, getColor(item));
        //}
    //}

    public void OnDrawToNearstTarget()
    {
        Color col = Color.green;
        Vector3 toTarget = Helper_5.GetSecLine(Go, Helper_5.GetAngleFromVector(Go, NearstTarget.transform.position), Vector3.Distance(NearstTarget.transform.position, Go.transform.position));
        toTarget.y = 0;
        if (NearstTarget.tag == "Bot")
        {
            col = Color.red;
        }
        Debug.DrawLine(Go.transform.position, toTarget + Go.transform.position, col);
    }

    //private Color getColor(GameObject item)
    //{
    //    Color col = Color.white;
    //    if (item != null)
    //    {
    //        if (item.tag == "Bot")
    //        {
    //            col = Color.red;
    //        }
    //        if (item.tag == "Wall")
    //        {
    //            col = Color.white;
    //        }
    //        if (item.tag == "Food")
    //        {
    //            col = Color.green;
    //        }
    //    }

    //    return col;
    //}
}

