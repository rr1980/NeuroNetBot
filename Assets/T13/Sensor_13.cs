using UnityEngine;

[ExecuteInEditMode]
public class Sensor_13 : MonoBehaviour
{
    public float Distance;

    private SensorBank_13 SensorBank;
    private Color col;
    private LayerMask layerMask;

    private void Start()
    {
        SensorBank = transform.parent.gameObject.GetComponent<SensorBank_13>();
        layerMask = LayerMask.GetMask("Wall");
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, SensorBank.ScanRange, layerMask))
        //if (Physics.Raycast(transform.position, transform.forward, out hit, SensorBank.ScanRange))
        {
            if (hit.transform.gameObject.tag == SensorBank.ObstTag)
            {
                Distance = SensorBank.ScanRange - Vector3.Distance(hit.transform.position, transform.position);
                //Distance = (Vector3.Distance(hit.transform.position, transform.position)/10)* (Vector3.Distance(hit.transform.position, transform.position) / 10);
                //Debug.Log(Distance);
            }
            else
            {
                Distance = 0;
            }
        }
        else
        {
            Distance = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (Distance != 0)
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
