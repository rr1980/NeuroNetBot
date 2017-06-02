using PropertyEnabledInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SensorBank_Base_11 : MonoBehaviour
{
    //public List<GameObject> Sensors;
    public float RefreshTime = 0.5f;
    public float ScanRange;
    [Header("Debug")]
    public bool DrawScanRange = true;
    public bool DrawAngle = true;

    [Space(5)]
    public bool DrawToNearstFoodTarget = true;
    public bool DrawToSector = true;
    public bool DrawRay = true;
}

[ExecuteInEditMode]
public class SensorBank_11 : SensorBank_Base_11
{
    public List<Target_11> Targets
    {
        get
        {
            List<Target_11> tt = new List<Target_11>();
            //var sen = GetComponentsInChildren<Sensor_11>().Where(s => s.Targets.Any());
            //foreach (var item in sen)
            //{
            //    tt.AddRange(item.Targets);
            //}
            return tt;
        }
    }

    [SerializeField]
    public List<Target_11> NearstTargets
    {
        get
        {
        List<Target_11> tt = new List<Target_11>();
        //var sen = GetComponentsInChildren<Sensor_11>().Where(s => s.NearstTarget != null);
        //foreach (var item in sen)
        //{
        //    tt.Add(item.NearstTarget);
        //}
        return tt;
        }
    }

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnDrawGizmos()
    {
        if (DrawScanRange)
        {
            DebugExtension.DrawCircle(transform.position, Color.green, ScanRange);
        }
    }
}
