using UnityEngine;

[System.Serializable]
public class Input_9
{
    public string Type = "I";
    public GameObject Sensor;
    private Sensor_Base sensor;
    //private Output_9 output;

    public string Tag
    {
        get
        {
            if (sensor == null)
            {
                sensor = Sensor.GetComponent<Sensor_Base>();
                if (sensor == null)
                {
                    //output = Sensor.GetComponent<Output_9>();
                    //return output.Tag;
                }
            }

            //if (sensor.NearstTarget != null && (sensor.NearstTarget.goTarget != null))
            //{
            //    return sensor.NearstTarget.Tag;
            //}
            //else
            //{
                return null;
            //}

        }
    }

    public float Value
    {
        get
        {
            if (sensor == null)
            {
                sensor = Sensor.GetComponent<Sensor_9>();
                if (sensor == null)
                {
                    //output = Sensor.GetComponent<Output_9>();
                    //return output.Value;
                }
            }

            //if (sensor.NearstTarget != null && (sensor.NearstTarget.goTarget != null))
            //{
            //    return sensor.NearstTarget.DistanceValue;
            //}
            //else
            //{
                return -Sensor.transform.root.GetComponentInChildren<SensorBank_9>().ScanRange;
            //}
        }
    }
}



//[ExecuteInEditMode]
//public class Input_9 : MonoBehaviour
//{
//    public GameObject Sensor;
//    public float Value;
//    public string Tag;

//    private Sensor_9 sensor;

//    void Start()
//    {
//        sensor = Sensor.GetComponent<Sensor_9>();
//    }

//    void Update()
//    {
//        if (sensor.NearstTarget != null && (sensor.NearstTarget.goTarget!=null))
//        {
//            if (sensor.NearstTarget.goTarget.tag == Tag)
//            {
//                Value = sensor.NearstTarget.DistanceValue;
//            }
//        }
//        else
//        {
//            Value = -transform.root.GetComponentInChildren<SensorBank_9>().ScanRange;
//        }
//    }
//}