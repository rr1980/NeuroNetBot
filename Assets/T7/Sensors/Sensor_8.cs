using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Sensor_8 : MonoBehaviour
{
    [ReadOnly]
    public int Id;
    public float RefreshTime = 0;
    public float AngleFrom;
    public float AngleTo;
    public List<Target> Targets = new List<Target>();

    private SensorBank_8 SensorBank;
    private float elapsed;
    private List<RaycastHit> hits = new List<RaycastHit>();

    [SerializeField]
    public Target NearstTarget
    {
        get
        {
            if (Targets.Any())
            {
                return Targets.OrderBy(t => t.Distance).First();
            }
            else
            {
                return null;
            }
        }
    }

    [SerializeField]
    public float AngleCenter
    {
        get
        {
            if (AngleFrom < AngleTo)
            {
                return AngleFrom + ((AngleTo - AngleFrom) / 2);
            }
            else
            {
                var a = (AngleFrom + AngleTo);

                if (a == 360)
                {
                    return 0;
                }
                else if (a > 360)
                {
                    return (a - 360) / 2;
                }
                else if (a < 360)
                {
                    a = (a - 360) / 2;
                    a = a * -1;
                }
                return a;
            }
        }
    }

    private void Start()
    {
        SensorBank = transform.root.GetComponentInChildren<SensorBank_8>();
    }



    private void Update()
    {
        if (pufferTime())
        {
            Targets = new List<Target>();
            hits = new List<RaycastHit>();

            var i = AngleFrom;

            while (i != AngleTo)
            {
                Vector3 angle = Helper_5.GetSecLine(gameObject, i, SensorBank.ScanRange);
                angle.y = 0;

                RaycastHit hit;

                if (Physics.Raycast(transform.position, angle, out hit, SensorBank.ScanRange))
                {
                    hits.Add(hit);
                    if (Targets.FirstOrDefault(t => t.goTarget == hit.transform.gameObject) == null)
                    {
                        Targets.Add(new Target(this,gameObject, hit.transform.gameObject));
                    }
                }

                i++;
                if (i == 361)
                {
                    i = 0;
                }
            }
        }
    }

    public bool Match(float angle)
    {
        if (AngleFrom > AngleTo)
        {
            return (angle > AngleFrom) || (angle < AngleTo);
        }
        else
        {
            return (angle > AngleFrom) && (angle < AngleTo);
        }
    }

    private void OnDrawGizmos()
    {
        if (SensorBank.DrawToSector)
        {
            OnDrawToSector();
        }

        if (SensorBank.DrawAngle)
        {
            OnDrawAngle();
        }

        if (SensorBank.DrawToNearstTarget)
        {
            OnDrawToNearstTarget();
        }

        if (SensorBank.DrawToStT)
        {
            foreach (var item in hits)
            {
                Vector3 angle2 = Helper_5.GetSecLine(gameObject, Helper_5.GetAngleFromVector(gameObject, item.point), item.distance);
                angle2.y = 0;
                Debug.DrawLine(transform.position, angle2 + transform.position, Color.red);
            }
        }
    }

    public void OnDrawAngle()
    {
        Vector3 from = Helper_5.GetSecLine(gameObject, AngleFrom, SensorBank.ScanRange);
        Vector3 to = Helper_5.GetSecLine(gameObject, AngleTo, SensorBank.ScanRange);

        Debug.DrawLine(transform.position, from + transform.position, Color.white);
        Debug.DrawLine(transform.position, to + transform.position, Color.white);
    }

    public void OnDrawToSector()
    {
        if (Targets.Any())
        {
            Color col = Color.yellow;
            Vector3 from = Helper_5.GetSecLine(gameObject, AngleFrom, SensorBank.ScanRange);
            Vector3 to = Helper_5.GetSecLine(gameObject, AngleTo, SensorBank.ScanRange);

            Debug.DrawLine(transform.position, from + transform.position, col);
            Debug.DrawLine(transform.position, to + transform.position, col);
            Debug.DrawLine(from + transform.position, to + transform.position, col);
        }
    }

    public void OnDrawToNearstTarget()
    {
        if (NearstTarget != null)
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

    private bool pufferTime()
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