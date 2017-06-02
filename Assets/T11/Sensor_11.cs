using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Sensor_Base_11 : MonoBehaviour
{
    public bool DrawDebug;
    public float RefreshTime = 0;
    public float Angle;
    [Space(5)]
    public List<Target_11> Food_Targets = new List<Target_11>();
    public List<Target_11> Bot_Targets = new List<Target_11>();
    public List<Target_11> Wall_Targets = new List<Target_11>();

    protected float elapsed;
    internal SensorBank_Base_11 SensorBank;


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

[ExecuteInEditMode]
public class Sensor_11 : Sensor_Base_11
{
    private List<RaycastHit> hits = new List<RaycastHit>();

    void Start()
    {
        SensorBank = transform.root.GetComponentInChildren<SensorBank_11>();
    }

    void Update()
    {
        if (pufferTime())
        {
            Food_Targets = new List<Target_11>();
            Bot_Targets = new List<Target_11>();
            Wall_Targets = new List<Target_11>();
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
                        if (hit.transform.gameObject.tag == "Food")
                        {
                            if (Food_Targets.FirstOrDefault(t => t.goTarget == hit.transform.gameObject) == null)
                            {
                                Food_Targets.Add(new Target_11(this, gameObject, hit.transform.gameObject));
                            }
                        }

                        if (hit.transform.gameObject.tag == "Bot")
                        {
                            if (Bot_Targets.FirstOrDefault(t => t.goTarget == hit.transform.gameObject) == null)
                            {
                                Bot_Targets.Add(new Target_11(this, gameObject, hit.transform.gameObject));
                            }
                        }

                        if (hit.transform.gameObject.tag == "Wall")
                        {
                            if (Wall_Targets.FirstOrDefault(t => t.goTarget == hit.transform.gameObject) == null)
                            {
                                Wall_Targets.Add(new Target_11(this, gameObject, hit.transform.gameObject));
                            }
                        }
                    }
                }
            }
        }
    }

    [SerializeField]
    public Target_11 Nearst_Food_Target
    {
        get
        {
            if (Food_Targets.Any())
            {
                return Food_Targets.OrderBy(t => t.Distance).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }

    [SerializeField]
    public Target_11 Nearst_Wall_Target
    {
        get
        {
            if (Wall_Targets.Any())
            {
                return Wall_Targets.OrderBy(t => t.Distance).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }


    [SerializeField]
    public Target_11 Nearst_Bot_Target
    {
        get
        {
            if (Bot_Targets.Any())
            {
                return Bot_Targets.OrderBy(t => t.Distance).FirstOrDefault();
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

            if (SensorBank.DrawToNearstFoodTarget)
            {
                OnDrawToNearstFoodTarget();
            }

            if (SensorBank.DrawRay)
            {
                OnDrawRays();
            }
        }
    }

    public void OnDrawToSector()
    {
        //if (Targets.Any())
        //{
        //    Color col = Color.yellow;
        //    Vector3 from = Helper_5.GetSecLine(gameObject, 1, SensorBank.ScanRange);
        //    Vector3 to = Helper_5.GetSecLine(gameObject, Angle - 1, SensorBank.ScanRange);
        //    from.y = 0;
        //    to.y = 0;
        //    Debug.DrawLine(transform.position, from + transform.position, col);
        //    Debug.DrawLine(transform.position, to + transform.position, col);
        //    Debug.DrawLine(from + transform.position, to + transform.position, col);
        //}
    }

    public void OnDrawToNearstFoodTarget()
    {
        if (Nearst_Food_Target != null && (Nearst_Food_Target.goTarget!=null))
        {
            Color col = Color.green;
            Vector3 toTarget = Helper_5.GetSecLine(gameObject, Helper_5.GetAngleFromVector(gameObject, Nearst_Food_Target.Position), Nearst_Food_Target.Distance);
            toTarget.y = 0;
            if (Nearst_Food_Target.goTarget.tag == "Bot")
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
