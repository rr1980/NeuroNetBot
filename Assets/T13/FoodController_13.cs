using UnityEngine;
public class FoodController_13 : MonoBehaviour
{
    private Controller_13 c;

    void Start()
    {
        c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_13>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.gameObject.tag == "Bot")
        {
            var bot = collision.transform.root.gameObject.GetComponent<Bot_13>();
            if (bot != null)
            {
                if (c != null)
                {
                    bot.Food += c.FoodGod;
                    gameObject.SetActive(false);
                    //var x = UnityEngine.Random.Range(c.FoodSpawnRange.x, c.FoodSpawnRange.y);
                    //var z = UnityEngine.Random.Range(c.FoodSpawnRange.x, c.FoodSpawnRange.y);
                    //transform.position = new Vector3(x, 0.5f, z);
                }
            }
        }
    }
}