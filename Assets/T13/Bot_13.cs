using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
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


    public NN_13 NN;
    public GameObject SensorBank;
    private SensorBank_13 sb;

    void Start()
    {
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

        NN = ScriptableObject.CreateInstance("NN_13") as NN_13;
        NN.Init(this, Inputs, Hiddens, Outputs);
    }

    void Update()
    {
        Compute();
        Move();
    }

    private void Move()
    {
        var r = Output[0] + Output[1];
        Vector3 d = Vector3.zero;

        d += new Vector3(0, -Output[0], 0) * RotateSpeed * Time.deltaTime;
        d += new Vector3(0, Output[1], 0) * RotateSpeed * Time.deltaTime;

        if (canRotate)
        {
            transform.Rotate(d);
        }

        if (canMove)
        {
            transform.position += (transform.forward * Time.deltaTime * Speed) * r;
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
}
