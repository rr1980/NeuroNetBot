using UnityEngine;

[ExecuteInEditMode]
public class BodyController_9 : MonoBehaviour
{
    public Controller_9 c;
    public BotController_9 bc;

    private void Start()
    {
        c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_9>();
        bc = transform.root.GetComponent<BotController_9>();
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
            bc.Health -= c.BotPain;
            //Debug.Log(Health);
            //Debug.Log("--------------------");
        }
        else if (collision.gameObject.tag == "Wall")
        {
            bc.Health -= c.WallPain;
        }
    }
}
