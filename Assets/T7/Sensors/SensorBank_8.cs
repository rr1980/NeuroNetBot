using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SensorBank_8 : MonoBehaviour
{
    //public List<GameObject> Sensors;
    public float RefreshTime = 0.5f;
    public float ScanRange;
    [Header("Debug")]
    public bool DrawScanRange = true;
    public bool DrawAngle = true;

    [Space(5)]
    public bool DrawToNearstTarget = true;
    public bool DrawToSector = true;
    public bool DrawToStT = true;

    [SerializeField]
    public List<Target> Targets
    {
        get
        {
            List<Target> tt = new List<Target>();
            var sen = GetComponentsInChildren<Sensor_8>().Where(s => s.Targets.Any());
            foreach (var item in sen)
            {
                tt.AddRange(item.Targets);
            }
            return tt;
        }
    }

    [SerializeField]
    public List<Target> NearstTargets
    {
        get
        {
            List<Target> tt = new List<Target>();
            var sen = GetComponentsInChildren<Sensor_8>().Where(s => s.NearstTarget != null);
            foreach (var item in sen)
            {
                tt.Add(item.NearstTarget);
            }
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
