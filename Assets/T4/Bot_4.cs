//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class Bot_4 : MonoBehaviour
//{
//    public enum BotState
//    {
//        Run,
//        Stop,
//        DontMove,
//        DontRotate,
//        Standby
//    }

//    public string Dna;
//    public bool debug = true;

//    [Header("Sim")]
//    public BotState State = BotState.Run;
//    public float Health = 100000;


//    [Space(5)]
//    public float rotateSpeed = 2;
//    public float Speed = 10;

//    [Header("Network")]
//    public bool draw_Neuro = true;
//    public int InpuNeuron = 15;
//    public int HiddenNeuron1 = 12;
//    public int HiddenNeuron2 = 8;
//    public int OutputNeuron = 5;
//    [Space(5)]

//    [Header("Sensor")]
//    public bool draw_FoodScanRange = true;
//    public float foodScanRange = 25;
//    public bool draw_WallScanRange = true;
//    public float wallScanRange = 16;
//    public bool draw_BotScanRange = true;
//    public float botScanRange = 15;

//    [Space(5)]
//    public bool draw_Sensor = true;
//    public int sensorsCount = 5;
//    public float Angle = 180;
//    //public float ToAngle = 90;

//    [Space(5)]
//    public bool draw_ToAllTarget = true;
//    public bool draw_ToNearstTarget = true;
//    //public bool draw_ToTarget = true;
//    public bool draw_ToSensor = true;

//    [SerializeField]
//    public List<Sensor_4> sensors;

//    private Collider selfCollider;
//    internal NeuralNet net;
//    //private Rigidbody rb;
//    private BotController_4 bc;
//    internal bool init=false;

//    void Start()
//    {
//        bc = GameObject.FindGameObjectWithTag("GameController").GetComponent<BotController_4>();
//        selfCollider = GetComponent<Collider>();
//        //rb = GetComponent<Rigidbody>();

//        //net = new NeuralNet(InpuNeuron, HiddenNeuron, OutputNeuron);


//    }


//    internal void Init(NeuralNet nn)//,bool first = true )
//    {
//        //if (!String.IsNullOrEmpty(Dna))
//        //{
//        //    Debug.Log("Buld Network from CSB");
//        //    buildDnaFromCSV(Dna);
//        //    Dna = String.Empty;
//        //}
//        //else
//        //{
//        //    net = nn;
//        //}

//        net = nn;
//        initSensor();
//        init = true;
//    }

//    void Update()
//    {
//        if (!init)
//        {
//            Init(new NeuralNet(InpuNeuron, HiddenNeuron1, HiddenNeuron1, OutputNeuron));
//        }

//        if (bc.State == BotController_4.ControllerState.Stop || State == BotState.Stop)
//        {
//            return;
//        }

//        //if (State == BotState.Run)
//        //{
//        //    sensor_update();
//        //    body_update();
//        //    network_update();
//        //}
//        //else if (State == BotState.DontMove)
//        //{
//        //    sensor_update();
//        //    //body_update();
//        //    network_update();
//        //}


//        //Debug.DrawLine(transform.position, from + transform.position, Color.blue);


//        sensor_update();
//        body_update();


//        //puffern!!!
//        network_update();
//    }

//    private void body_update()
//    {
//        List<Collider> hitColliders = Physics.OverlapSphere(transform.position, 2).ToList();
//        hitColliders.Remove(selfCollider);
//        var foodsInRange = hitColliders.FirstOrDefault(hc => hc.tag == "Food");
//        if (foodsInRange != null)
//        {
//            Health += bc.foodGod;
//            var x = UnityEngine.Random.Range(bc.FoodSpawnRange.x, bc.FoodSpawnRange.y);
//            var z = UnityEngine.Random.Range(bc.FoodSpawnRange.x, bc.FoodSpawnRange.y);
//            foodsInRange.gameObject.transform.position = new Vector3(x, 0.5f, z);
//        }

//        var botsInRange = hitColliders.FirstOrDefault(hc => hc.tag == "Bot");
//        if (botsInRange != null)
//        {
//            if ((Health - bc.botPain) > 0)
//            {
//                Health -= bc.botPain;
//            }
//        }

//        var wallsInRange = hitColliders.FirstOrDefault(hc => hc.tag == "Wall");
//        if (wallsInRange != null)
//        {
//            if ((Health - bc.wallPain) > 0)
//            {
//                Health -= bc.wallPain;
//            }
//        }
//    }

//    void OnDrawGizmos()
//    {
//        if (Application.isPlaying)
//        {
//            if (debug)
//            {
//                drawSensor();
//            }
//        }
//    }

//    #region Network
//    private void network_update()
//    {
//        double[] input = new double[InpuNeuron];
//        var food = sensors.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == "Food")).ToList();
//        var wall = sensors.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == "Wall")).ToList();
//        var bot = sensors.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == "Bot")).ToList();

//        food = food.OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position)).ToList();

//        //Debug.Log("first : " + Vector3.Distance(food.First().NearstTarget.transform.position, transform.position));
//        //Debug.Log("last  : " + Vector3.Distance(food.Last().NearstTarget.transform.position, transform.position));

//        if (food.Any())
//        {
//            input[food.First().Id] = 1;
//        }

//        //foreach (var sen in food)
//        //{
//        //    input[sen.Id] = 1;
//        //}

//        foreach (var sen in wall)
//        {
//            input[sen.Id + sensorsCount] = 1;
//        }

//        foreach (var sen in bot)
//        {
//            input[sen.Id + sensorsCount*2] = 1;
//        }

//        var result = net.Compute(input);

//        Color col = Color.yellow;

//        if (result[0] > 0.5)
//        {
//            var d = new Vector3(0, -1, 0) * rotateSpeed * Time.deltaTime;
//            if (State != BotState.DontRotate && State != BotState.Standby)
//            {
//                transform.Rotate(d);
//            }

//            if (draw_Neuro)
//            {
//                Debug.Log("hard left");
//                Vector3 center = getSecLine(sensors[0].AngleCenter, 1);
//                Debug.DrawLine(transform.position, center + transform.position, col);
//            }
//        }

//        if (result[1] > 0.5 && State != BotState.DontRotate && State != BotState.Standby)
//        {
//            var d = new Vector3(0, -0.5f, 0) * rotateSpeed * Time.deltaTime;
//            if (State != BotState.DontRotate && State != BotState.Standby)
//            {
//                transform.Rotate(d);
//            }

//            if (draw_Neuro)
//            {
//                Debug.Log("soft left");
//                Vector3 center = getSecLine(sensors[1].AngleCenter, 1);
//                Debug.DrawLine(transform.position, center + transform.position, col);
//            }
//        }

//        if (result[2] > 0.5 && State != BotState.DontMove && State != BotState.Standby)
//        {
//            transform.position += transform.forward * Time.deltaTime * Speed;
//            if ((Health - bc.permaPain) > 0)
//            {
//                Health -= bc.permaPain;
//            }
//        }

//        if (result[3] > 0.5 && State != BotState.DontRotate && State != BotState.Standby)
//        {
//            var d = new Vector3(0, 0.5f, 0) * rotateSpeed * Time.deltaTime;
//            if (State != BotState.DontRotate && State != BotState.Standby)
//            {
//                transform.Rotate(d);
//            }

//            if (draw_Neuro)
//            {
//                Debug.Log("soft right");
//                Vector3 center = getSecLine(sensors[3].AngleCenter, 1);
//                Debug.DrawLine(transform.position, center + transform.position, col);
//            }
//        }

//        if (result[4] > 0.5 && State != BotState.DontRotate && State != BotState.Standby)
//        {
//            var d = new Vector3(0, 1, 0) * rotateSpeed * Time.deltaTime;
//            if (State != BotState.DontRotate && State != BotState.Standby)
//            {
//                transform.Rotate(d);
//            }

//            if (draw_Neuro)
//            {
//                Debug.Log("hard right");
//                Vector3 center = getSecLine(sensors[4].AngleCenter, 1);
//                Debug.DrawLine(transform.position, center + transform.position, col);
//            }
//        }
//    }
//    #endregion

//    #region Sensor

//    private void initSensor()
//    {
//        sensors = new List<Sensor_4>();
//        var steps = Angle / sensorsCount;
//        //float angle = FromAngle;

//        var angle = 360 - Angle / 2;
//        //var last = Angle / 2;

//        for (int i = 0; i < sensorsCount; i++)
//        {
//            float tmp;
//            if (angle + steps > 360)
//            {
//                tmp = (angle + steps) - 360;
//            }
//            else
//            {
//                tmp = angle + steps;
//            }

//            sensors.Add(new Sensor_4(i, gameObject, angle, tmp));
//            angle += steps;
//            if (angle > 360)
//            {
//                tmp = angle - 360;
//                angle = tmp;
//            }
//        }
//    }

//    private void sensor_update()
//    {
//        sensors.ForEach(s => s.Targets = new List<GameObject>());
//        List<Collider> food_collider = Physics.OverlapSphere(transform.position, foodScanRange).Where(hc => hc.tag == "Food").ToList();
//        List<Collider> bot_collider = Physics.OverlapSphere(transform.position, botScanRange).Where(hc => hc.tag == "Bot").ToList();
//        List<Collider> wall_collider = Physics.OverlapSphere(transform.position, wallScanRange).Where(hc => hc.tag == "Wall").ToList();
//        bot_collider.Remove(selfCollider);

//        //Debug.Log(food_collider.Count);

//        sensor_setTargets(food_collider);
//        sensor_setTargets(bot_collider);
//        sensor_setTargets(wall_collider);
//    }

//    private void sensor_setTargets(List<Collider> hitColliders)
//    {
//        foreach (var item in hitColliders)
//        {
//            float angleToTarget = getAngleFromVector(item.transform.position);
//            //Debug.Log(angleToTarget);
//            //var sector = sensors.FirstOrDefault(s => s.AngleFrom < angleToTarget && s.AngleTo > angleToTarget);
//            var sector = sensors.FirstOrDefault(s => s.Match(angleToTarget));
//            if (sector != null)
//            {
//                sector.Targets.Add(item.transform.gameObject);
//            }
//        }
//    }

//    private void drawSensor()
//    {
//        //Debug.Log("FF: "+ transform.forward);
//        //getSecLine();
//        if (draw_FoodScanRange)
//        {
//            DebugExtension.DrawCircle(transform.position, Color.green, foodScanRange);
//        }

//        if (draw_WallScanRange)
//        {
//            DebugExtension.DrawCircle(transform.position, Color.white, wallScanRange);
//        }

//        if (draw_BotScanRange)
//        {
//            DebugExtension.DrawCircle(transform.position, Color.yellow, botScanRange);
//        }



//        Color col = Color.green;



//        foreach (var sen in sensors)
//        {
//            var item = sen.NearstTarget;
//            if (item != null)
//            {
//                if (item.tag == "Bot")
//                {
//                    col = Color.red;
//                }
//                if (item.tag == "Wall")
//                {
//                    col = Color.white;
//                }
//                if (item.tag == "Food")
//                {
//                    col = Color.green;
//                }
//            }

//            if (draw_Sensor)
//            {
//                Vector3 from = getSecLine(sen.AngleFrom, 5);
//                Vector3 to = getSecLine(sen.AngleTo, 5);

//                Debug.DrawLine(transform.position, from + transform.position, Color.blue);
//                Debug.DrawLine(transform.position, to + transform.position, Color.blue);
//            }

//            if (draw_ToAllTarget)
//            {

//                if (item != null)
//                {
//                    Vector3 toTarget = getSecLine(getAngleFromVector(item.transform.position), Vector3.Distance(item.transform.position, transform.position));
//                    toTarget.y = 1;
//                    Debug.DrawLine(transform.position, toTarget + transform.position, col);
//                }
//            }

//            if (draw_ToSensor && sen.Targets.Any())
//            {
//                //Debug.Log("s_Id: "+sen.Id+" : "+sen.AngleCenter);
//                Vector3 center = getSecLine(sen.AngleCenter, foodScanRange / 4);
//                center.y = 1;
//                Debug.DrawLine(transform.position, center + transform.position, col);
//            }
//        }

//        if (draw_ToNearstTarget)
//        {
//            var nearst = sensors.Where(w => w.Targets.Any(a => a.tag == "Food")).OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position)).ToList();

//            if (nearst.Any())
//            {
//                Vector3 toNearst = getSecLine(getAngleFromVector(nearst.First().NearstTarget.transform.position), Vector3.Distance(nearst.First().NearstTarget.transform.position, transform.position));
//                toNearst.y = 1;
//                Debug.DrawLine(transform.position, toNearst + transform.position, Color.blue);
//            }
//        }


//        DebugExtension.DrawArrow(transform.position, transform.forward * 2);
//    }

//    private Vector3 getSecLine(float angleFrom, float scanRange)
//    {
//        var d = transform.TransformDirection(Quaternion.AngleAxis(angleFrom, Vector3.up) * Vector3.forward) * scanRange;
//        d.y = 1;
//        return d;
//    }

//    private float getAngleFromVector(Vector3 target)
//    {
//        var localTarget = transform.InverseTransformPoint(target);
//        var angleToTarget = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
//        if (angleToTarget < 0)
//        {
//            angleToTarget += 360;
//        }
//        return angleToTarget;
//    }


//    #endregion
//}
