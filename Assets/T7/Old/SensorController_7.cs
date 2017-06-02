using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SensorController_7 : MonoBehaviour
{
    public float ScanRange;
    public List<GameObject> Targets;

    [Header("Debug")]
    public bool DrawScanRange = true;
    public bool DrawAngle = true;

    [Space(5)]
    public bool DrawToNearstTarget = true;
    public bool DrawToSector = true;

    private List<SensorItem_7> Sensors;
    //private GameObject Body;
    //private GameObject rootGO;
    //private BotController_7 bc;
    //private Collider selfCollider;

    void Start()
    {
        //rootGO = gameObject.transform.root.gameObject;
        //bc = GetComponentInParent<BotController_7>();
        Sensors = GetComponentsInChildren<SensorItem_7>().ToList();
        //Body = rootGO.transform.Find("Body").gameObject;
        //selfCollider = Body.GetComponent<Collider>();
        name = "Sensors";
    }

    private void Update()
    {
        Sensors.ForEach(s => s.Targets = new List<GameObject>());
        Targets = Physics.OverlapSphere(transform.position, ScanRange).Where(t => t.tag != "Floor" && t.transform.root != transform.root).Select(s=>s.gameObject).ToList();

        foreach (var item in Targets)
        {
            float angleToTarget = Helper_5.GetAngleFromVector(gameObject, item.transform.position);
            var sector = Sensors.FirstOrDefault(s => s.Match(angleToTarget));
            if (sector != null)
            {
                sector.Targets.Add(item.transform.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (DrawScanRange)
        {
            DebugExtension.DrawCircle(transform.position, Color.green, ScanRange);
        }
    }
}
