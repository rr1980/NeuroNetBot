//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//[System.Serializable]
//public class Sector
//{
//    public float AngleFrom;
//    public float AngleTo;

//    [SerializeField]
//    public float AngleCenter
//    {
//        get
//        {
//            return AngleFrom + ((AngleTo - AngleFrom) / 2);
//        }
//    }

//    public List<GameObject> Foods;
//    public List<GameObject> Walls;
//    public List<GameObject> Bots;

//    public Sector(float angleFrom, float angleTo)
//    {
//        Foods = new List<GameObject>();
//        Walls = new List<GameObject>();
//        Bots = new List<GameObject>();
//        AngleFrom = angleFrom;
//        AngleTo = angleTo;
//    }
//}


//public class Bot_AI2 : MonoBehaviour
//{
//    public bool debug = false;
//    public BotController_2 Controller;
//    public float ScanRange = 15;
//    public float Speed = 1;
//    public List<Sector> Sectors = new List<Sector>();

//    public int FoodCount;

//    //[SerializeField]
//    private NeuralNet NeuralNet;


//    public Collider selfCollider;

//    void Start()
//    {
//        selfCollider = GetComponent<Collider>();

//        Controller = FindObjectOfType<BotController_2>();

//        Sectors.Add(new Sector(0, 45));
//        Sectors.Add(new Sector(45, 90));
//        Sectors.Add(new Sector(90, 135));
//        Sectors.Add(new Sector(135, 180));
//        Sectors.Add(new Sector(180, 225));
//        Sectors.Add(new Sector(225, 270));
//        Sectors.Add(new Sector(270, 315));
//        Sectors.Add(new Sector(315, 360));

//        NeuralNet = new NeuralNet(24, 8, 8);
//    }

//    void Update()
//    {
//        if (Controller.State == BotController_2.StateEnum.Run)
//        {
//            Sectors.ForEach(s => s.Foods = new List<GameObject>());
//            Sectors.ForEach(s => s.Walls = new List<GameObject>());
//            Sectors.ForEach(s => s.Bots = new List<GameObject>());

//            List<Collider> hitColliders = Physics.OverlapSphere(transform.position, ScanRange).ToList();//.Where(hc => hc.tag == tag).ToList();
//            hitColliders.Remove(selfCollider);

//            var foodsInRange = hitColliders.FirstOrDefault(hc => hc.tag == "Food" && Vector3.Distance(hc.transform.position, transform.position) < 2);
//            if (foodsInRange != null)
//            {
//                FoodCount++;
//                var x = UnityEngine.Random.Range(-20, 20);
//                var z = UnityEngine.Random.Range(-20, 20);
//                foodsInRange.gameObject.transform.position = new Vector3(x, 0.5f, z);
//            }

//            setFoodToSectors(hitColliders.Where(hc => hc.tag == "Food").ToList());
//            setWallsToSectors(hitColliders.Where(hc => hc.tag == "Wall").ToList());
//            setBotsToSectors(hitColliders.Where(hc => hc.tag == "Bot").ToList());

//            setNN();
//        }
//    }

//    private void setWallsToSectors(List<Collider> hitColliders)
//    {
//        foreach (var item in hitColliders)
//        {
//            float angleToTarget = getAngleToTarget(item);
//            if (angleToTarget < 0)
//            {
//                angleToTarget += 360;
//            }

//            var sector = Sectors.FirstOrDefault(s => s.AngleFrom < angleToTarget && s.AngleTo > angleToTarget);

//            if (sector != null)
//            {
//                sector.Walls.Add(item.transform.gameObject);
//            }

//            Vector3 to = getSecLine(angleToTarget, 15);

//            if (debug || Controller.debug)
//            {
//                Debug.DrawLine(transform.position, to, Color.green);
//            }
//        }
//    }

//    private void setBotsToSectors(List<Collider> hitColliders)
//    {
//        foreach (var item in hitColliders)
//        {
//            float angleToTarget = getAngleToTarget(item);
//            if (angleToTarget < 0)
//            {
//                angleToTarget += 360;
//            }

//            var sector = Sectors.FirstOrDefault(s => s.AngleFrom < angleToTarget && s.AngleTo > angleToTarget);

//            if (sector != null)
//            {
//                sector.Bots.Add(item.transform.gameObject);
//            }

//            Vector3 to = getSecLine(angleToTarget, 15);

//            if (debug || Controller.debug)
//            {
//                Debug.DrawLine(transform.position, to, Color.green);
//            }
//        }
//    }

//    private void setFoodToSectors(List<Collider> hitColliders)
//    {
//        foreach (var item in hitColliders)
//        {
//            float angleToTarget = getAngleToTarget(item);
//            if (angleToTarget < 0)
//            {
//                angleToTarget += 360;
//            }

//            var sector = Sectors.FirstOrDefault(s => s.AngleFrom < angleToTarget && s.AngleTo > angleToTarget);

//            if (sector != null)
//            {
//                sector.Foods.Add(item.transform.gameObject);
//            }

//            Vector3 to = getSecLine(angleToTarget, 15);

//            if (debug || Controller.debug)
//            {
//                Debug.DrawLine(transform.position, to, Color.green);
//            }
//        }
//    }

//    private void setNN()
//    {
//        double[] input = new double[24];

//        for (int i = 0; i < 7; i++)
//        {
//            input[i] = Sectors[i].Foods.Any() == true ? 1 : 0;
//        }

//        for (int i = 8; i < 15; i++)
//        {
//            input[i] = Sectors[i - 8].Walls.Any() == true ? 1 : 0;
//        }

//        for (int i = 16; i < 23; i++)
//        {
//            input[i] = Sectors[i - 16].Bots.Any() == true ? 1 : 0;
//        }

//        var xr = NeuralNet.Compute(input);

//        Vector3 diradd = new Vector3();

//        for (int i = 0; i < Sectors.Count; i++)
//        {
//            if (xr[i] > 0.5)
//            {
//                var ttt = getSecLineRaw(Sectors[i].AngleCenter, 0.01f);
//                diradd += ttt;
//            }
//        }
//        transform.position += diradd*Speed;
//    }


//    private float getAngleToTarget(Collider item)
//    {
//        var localTarget = transform.InverseTransformPoint(item.transform.position);
//        var angleToTarget = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
//        return angleToTarget;
//    }

//    private void OnDrawGizmos()
//    {
//        if (Application.isPlaying)
//        {
//            if (debug || Controller.debug)
//            {
//                DebugExtension.DrawCircle(transform.position, ScanRange);

//                IEnumerable<Sector> secs = new List<Sector>();
//                if (debug)
//                {
//                    secs = Sectors;
//                }
//                else
//                {
//                    secs = Sectors.Where(s => s.Foods.Any());
//                }

//                foreach (var sec in secs)
//                {
//                    Vector3 from = getSecLine(sec.AngleFrom, ScanRange);
//                    Vector3 to = getSecLine(sec.AngleTo, ScanRange);
//                    Color col = Color.green;
//                    if (sec.Foods.Any())
//                    {
//                        col = Color.red;
//                    }
//                    Debug.DrawLine(transform.position, from, col);
//                    Debug.DrawLine(transform.position, to, col);
//                }
//            }
//        }
//    }

//    private Vector3 getSecLine(float angle, float range)
//    {
//        var to = (AngelToVector(angle) * range) + transform.position;
//        to.y = 1;
//        return to;
//    }

//    private Vector3 getSecLineRaw(float angle, float range)
//    {
//        var to = (AngelToVector(angle) * range);
//        to.y = 0;
//        return to;
//    }

//    private Vector3 AngelToVector(float angle)
//    {
//        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 1, Mathf.Cos(Mathf.Deg2Rad * angle));
//    }

//    public string DoubleToBinaryString(double d)
//    {
//        return Convert.ToString(BitConverter.DoubleToInt64Bits(d), 2);
//    }

//    public double BinaryStringToDouble(string s)
//    {
//        return BitConverter.Int64BitsToDouble(Convert.ToInt64(s, 2));
//    }
//}
