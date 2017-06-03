using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "T13/Neuron_13")]
public class Neuron_13 : ScriptableObject
{
    public float Value;
    public List<Synapse_13> InputSynapses;
    public List<Synapse_13> OutputSynapses;

    private void Awake()
    {
        InputSynapses = new List<Synapse_13>();
        OutputSynapses = new List<Synapse_13>();
    }

    internal void Init(Layer_13 inp)
    {
        foreach (var item in inp.Neurons)
        {
            var s = ScriptableObject.CreateInstance("Synapse_13") as Synapse_13;
            s.Init(item,this);
            item.OutputSynapses.Add(s);
            InputSynapses.Add(s);
        }
    }

    internal float CalculateValue()
    {
        return Value = Sigmoid_13.HyperbolicTangtent((float)InputSynapses.Sum(a => a.Weight * a.Input.Value));
    }

    internal void Randomize()
    {
        Debug.Log("Randomize!");
    }
}

public static class Sigmoid_13
{
    public static float Output(float x)
    {
        //return Math.Tanh(x);
        return x < -45.0f ? 0.0f : x > 45.0f ? 1.0f : 1.0f / (1.0f + (float)Math.Exp(-x));
    }

    public static float HyperbolicTangtent(float x)
    {
        if (x < -45.0f) return -1.0f;
        else if (x > 45.0f) return 1.0f;
        else return (float)Math.Tanh(x);
    }

    public static float BiPolarSigmoid(float a, float p)
    {
        float ap = (-a) / p;
        return (2 / (1 + Mathf.Exp(ap)) - 1);
    }

    public static double Derivative(double x)
    {
        return x * (1 - x);
    }
}