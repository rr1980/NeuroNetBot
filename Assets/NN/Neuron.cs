using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Neuron
{
    public List<Synapse> InputSynapses;
    public List<Synapse> OutputSynapses;
    public double Bias;
    public double BiasDelta;
    public double Gradient;
    public double Value;

    private bool isBias;

    public Neuron(bool bias)
    {
        isBias = bias;
        InputSynapses = new List<Synapse>();
        OutputSynapses = new List<Synapse>();
        Bias = NeuralNet.GetRandom();
    }

    public Neuron(IEnumerable<Neuron> inputNeurons,bool bias) : this(bias)
    {
        foreach (var inputNeuron in inputNeurons)
        {
            var synapse = new Synapse(inputNeuron, this);
            inputNeuron.OutputSynapses.Add(synapse);
            InputSynapses.Add(synapse);
        }
    }

    public virtual double CalculateValue()
    {
        if (isBias)
        {
            return Value = Sigmoid.Output(InputSynapses.Sum(a => a.Weight * a.InputNeuron.Value) + Bias);
        }
        else
        {
            return Value = Sigmoid.Output(InputSynapses.Sum(a => a.Weight * a.InputNeuron.Value));
        }
    }

    public double CalculateError(double target)
    {
        return target - Value;
    }

    public double CalculateGradient(double? target = null)
    {
        if (target == null)
            return Gradient = OutputSynapses.Sum(a => a.OutputNeuron.Gradient * a.Weight) * Sigmoid.Derivative(Value);

        return Gradient = CalculateError(target.Value) * Sigmoid.Derivative(Value);
    }

    public void UpdateWeights(double learnRate, double momentum)
    {
        var prevDelta = BiasDelta;
        BiasDelta = learnRate * Gradient;
        if (isBias)
        {
            Bias += BiasDelta + momentum * prevDelta;
        }

        foreach (var synapse in InputSynapses)
        {
            prevDelta = synapse.WeightDelta;
            synapse.WeightDelta = learnRate * Gradient * synapse.InputNeuron.Value;
            synapse.Weight += synapse.WeightDelta + momentum * prevDelta;
        }
    }

}

public class Synapse
{
    public Neuron InputNeuron;
    public Neuron OutputNeuron;
    public double Weight;
    public double WeightDelta;

    public Synapse(Neuron inputNeuron, Neuron outputNeuron)
    {
        InputNeuron = inputNeuron;
        OutputNeuron = outputNeuron;
        Weight = NeuralNet.GetRandom();
    }
}

public static class Sigmoid
{
    public static double Output(double x)
    {
        //return Math.Tanh(x);
        return x < -45.0 ? 0.0 : x > 45.0 ? 1.0 : 1.0 / (1.0 + Math.Exp((float)-x));
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

public class DataSets
{
    public double[] Values { get; set; }
    public double[] Targets { get; set; }

    public DataSets(double[] values, double[] targets)
    {
        Values = values;
        Targets = targets;
    }
}