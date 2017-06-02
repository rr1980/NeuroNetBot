using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SensorBank_13 : MonoBehaviour
{
    public GameObject SensorPrefab;
    [Space(5)]
    public float ScanRange;
    public string ObstTag;

    public int Count
    {
        get
        {
            return Sensors.Count;
        }
    }


    public List<Sensor_13> Sensors;

    private void Start()
    {
        Sensors = GetComponentsInChildren<Sensor_13>().ToList();
    }
}
