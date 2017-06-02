using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SensorController_5 : MonoBehaviour
{
    [Header("ScanRange")]
    public float FoodScanRange;
    public float WallScanRange;
    public float BotScanRange;

    [Header("Debug")]
    public bool DrawFoodRange;
    public bool DrawWallRange;
    public bool DrawBotRange;

    [Space(5)]
    public bool DrawAngle = true;
    public bool DrawToSector = true;
    public bool DrawToAllTargets = true;
    public bool DrawToNearstFood = true;
    public bool DrawToNearstBot = true;

    [Header("Sensors")]
    public List<Sensor_5> Sensors;

    private Collider selfCollider;
    private Controller_5 c;

    private void Start()
    {
        c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_5>();
        selfCollider = GetComponent<Collider>();

        for (int i = 0; i < Sensors.Count; i++)
        {
            Sensors[i].Id = i;
            Sensors[i].Go = gameObject;
            Sensors[i].Sc = this;
        }
    }

    private void FixedUpdate()
    {
        sensor_update();
    }

    private void sensor_update()
    {
        Sensors.ForEach(s => s.Targets = new List<GameObject>());
        List<Collider> food_collider = Physics.OverlapSphere(transform.position, FoodScanRange).Where(hc => hc.tag == "Food").ToList();
        List<Collider> bot_collider = Physics.OverlapSphere(transform.position, BotScanRange).Where(hc => hc.tag == "Bot").ToList();
        List<Collider> wall_collider = Physics.OverlapSphere(transform.position, WallScanRange).Where(hc => hc.tag == "Wall").ToList();
        bot_collider.Remove(selfCollider);

        sensor_setTargets(food_collider);
        sensor_setTargets(bot_collider);
        sensor_setTargets(wall_collider);
    }

    private void sensor_setTargets(List<Collider> hitColliders)
    {
        foreach (var item in hitColliders)
        {
            float angleToTarget = Helper_5.GetAngleFromVector(gameObject, item.transform.position);
            var sector = Sensors.FirstOrDefault(s => s.Match(angleToTarget));
            if (sector != null)
            {
                sector.Targets.Add(item.transform.gameObject);
            }
        }
    }

    public List<Targets> GetTargetSensors(string tag)
    {
        var sensors = Sensors.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == tag && s.NearstTarget!=gameObject)).OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position)).ToList();
        //var wallSensors = Sensors.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == "Wall")).OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position)).ToList();
        //var botSensors = Sensors.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == "Bot")).OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position)).ToList();
        //var bot = Sensors.Where(s => s.Targets.Any(t => t.tag == "Bot")).OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position)).ToList();

        List<Targets> targets = new List<Targets>();

        foreach (var s in sensors)
        {
            targets.Add(new Targets(tag, s));
        }

        return targets;
    }

    private void OnDrawGizmos()
    {
        if (c != null && (c.State == Controller_5.ControllerState.Run))
        {
            if (DrawFoodRange)
            {
                DebugExtension.DrawCircle(transform.position, Color.green, FoodScanRange);
            }

            if (DrawWallRange)
            {
                DebugExtension.DrawCircle(transform.position, Color.white, WallScanRange);
            }

            if (DrawBotRange)
            {
                DebugExtension.DrawCircle(transform.position, Color.yellow, BotScanRange);
            }

            foreach (var sen in Sensors)
            {
                if (DrawToSector)
                {
                    sen.OnDrawToSector();
                }

                if (DrawToAllTargets)
                {
                    sen.OnDrawToAllTargets();
                }

                if (DrawAngle)
                {
                    sen.OnDrawAngle();
                }
            }

            if (DrawToNearstFood)
            {
                var nearstFoodSensor = Sensors.Where(w => w.NearstTarget != null && (w.NearstTarget.tag == "Food"));
                if (nearstFoodSensor.Any())
                {
                    nearstFoodSensor = nearstFoodSensor.OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position));
                    nearstFoodSensor.First().OnDrawToNearstTarget();
                }
            }

            if (DrawToNearstBot)
            {
                var nearstBotSensor = Sensors.Where(w => w.NearstTarget != null && (w.NearstTarget.tag == "Bot"));
                if (nearstBotSensor.Any())
                {
                    nearstBotSensor = nearstBotSensor.OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position));
                    nearstBotSensor.First().OnDrawToNearstTarget();
                }
            }
        }
    }
}
