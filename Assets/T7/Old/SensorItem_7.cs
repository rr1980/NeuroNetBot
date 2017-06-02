using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SensorItem_7 : MonoBehaviour
{
    [ReadOnly]
    public int Id;
    public string Name;
    public float AngleFrom;
    public float AngleTo;
    [Space(5)]
    public List<GameObject> Targets;

    private GameObject rootGO;
    private SensorController_7 sc;

    [SerializeField]
    public GameObject NearstTarget
    {
        get
        {
            if (Targets.Any())
            {
                return Targets.OrderBy(t => Vector3.Distance(t.transform.position, rootGO.transform.position)).First();
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

    void Start()
    {
        rootGO = gameObject.transform.root.gameObject;
        sc = transform.parent.GetComponent<SensorController_7>();
    }

    void Update()
    {

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
        if (sc.DrawToSector)
        {
            OnDrawToSector();
        }

        if (sc.DrawAngle)
        {
            OnDrawAngle();
        }

        if (sc.DrawToNearstTarget)
        {
            OnDrawToNearstTarget();
        }
    }

    public void OnDrawAngle()
    {
        Vector3 from = Helper_5.GetSecLine(rootGO, AngleFrom, sc.ScanRange);
        Vector3 to = Helper_5.GetSecLine(rootGO, AngleTo, sc.ScanRange);

        Debug.DrawLine(rootGO.transform.position, from + rootGO.transform.position, Color.white);
        Debug.DrawLine(rootGO.transform.position, to + rootGO.transform.position, Color.white);
    }

    public void OnDrawToSector()
    {
        if (Targets.Any())
        {
            Color col = Color.yellow;
            Vector3 from = Helper_5.GetSecLine(rootGO, AngleFrom, sc.ScanRange);
            Vector3 to = Helper_5.GetSecLine(rootGO, AngleTo, sc.ScanRange);

            Debug.DrawLine(rootGO.transform.position, from + rootGO.transform.position, col);
            Debug.DrawLine(rootGO.transform.position, to + rootGO.transform.position, col);
            Debug.DrawLine(from + rootGO.transform.position, to + rootGO.transform.position, col);
        }
    }

    public void OnDrawToNearstTarget()
    {
        if (NearstTarget != null)
        {
            Color col = Color.green;
            Vector3 toTarget = Helper_5.GetSecLine(rootGO, Helper_5.GetAngleFromVector(rootGO, NearstTarget.transform.position), Vector3.Distance(NearstTarget.transform.position, rootGO.transform.position));
            toTarget.y = 0;
            if (NearstTarget.tag == "Bot")
            {
                col = Color.red;
            }
            Debug.DrawLine(rootGO.transform.position, toTarget + rootGO.transform.position, col);
        }
    }
}
