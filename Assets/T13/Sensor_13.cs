using UnityEngine;

[ExecuteInEditMode]
public class Sensor_13 : MonoBehaviour
{
    public float Distance;
    public LayerMask layerMask;
    

    private SensorBank_13 SensorBank;
    private Color col;

    private void Start()
    {
        SensorBank = transform.parent.gameObject.GetComponent<SensorBank_13>();
        //layerMask = LayerMask.GetMask("Food");
        //Debug.Log(layerMask.value);
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out  hit, SensorBank.ScanRange, layerMask))
        //if (Physics.Raycast(transform.position, transform.forward, out hit, SensorBank.ScanRange))
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

    private void OnDrawGizmos()
    {
        if (Distance != 0)
        {
            col = Color.red;
            Debug.DrawLine(transform.position, transform.position + transform.forward * SensorBank.ScanRange, col);
        }
        else
        {
            col = Color.green;
        }


    }
}
