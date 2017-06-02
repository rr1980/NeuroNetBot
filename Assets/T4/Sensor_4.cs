//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//[System.Serializable]
//public class Sensor_4
//{
//    public int Id;
//    public float AngleFrom;
//    public float AngleTo;

//    [SerializeField]
//    public GameObject NearstTarget
//    {
//        get
//        {
//            if (Targets.Any())
//            {
//                return Targets.OrderBy(t => Vector3.Distance(t.transform.position, self.transform.position)).FirstOrDefault();
//            }
//            else
//            {
//                return null;
//            }
//        }
//    }

//    [SerializeField]
//    public float AngleCenter
//    {
//        get
//        {
//            if (AngleFrom < AngleTo)
//            {
//                return AngleFrom + ((AngleTo - AngleFrom) / 2);
//            }
//            else
//            {
//                var a = (AngleFrom + AngleTo);

//                if (a == 360)
//                {
//                    return 0;
//                }
//                else if (a > 360)
//                {
//                    return (a - 360) / 2;
//                }
//                else if (a < 360)
//                {
//                    a = (a - 360) / 2;
//                    a = a * -1;
//                }
//                return a;
//            }
//        }
//    }

//    public List<GameObject> Targets;

//    private GameObject self;

//    public Sensor_4(int i, GameObject go, float angleFrom, float angleTo)
//    {
//        Id = i;
//        self = go;
//        Targets = new List<GameObject>();
//        AngleFrom = angleFrom;
//        AngleTo = angleTo;
//    }

//    public bool Match(float angle)
//    {
//        if (AngleFrom > AngleTo)
//        {
//            return (angle > AngleFrom) || (angle < AngleTo);
//        }
//        else
//        {
//            return (angle > AngleFrom) && (angle < AngleTo);
//        }
//    }
//}
