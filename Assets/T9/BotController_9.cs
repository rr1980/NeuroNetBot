using UnityEngine;

[ExecuteInEditMode]
public class BotController_9 : MonoBehaviour
{
    public float Health = 100000;
    [ReadOnly]
    public int TargetsCount;

    private SensorBank_9 sb;
    public Controller_9 c;


    private void Start()
    {
        sb = GetComponentInChildren<SensorBank_9>();
        c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_9>();
    }

    private void Update()
    {
        TargetsCount = sb.Targets.Count;
    }

    private void OnDrawGizmos()
    {
        DebugExtension.DrawArrow(transform.position, transform.forward * 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (c == null)
        {
            c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_9>();
        }

        if (collision.transform.root.gameObject.tag == "Bot")
        {
            //Debug.Log("+");
            //Debug.Log(Health);
            Health -= c.BotPain;
            //Debug.Log(Health);
            //Debug.Log("--------------------");
        }
        else if (collision.gameObject.tag == "Wall")
        {
            Health -= c.WallPain;
        }
    }

}