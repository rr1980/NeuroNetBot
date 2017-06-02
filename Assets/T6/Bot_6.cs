using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;



public class Bot_6 : MonoBehaviour
{
    public float RotateSpeed;
    public float Speed;
    public float Health = 100000;

    private NetworkController_6 nc;
    private Controller_6 c;

    void Start()
    {
        //floor = GameObject.FindGameObjectWithTag("Floor");
        c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_6>();
        nc = GetComponent<NetworkController_6>();
    }

    void Update()
    {


        move();

        //if (!isOnFloor())
        //{
        //    Destroy(gameObject);
        //}
    }

    private void move()
    {
        var results = nc.Results;
        Vector3 d = Vector3.zero;
        var check = false;


        foreach (var item in results)
        {
            switch (item.Direction)
            {
                case "l": 
                    d += new Vector3(0, -item.Value, 0) * RotateSpeed * Time.deltaTime;
                    Health -= c.RotateCost;
                    break;
                case "f":
                    transform.position += (transform.forward * Time.deltaTime * Speed)*item.Value;
                    Health -= c.MoveCost;
                    check = true;
                    break;
                case "r":
                    d += new Vector3(0, item.Value, 0) * RotateSpeed * Time.deltaTime;
                    Health -= c.RotateCost;
                    break;
                //case "r":
                //    d += new Vector3(0, 1, 0) * RotateSpeed * Time.deltaTime;
                //    Health -= c.RotateCost;
                //    break;
                default:
                    break;
            }
        }


        if (!check)
        {
            Health -= c.StayCost;
        }

        transform.Rotate(d);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (c == null)
        {
            c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_6>();
        }

        if (collision.gameObject.tag == "Bot")
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (c == null)
    //    {
    //        c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_6>();
    //    }

    //    if (other.tag == "Bot")
    //    {
    //        //Debug.Log("+");
    //        //Debug.Log(Health);
    //        Health -= c.BotPain;
    //        //Debug.Log(Health);
    //        //Debug.Log("--------------------");
    //    }
    //    else if (other.tag == "Wall")
    //    {
    //        Health -= c.WallPain;
    //    }
    //}

    private void OnDrawGizmos()
    {
        DebugExtension.DrawArrow(transform.position, transform.forward * 3, Color.yellow);
    }
}

