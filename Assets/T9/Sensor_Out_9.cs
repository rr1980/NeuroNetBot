using System.Linq;
using UnityEngine;
[ExecuteInEditMode]
public class Sensor_Out_9 : Sensor_Base
{
    //private List<RaycastHit> hits = new List<RaycastHit>();

    void Start()
    {
        SensorBank = transform.root.GetComponentInChildren<SensorBank_9>();
    }

    void Update()
    {
        
    }

    [SerializeField]
    public override Target_9 NearstTarget
    {
        get
        {
            if (Targets.Any())
            {
                return Targets.OrderBy(t => t.Distance).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
    

}
