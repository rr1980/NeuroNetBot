using UnityEngine;

[ExecuteInEditMode]
public class Sensor_13 : MonoBehaviour
{
    public float Distance;

    private SensorBank_13 SensorBank;

    private void Start()
    {
        SensorBank = transform.parent.gameObject.GetComponent<SensorBank_13>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, SensorBank.ScanRange))
        {
            if (hit.transform.root.gameObject.tag == SensorBank.ObstTag)
            {
                Distance = Vector3.Distance(hit.transform.position, transform.position);
            }
            else
            {
                Distance = -1;
            }
        }
        else
        {
            Distance = -1;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.forward * SensorBank.ScanRange, Color.green);
    }
}
