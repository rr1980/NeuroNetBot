using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Bot_12 : MonoBehaviour
{
    public List<Sensor_12> Sensors;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        DebugExtension.DrawArrow(transform.position, transform.forward * 2);
    }
}
