using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bot_5 : MonoBehaviour
{
    public float RotateSpeed;
    public float Speed;
    public float Health = 100000;

    private NetworkController_5 nc;
    private Controller_5 c;
    private GameObject floor;

    void Start()
    {
        floor = GameObject.FindGameObjectWithTag("Floor");
        c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_5>();
        nc = GetComponent<NetworkController_5>();
    }
    
    void FixedUpdate()
    {


        move();

        if (!isOnFloor())
        {
            Destroy(gameObject);
        }
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
                case "l_h":
                    d += new Vector3(0, -1, 0) * RotateSpeed * Time.deltaTime;
                    Health -= c.RotateCost;
                    break;
                case "l_s":
                    d += new Vector3(0, -0.5f, 0) * RotateSpeed * Time.deltaTime;
                    Health -= c.RotateCost;
                    break;
                case "f":
                    transform.position += transform.forward * Time.deltaTime * Speed;
                    Health -= c.MoveCost;
                    check = true;
                    break;
                case "r_s":
                    d += new Vector3(0, 0.5f, 0) * RotateSpeed * Time.deltaTime;
                    Health -= c.RotateCost;
                    break;
                case "r_h":
                    d += new Vector3(0, 1, 0) * RotateSpeed * Time.deltaTime;
                    Health -= c.RotateCost;
                    break;
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

    private void OnTriggerEnter(Collider other)
    {
        if (c == null)
        {
            c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_5>();
        }

        if (other.tag == "Bot")
        {
            //Debug.Log("+");
            //Debug.Log(Health);
            Health -= c.BotPain;
            //Debug.Log(Health);
            //Debug.Log("--------------------");
        }
        else if (other.tag == "Wall")
        {
            Health -= c.WallPain;
        }
    }

    private void OnDrawGizmos()
    {
        DebugExtension.DrawArrow(transform.position, transform.forward * 2);
    }

    private bool isOnFloor()
    {
        if (floor == null)
        {
            floor = GameObject.FindGameObjectWithTag("Floor");
        }

        Bounds bounds = floor.GetComponent<Renderer>().bounds;

        var tp = transform.position;
        var bs = bounds.size;

        if (tp.x < -(bs.x / 2) || tp.x > (bs.x / 2) || tp.z < -(bs.z / 2) || tp.z > (bs.z / 2))
        {
            return false;
        }

        return true;
    }
}