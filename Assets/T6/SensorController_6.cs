using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[ExecuteInEditMode]
public class SensorController_6 : MonoBehaviour
{
    [Header("Debug")]
    public bool DrawScanRange = true;
    public bool DrawAngle = true;

    [Space(5)]
    public bool DrawToNearstTarget = true;
    public bool DrawToSector = true;

    [Header("ScanRange")]
    public float ScanRange;

    [Header("Sensors")]
    public List<Sensor_6> Sensors;

    private Collider selfCollider;
    private Controller_6 c;
    private int foundindex = 0;
    private Dictionary<string, int> founds = new Dictionary<string, int>();

    private void Start()
    {
        c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_6>();
        selfCollider = GetComponent<Collider>();

        for (int i = 0; i < Sensors.Count; i++)
        {
            Sensors[i].Id = i;
            Sensors[i].Go = gameObject;
            Sensors[i].Sc = this;
        }
    }

    private void Update()
    {
        sensor_update();
    }

    private void sensor_update()
    {
        Sensors.ForEach(s => s.Targets = new List<GameObject>());
        List<Collider> targets = Physics.OverlapSphere(transform.position, ScanRange).Where(t => t.tag != "Floor").ToList();

        //List<Collider> bot_collider = Physics.OverlapSphere(transform.position, BotScanRange).Where(hc => hc.tag == "Bot").ToList();
        //List<Collider> wall_collider = Physics.OverlapSphere(transform.position, WallScanRange).Where(hc => hc.tag == "Wall").ToList();
        targets.Remove(selfCollider);

        //sensor_setTargets(food_collider);
        //sensor_setTargets(bot_collider);
        sensor_setTargets(targets);
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

    //public List<Targets> GetTargetSensors(string tag)
    //{
    //    var sensors = Sensors.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == tag && s.NearstTarget != gameObject)).OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position)).ToList();
    //    //var wallSensors = Sensors.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == "Wall")).OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position)).ToList();
    //    //var botSensors = Sensors.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == "Bot")).OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position)).ToList();
    //    //var bot = Sensors.Where(s => s.Targets.Any(t => t.tag == "Bot")).OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position)).ToList();

    //    List<Targets> targets = new List<Targets>();

    //    foreach (var s in sensors)
    //    {
    //        targets.Add(new Targets(tag, s));
    //    }

    //    return targets;
    //}

    private void OnDrawGizmos()
    {
        if (c != null && (c.State == Controller_6.ControllerState.Run))
        {
            if (DrawScanRange)
            {
                DebugExtension.DrawCircle(transform.position, Color.green, ScanRange);
            }

            foreach (var sen in Sensors)
            {
                if (DrawToSector)
                {
                    sen.OnDrawToSector();
                }

                if (DrawAngle)
                {
                    sen.OnDrawAngle();
                }
            }

            if (DrawToNearstTarget)
            {
                var nearstFoodSensor = getNearstTargetSensor();
                if (nearstFoodSensor != null)
                {
                    nearstFoodSensor.OnDrawToNearstTarget();
                }
            }
        }
    }

    public Target_6 GetNearstTargetSensors()
    {
        var nt = getNearstTargetSensor();
        if (nt != null)
        {
            int fi;

            if (founds.TryGetValue(nt.NearstTarget.tag, out fi))
            {
                return new Target_6(nt.NearstTarget.tag, nt, fi);
            }
            else
            {
                var newFoundTag = nt.NearstTarget.tag;
                founds.Add(newFoundTag, foundindex);
                var newTarget = new Target_6(nt.NearstTarget.tag, nt, foundindex);
                foundindex++;
                return newTarget;
            }
        }
        return null;
    }

    private Sensor_6 getNearstTargetSensor()
    {
        var nearstFoodSensor = Sensors.Where(w => w.NearstTarget != null);
        if (nearstFoodSensor.Any())
        {
            nearstFoodSensor = nearstFoodSensor.OrderBy(f => Vector3.Distance(f.NearstTarget.transform.position, transform.position));
            return nearstFoodSensor.First();
        }

        return null;
    }

    private GameObject getNearstTarget()
    {
        return getNearstTargetSensor().NearstTarget;
    }
}

[System.Serializable]
public class Target_6
{
    public string Tag;
    public string Name;
    public int SensorId;
    public int FoundIndex;

    public Target_6(string tag, Sensor_6 sensor, int fi)
    {
        //Debug.Log(fi);
        Tag = tag;
        Name = sensor.Name;
        SensorId = sensor.Id;
        FoundIndex = fi;
    }
}