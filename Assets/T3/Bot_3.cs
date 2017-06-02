//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class Bot_3 : MonoBehaviour
//{

//    [Header("Fitness")]
//    public float Health = 10000;

//    [Header("Settings")]
//    public float RefreshTime = 0.2f;
//    public bool canMove = true;
//    public float Speed = 2;
//    public float foodGod = 10000;
//    public float wallPain = 100;
//    public float botPain = 10;
//    public float permaPain = 1;

//    private Network_3 net;
//    private Sensor_3 sensor;
//    private BotController_2 controller;
//    private Collider selfCollider;
//    private float elapsed;

//    void Start()
//    {
//        selfCollider = GetComponent<Collider>();
//        controller = FindObjectOfType<BotController_2>();
//        net = GetComponent<Network_3>();
//        sensor = GetComponent<Sensor_3>();
//    }

//    void FixedUpdate()
//    {
//        if (controller.State == BotController_2.StateEnum.Run)
//        {
//            //if (pufferTime())
//            //{
//            List<Collider> hitColliders = Physics.OverlapSphere(transform.position, 2).ToList();
//            hitColliders.Remove(selfCollider);
//            var foodsInRange = hitColliders.FirstOrDefault(hc => hc.tag == "Food");
//            if (foodsInRange != null)
//            {
//                Health += foodGod;
//                var x = UnityEngine.Random.Range(-80, 80);
//                var z = UnityEngine.Random.Range(-80, 80);
//                foodsInRange.gameObject.transform.position = new Vector3(x, 0.5f, z);
//            }

//            var botsInRange = hitColliders.FirstOrDefault(hc => hc.tag == "Bot");
//            if (botsInRange != null)
//            {
//                if ((Health - botPain) > 0)
//                {
//                    Health -= botPain;
//                }
//            }

//            var wallsInRange = hitColliders.FirstOrDefault(hc => hc.tag == "Wall");
//            if (wallsInRange != null)
//            {
//                if ((Health - wallPain) > 0)
//                {
//                    Health -= wallPain;
//                }
//            }

//            if ((Health - permaPain) > 0)
//            {
//                Health -= permaPain;
//            }

//            if (canMove)
//            {
//                transform.position += net.goDirection;
//            }
//        }
//    }

//    private void OnDrawGizmos()
//    {
//        DebugExtension.DrawCircle(transform.position, Color.red, 2);
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
