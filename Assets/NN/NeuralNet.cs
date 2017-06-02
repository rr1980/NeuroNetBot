using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NeuralNet
{
    [SerializeField]
    public double LearnRate;
    public double Momentum;
    public List<Neuron> InputLayer;
    public List<Neuron> HiddenLayer1;
    public List<Neuron> HiddenLayer2;
    public List<Neuron> HiddenLayer3;
    public List<Neuron> OutputLayer;
    public bool Init = false;
    public readonly bool Bias;
    public List<double> errors = new List<double>();

    private static readonly System.Random Random = new System.Random(Guid.NewGuid().GetHashCode());
    //int count = 0;

    public NeuralNet(bool bias,int inputSize, int hiddenSize1, int hiddenSize2, int hiddenSize3, int outputSize, double? learnRate = null, double? momentum = null)
    {
        Bias = bias;
        Init = true;
        LearnRate = learnRate ?? .4;
        Momentum = momentum ?? .9;
        InputLayer = new List<Neuron>();
        HiddenLayer1 = new List<Neuron>();
        if (hiddenSize2 > 0)
        {
            HiddenLayer2 = new List<Neuron>();
        }
        if (hiddenSize3 > 0)
        {
            HiddenLayer3 = new List<Neuron>();
        }
        OutputLayer = new List<Neuron>();

        List<Neuron> nextLayer;
        for (var i = 0; i < inputSize; i++)
        {
            InputLayer.Add(new Neuron(bias));
        }


        nextLayer = InputLayer;
        for (var i = 0; i < hiddenSize1; i++)
        {
            HiddenLayer1.Add(new Neuron(nextLayer, bias));
        }


        nextLayer = HiddenLayer1;

        if (hiddenSize2 > 0)
        {
            for (var i = 0; i < hiddenSize2; i++)
            {
                HiddenLayer2.Add(new Neuron(nextLayer, bias));
            }

            nextLayer = HiddenLayer2;

            //for (var i = 0; i < outputSize; i++)
            //    OutputLayer.Add(new Neuron(HiddenLayer2, bias));
        }
        
        if (hiddenSize3 > 0)
        {
            for (var i = 0; i < hiddenSize3; i++)
            {
                HiddenLayer3.Add(new Neuron(nextLayer, bias));
            }

            nextLayer = HiddenLayer3;
            //for (var i = 0; i < outputSize; i++)
            //    OutputLayer.Add(new Neuron(HiddenLayer3, bias));
        }

        for (var i = 0; i < outputSize; i++)
        {
            OutputLayer.Add(new Neuron(nextLayer, bias));
        }
        



    }

    public List<double> TrainC(List<DataSets> dataSets, int numEpochs)
    {
        //double error = 0;
        for (var i = 0; i < numEpochs; i++)
        {
            //count++;
            errors = new List<double>();
            foreach (var dataSet in dataSets)
            {
                ForwardPropagate(dataSet.Values);
                BackPropagate(dataSet.Targets);
                errors.Add(CalculateError(dataSet.Targets));
            }

            //error = errors.Average();
            //if (count > 50)
            //{
            //    //Console.WriteLine("error: " + error);
            //    count = 0;
            //}
            ////Console.WriteLine("error: " + error);
        }

        return errors;
    }

    public void TrainE(List<DataSets> dataSets, double minimumError)
    {
        var error = 1.0;
        var numEpochs = 0;
        var count = 0;

        while (error > minimumError && numEpochs < int.MaxValue)
        {
            count++;
            var errors = new List<double>();
            foreach (var dataSet in dataSets)
            {
                ForwardPropagate(dataSet.Values);
                BackPropagate(dataSet.Targets);
                errors.Add(CalculateError(dataSet.Targets));
            }
            error = errors.Average();
            numEpochs++;
            if (count > 50)
            {
                Console.WriteLine("error: " + error);
                count = 0;
            }
            //Console.WriteLine("numEpochs: " + numEpochs);
        }
        Console.WriteLine("error: " + error);
    }

    private void ForwardPropagate(params double[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            InputLayer[i].Value = inputs[i];
            //i++;          WHY!?
        }
        //InputLayer.ForEach(a => a.Value = inputs[i++]);
        HiddenLayer1.ForEach(a => a.CalculateValue());
        if (HiddenLayer2!=null)
        {
            HiddenLayer2.ForEach(a => a.CalculateValue());
        }
        if (HiddenLayer3 != null)
        {
            HiddenLayer3.ForEach(a => a.CalculateValue());
        }
        OutputLayer.ForEach(a => a.CalculateValue());
    }

    private void BackPropagate(params double[] targets)
    {
        var i = 0;
        OutputLayer.ForEach(a => a.CalculateGradient(targets[i++]));
        HiddenLayer1.ForEach(a => a.CalculateGradient());
        HiddenLayer1.ForEach(a => a.UpdateWeights(LearnRate, Momentum));
        OutputLayer.ForEach(a => a.UpdateWeights(LearnRate, Momentum));
    }

    public double[] Compute(params double[] inputs)
    {
        ForwardPropagate(inputs);
        return OutputLayer.Select(a => a.Value).ToArray();
    }

    private double CalculateError(params double[] targets)
    {
        var i = 0;
        return OutputLayer.Sum(a => Math.Abs((float)a.CalculateError(targets[i++])));
    }

    public static double GetRandom()
    {
        return 2 * Random.NextDouble() - 1;
    }
}
