using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Sector_3
{
    public int Id;
    public float AngleFrom;
    public float AngleTo;

    [SerializeField]
    public GameObject NearstTarget
    {
        get
        {
            if (Targets.Any())
            {
                return Targets.OrderBy(t => Vector3.Distance(t.transform.position, self.transform.position)).FirstOrDefault();
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
            return AngleFrom + ((AngleTo - AngleFrom) / 2);
        }
    }

    public List<GameObject> Targets;

    private GameObject self;

    public Sector_3(int i,GameObject go,float angleFrom, float angleTo)
    {
        Id = i;
        self = go;
        Targets = new List<GameObject>();
        AngleFrom = angleFrom;
        AngleTo = angleTo;
    }
}
