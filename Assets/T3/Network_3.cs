//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class Network_3 : MonoBehaviour
//{

//    public float RefreshTime = 0.1f;

//    [Header("Settings")]
//    public int InpuNeuron = 24;
//    public int HiddenNeuron1 = 16;   // aufteilen
//    //public int HiddenNeuron2 = 48;   // aufteilen
//    public int OutputNeuron = 8;

//    [Header("Results")]
//    public Vector3 goDirection;
//    public NeuralNet net;

//    private Sensor_3 sensor;
//    private Bot_3 bot;
//    private float elapsed;

//    void Start()
//    {
//        bot = GetComponent<Bot_3>();
//        sensor = GetComponent<Sensor_3>();
//        //floor = GameObject.FindGameObjectWithTag("Floor");
//    }

//    public void Init(NeuralNet network)
//    {
//        net = network;
//    }

//    void Update()
//    {
//        if (net.Init)
//        {
//            if (pufferTime())
//            {
//                bool randomize = false;
//                double[] input = new double[InpuNeuron];

//                var se = sensor.Sectors.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == "Food")).ToList();
//                //var se = sensor.Sectors.Where(s => s.NearstTarget.tag == "Food");
//                //var se = sensor.Sectors.Where(s => s.Targets.Any(ss => ss.tag == "Food"));
//                //se = se.OrderBy(t => Vector3.Distance(t.Targets.OrderBy(o => Vector3.Distance(o.transform.position, transform.position)).ToList()[0].transform.position, transform.position)).ToList();
//                var see = se.OrderBy(t => Vector3.Distance(t.NearstTarget.transform.position, transform.position)).FirstOrDefault();

//                for (int i = 0; i <= ((InpuNeuron / 3) - 1); i++)     // 7 bei 24
//                {
//                    input[i] = 0;
//                }


//                if (see != null)
//                {
//                    input[see.Id] = 1;
//                }

//                for (int i = (InpuNeuron / 3); i <= ((InpuNeuron / 3) * 2 - 1); i++)     // 15
//                {
//                    input[i] = sensor.Sectors[i - (InpuNeuron / 3)].Targets.Where(s => s.tag == "Wall").Any() == true ? 1 : 0;      // 8
//                }

//                for (int i = ((InpuNeuron / 3) * 2); i <= ((InpuNeuron / 3) * 3 - 1); i++)       // 23
//                {
//                    input[i] = sensor.Sectors[i - ((InpuNeuron / 3) * 2)].Targets.Where(s => s.tag == "Bot").Any() == true ? 1 : 0;     // 16
//                }

//                //for (int i = 0; i <= ((InpuNeuron / 3) - 1); i++)     // 7 bei 24
//                //{
//                //    input[i] = sensor.Sectors[i].Targets.Where(s => s.tag == "Food").Any() == true ? 1 : 0;
//                //}

//                //for (int i = (InpuNeuron / 3); i <= ((InpuNeuron / 3) * 2 - 1); i++)     // 15
//                //{
//                //    input[i] = sensor.Sectors[i - (InpuNeuron / 3)].Targets.Where(s => s.tag == "Wall").Any() == true ? 1 : 0;      // 8
//                //}

//                //for (int i = ((InpuNeuron / 3) * 2); i <= ((InpuNeuron / 3) * 3 - 1); i++)       // 23
//                //{
//                //    input[i] = sensor.Sectors[i - ((InpuNeuron / 3) * 2)].Targets.Where(s => s.tag == "Bot").Any() == true ? 1 : 0;     // 16
//                //}

//                var inp = input.Where(i => i == 1);

//                if (!inp.Any())
//                {
//                    randomize = true;
//                }

//                var result = net.Compute(input);

//                goDirection = new Vector3();

//                for (int i = 0; i < result.Length; i++)
//                {
//                    if (result[i] > 0.5)
//                    {
//                        var ttt = Helper.GetSecLine(sensor.Sectors[i].AngleCenter, 0.01f).normalized;
//                        goDirection += ttt;
//                    }
//                }

//                //for (int i = 0; i < sensor.Sectors.Count; i++)
//                //{
//                //    if (result[i] > 0.5)
//                //    {
//                //        var ttt = Helper.GetSecLine(sensor.Sectors[i].AngleCenter, 0.01f).normalized;
//                //        goDirection += ttt;
//                //    }
//                //}

//                if (randomize)
//                {
//                    //Debug.Log("goDirection == Vector3.zero");

//                    var at = Helper.GetAngleToTarget(gameObject, new Vector3(UnityEngine.Random.Range(-90, 90), 0, UnityEngine.Random.Range(-90, 90)));
//                    goDirection = Helper.GetSecLine(at, 0.01f).normalized;
//                    //Debug.Log(goDirection);
//                }

//                goDirection = goDirection.normalized;
//                goDirection *= bot.Speed;
//                goDirection.y = 0;

//                goDirection.x = (float)Math.Round(goDirection.x, 8);
//                goDirection.z = (float)Math.Round(goDirection.z, 8);

//                if (goDirection.x > 1 || goDirection.z > 1)
//                {
//                    Debug.Log("goDirection to big" + goDirection);
//                }
//                else if (goDirection.x < -1 || goDirection.z < -1)
//                {
//                    Debug.Log("goDirection to small" + goDirection);
//                }
//            }
//        }
//    }

//    private bool pufferTime()
//    {
//        elapsed += Time.deltaTime;
//        if (elapsed > RefreshTime)
//        {
//            elapsed = 0;
//            return true;
//        }

//        return false;
//    }
//}
