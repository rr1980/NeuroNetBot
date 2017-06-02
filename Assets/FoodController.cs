using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    private Controller_9 c;

    void Start()
    {
        //c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_9>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.gameObject.tag == "Bot")
        {
            var bot = collision.transform.root.gameObject.GetComponent<BotController_9>();
            if (bot != null)
            {
                if (c != null)
                {
                    bot.Health += c.FoodGod;
                    var x = UnityEngine.Random.Range(c.FoodSpawnRange.x, c.FoodSpawnRange.y);
                    var z = UnityEngine.Random.Range(c.FoodSpawnRange.x, c.FoodSpawnRange.y);
                    transform.position = new Vector3(x, 0.5f, z);
                }
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Bot")
    //    {
    //        var bot = other.GetComponent<Bot_6>();
    //        if (bot != null)
    //        {
    //            if (c != null)
    //            {
    //                bot.Health += c.FoodGod;
    //                var x = UnityEngine.Random.Range(c.FoodSpawnRange.x, c.FoodSpawnRange.y);
    //                var z = UnityEngine.Random.Range(c.FoodSpawnRange.x, c.FoodSpawnRange.y);
    //                transform.position = new Vector3(x, 0.5f, z);
    //            }
    //        }
    //    }
    //}
}
