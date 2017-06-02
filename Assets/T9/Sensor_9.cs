using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[ExecuteInEditMode]
public class Sensor_Base : MonoBehaviour
{
    public bool DrawDebug;
    public float RefreshTime = 0;
    public float Angle;
    [Space(5)]
    public List<Target_9> Targets = new List<Target_9>();

    protected float elapsed;
    internal SensorBank_Base SensorBank;

    public virtual Target_9 NearstTarget {get;set;}

    protected bool pufferTime()
    {
        if (!Application.isPlaying)
        {
            return true;
        }

        float rf = RefreshTime;

        if (rf == 0)
        {
            rf = SensorBank.RefreshTime;
        }

        elapsed += Time.deltaTime;
        if (elapsed > rf)
        {
            elapsed = 0;
            return true;
        }

        return false;
    }
}

//[ExecuteInEditMode]
public class Sensor_9 : Sensor_Base
{
    private List<RaycastHit> hits = new List<RaycastHit>();

    void Start()
    {
        SensorBank = transform.root.GetComponentInChildren<SensorBank_9>();
    }

    void Update()
    {
        if (pufferTime())
        {
            Targets = new List<Target_9>();
            hits = new List<RaycastHit>();

            for (int i = 0; i < Angle; i++)
            {
                Vector3 angle = Helper_5.GetSecLine(gameObject, i, SensorBank.ScanRange);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, angle, out hit, SensorBank.ScanRange))
                {
                    if (hit.transform.root.gameObject != transform.root.gameObject)
                    {
                        hits.Add(hit);
                        if (Targets.FirstOrDefault(t => t.goTarget == hit.transform.gameObject) == null)
                        {
                            Targets.Add(new Target_9(this, gameObject, hit.transform.gameObject));
                        }
                    }
                }
            }
        }
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

    private void OnDrawGizmos()
    {
        if (DrawDebug)
        {
            if (SensorBank.DrawAngle)
            {
                OnDrawAngle();
            }

            if (SensorBank.DrawToSector)
            {
                OnDrawToSector();
            }

            if (SensorBank.DrawToNearstTarget)
            {
                OnDrawToNearstTarget();
            }

            if (SensorBank.DrawRay)
            {
                OnDrawRays();
            }
        }
    }

    public void OnDrawToSector()
    {
        if (Targets.Any())
        {
            Color col = Color.yellow;
            Vector3 from = Helper_5.GetSecLine(gameObject, 1, SensorBank.ScanRange);
            Vector3 to = Helper_5.GetSecLine(gameObject, Angle - 1, SensorBank.ScanRange);
            from.y = 0;
            to.y = 0;
            Debug.DrawLine(transform.position, from + transform.position, col);
            Debug.DrawLine(transform.position, to + transform.position, col);
            Debug.DrawLine(from + transform.position, to + transform.position, col);
        }
    }

    public void OnDrawToNearstTarget()
    {
        if (NearstTarget != null && (NearstTarget.goTarget!=null))
        {
            Color col = Color.green;
            Vector3 toTarget = Helper_5.GetSecLine(gameObject, Helper_5.GetAngleFromVector(gameObject, NearstTarget.Position), NearstTarget.Distance);
            toTarget.y = 0;
            if (NearstTarget.goTarget.tag == "Bot")
            {
                col = Color.red;
            }
            Debug.DrawLine(transform.position, toTarget + transform.position, col);
        }
    }


    private void OnDrawRays()
    {
        foreach (var item in hits)
        {
            Vector3 angle2 = Helper_5.GetSecLine(gameObject, Helper_5.GetAngleFromVector(gameObject, item.point), item.distance);
            angle2.y = 0;
            Debug.DrawLine(transform.position, angle2 + transform.position, Color.red);
        }
    }

    public void OnDrawAngle()
    {
        Vector3 from = Helper_5.GetSecLine(gameObject, 0, SensorBank.ScanRange);
        Vector3 to = Helper_5.GetSecLine(gameObject, Angle, SensorBank.ScanRange);
        from.y = 0;
        to.y = 0;
        Debug.DrawLine(transform.position, from + transform.position, Color.white);
        Debug.DrawLine(transform.position, to + transform.position, Color.white);
        //Debug.DrawLine(from + transform.position, to + transform.position, Color.white);
    }

}
