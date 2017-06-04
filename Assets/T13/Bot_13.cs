using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Bot_13 : MonoBehaviour
{
    public bool canMove;
    public bool canRotate;
    public float RotateSpeed;
    public float Speed;
    [Space(5)]
    public float[] Output;

    [Space(5)]
    [ReadOnly]
    public int Inputs;
    public List<int> Hiddens;
    public int Outputs;

    [Space(5)]
    [ReadOnly]
    public float Range;
    [ReadOnly]
    public float Food;
    public float Fitness
    {
        get
        {
            return Range + Food;
        }
    }

    [Space(5)]
    public GameObject Goal;
    public NN_13 NN;
    public GameObject SensorBank;
    private SensorBank_13 sb;
    private Vector3 StartPoint;

    public float MyProperty { get; set; }

    void Start()
    {
        Goal = GameObject.FindGameObjectWithTag("Finish");
        StartPoint = transform.root.position;

        var t = GetComponentsInChildren<SensorBank_13>();
        if (t.Length < 1)
        {
            SensorBank = Instantiate(SensorBank);
            SensorBank.name = "SensorBank";
            SensorBank.transform.parent = transform;
            SensorBank.transform.position = Vector3.zero;
        }
        else
        {
            for (int i = 1; i < t.Length; i++)
            {
                GameObject.DestroyImmediate(t[i].gameObject);
            }
        }

        sb = SensorBank.GetComponent<SensorBank_13>();

        Inputs = sb.Count;

        if (!Application.isPlaying)
        {
            NN = ScriptableObject.CreateInstance("NN_13") as NN_13;
            NN.Init(Inputs, Hiddens, Outputs);
        }
    }

    public void Init(NN_13 nn)
    {
        NN = nn;
    }

    void Update()
    {
        if (NN == null)
        {
            return;
        }

        Compute();
        Move();

        //Fitness = (Goal.transform.position - StartPoint) - (Goal.transform.position - transform.position);
        //Fitness = Vector3.Distance(Goal.transform.position, StartPoint) - Vector3.Distance(Goal.transform.position, transform.root.position);
        Range = Vector3.Distance(StartPoint, transform.position);
    }

    private void Move()
    {
        //var r = Mathf.Clamp((Output[0] + Output[1]), 0.1f, 0.9f);
        Vector3 d = Vector3.zero;

        //d += new Vector3(0, -Output[0], 0) * RotateSpeed * Time.deltaTime;
        d += new Vector3(0, Output[0], 0) * RotateSpeed * Time.deltaTime;

        if (canRotate)
        {
            //Fitness -= 1f;
            transform.Rotate(d);
        }

        if (canMove)
        {
            //Fitness -= 1f;
            //transform.position += (transform.forward * Time.deltaTime * Speed) * r;
            transform.position += (transform.forward * Time.deltaTime * Speed) * (Output[1] + 1);
        }
    }

    public void Compute()
    {
        float[] inps = new float[Inputs];

        for (int i = 0; i < sb.Sensors.Count; i++)
        {
            inps[i] = sb.Sensors[i].Distance;
        }

        Output = NN.Compute(inps);
    }

    private void OnDrawGizmos()
    {
        DebugExtension.DrawArrow(transform.position, transform.forward * 2, Color.red);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.tag == "Wall")
        {
            //Debug.Log("BÄM");
            //gameObject.SetActive(false);

            canMove = false;
            canRotate = false;
            gameObject.GetComponent<Renderer>().material.color = Color.black;


            //var bot = collision.transform.root.gameObject.GetComponent<BotController_9>();
            //if (bot != null)
            //{
            //    if (c != null)
            //    {
            //        bot.Health += c.FoodGod;
            //        var x = UnityEngine.Random.Range(c.FoodSpawnRange.x, c.FoodSpawnRange.y);
            //        var z = UnityEngine.Random.Range(c.FoodSpawnRange.x, c.FoodSpawnRange.y);
            //        transform.position = new Vector3(x, 0.5f, z);
            //    }
            //}
        }
    }
}
