using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sensor_3 : MonoBehaviour
{
    [Header("Debug")]
    public bool debug = true;
    public bool rayToTarget = true;
    public bool rayToSector = true;
    public bool activeSectorsOnly = false;
    public bool drawSector = true;
    public bool drawFoodScanRange = true;
    public bool drawBotScanRange = true;
    public bool drawWallScanRange = true;

    [Header("Settings")]
    public float RefreshTime = 0.5f;
    public float FoodScanRange = 15;
    public float WallScanRange = 5;
    public float BotScanRange = 5;
    public float FromAngle;
    public float ToAngle;
    public int SectorCount = 1;

    [Header("Results")]
    public List<Sector_3> Sectors;

    private float elapsed;
    private Collider selfCollider;

    void Start()
    {
        selfCollider = GetComponent<Collider>();
        Sectors = new List<Sector_3>();

        var steps = (ToAngle - FromAngle) / SectorCount;
        float angle = FromAngle;

        for (int i = 0; i < SectorCount; i++)
        {
            Sectors.Add(new Sector_3(i, gameObject, angle, angle + steps));
            angle += steps;
        }
    }

    void Update()
    {
        if (pufferTime())
        {
            Sectors.ForEach(s => s.Targets = new List<GameObject>());
            List<Collider> food_collider = Physics.OverlapSphere(transform.position, FoodScanRange).Where(hc => hc.tag == "Food").ToList();
            List<Collider> bot_collider = Physics.OverlapSphere(transform.position, BotScanRange).Where(hc => hc.tag == "Bot").ToList();
            List<Collider> wall_collider = Physics.OverlapSphere(transform.position, WallScanRange).Where(hc => hc.tag == "Wall").ToList();
            bot_collider.Remove(selfCollider);

            setTargets(food_collider);
            setTargets(bot_collider);
            setTargets(wall_collider);
        }
    }

    private void setTargets(List<Collider> hitColliders)
    {
        foreach (var item in hitColliders)
        {
            float angleToTarget = Helper.GetAngleToTarget(gameObject, item.gameObject);
            var sector = Sectors.FirstOrDefault(s => s.AngleFrom < angleToTarget && s.AngleTo > angleToTarget);
            if (sector != null)
            {
                sector.Targets.Add(item.transform.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            IEnumerable<Sector_3> secs = new List<Sector_3>();
            if (activeSectorsOnly)
            {
                secs = Sectors.Where(s => s.Targets.Any());
            }
            else
            {
                secs = Sectors;
            }

            foreach (var sec in secs)
            {
                if (drawFoodScanRange)
                {
                    DebugExtension.DrawCircle(transform.position, Color.green, FoodScanRange);
                }

                if (drawWallScanRange)
                {
                    DebugExtension.DrawCircle(transform.position, Color.white, WallScanRange);
                }

                if (drawBotScanRange)
                {
                    DebugExtension.DrawCircle(transform.position, Color.yellow, BotScanRange);
                }

                Vector3 from = getSecLine(sec.AngleFrom, FoodScanRange);
                Vector3 to = getSecLine(sec.AngleTo, FoodScanRange);
                from.y = 1;
                to.y = 1;

                Color col = Color.blue;
                if (sec.Targets.Any())
                {
                    col = Color.red;
                    foreach (var item in sec.Targets)
                    {
                        if (item.tag == "Bot")
                        {
                            if (rayToSector)
                            {
                                Vector3 center = getSecLine(sec.AngleCenter, FoodScanRange / 4);
                                center.y = 1;
                                Debug.DrawLine(transform.position, center, Color.red);
                            }

                            if (rayToTarget)
                            {
                                Vector3 toTarget = getSecLine(Helper.GetAngleToTarget(gameObject, item.gameObject), Vector3.Distance(item.transform.position, transform.position));
                                toTarget.y = 1;
                                Debug.DrawLine(transform.position, toTarget, Color.red);

                            }
                        }

                    }
                }

                if (drawSector)
                {
                    Debug.DrawLine(transform.position, from, col);
                    Debug.DrawLine(transform.position, to, col);
                }
            }

            if (rayToTarget)
            {
                var se = secs.Where(s => s.NearstTarget != null && (s.NearstTarget.tag == "Food")).ToList();

                //Debug.Assert(se.Count() > 0, "Fehler1"+ se.Count());
                //Debug.Assert(se != null, "Fehler1");
                //Debug.Assert(se.Any() == true, "Fehler2");
                //Debug.Assert(se.Select(s => s.NearstTarget).Any() == true, "Fehler3");


                if (se.Any())
                {
                    var see = se.OrderBy(t => Vector3.Distance(t.NearstTarget.transform.position, transform.position)).FirstOrDefault();

                    if (see != null)
                    {
                        //var see = see.NearstTarget;

                        float dist = Vector3.Distance(see.NearstTarget.gameObject.transform.position, transform.position);
                        Vector3 toTarget = getSecLine(Helper.GetAngleToTarget(gameObject, see.NearstTarget.gameObject), dist);
                        toTarget.y = 1;
                        Debug.DrawLine(transform.position, toTarget, Color.green);
                    }
                }
            }
        }
    }

    private Vector3 getSecLine(float angleFrom, float scanRange)
    {
        return Helper.GetSecLine(angleFrom, scanRange) + transform.position;
    }

    private bool pufferTime()
    {
        elapsed += Time.deltaTime;
        if (elapsed > RefreshTime)
        {
            elapsed = 0;
            return true;
        }

        return false;
    }
}
