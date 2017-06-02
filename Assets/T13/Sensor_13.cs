using UnityEngine;

[ExecuteInEditMode]
public class Sensor_13 : MonoBehaviour
{
    public float Distance;

    private SensorBank_13 SensorBank;
    private Color col;

    private void Start()
    {
        SensorBank = transform.parent.gameObject.GetComponent<SensorBank_13>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, SensorBank.ScanRange))
        {
            if (hit.transform.gameObject.tag == SensorBank.ObstTag)
            {
                Distance = (SensorBank.ScanRange - Vector3.Distance(hit.transform.position, transform.position)) *1000;
                //Debug.Log(Distance);
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
        if (Distance != -1)
        {
            col = Color.red;
        }
        else
        {
            col = Color.green;
        }

        Debug.DrawLine(transform.position, transform.position + transform.forward * SensorBank.ScanRange, col);
    }
}
