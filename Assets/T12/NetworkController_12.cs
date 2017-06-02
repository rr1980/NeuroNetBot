using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Synapse_12
{
    [SerializeField]
    private Neuron_12 InputNeuron;
    [SerializeField]
    private Neuron_12 OutputNeuron;

    public Synapse_12(Neuron_12 input, Neuron_12 output)
    {
        InputNeuron = input;
        OutputNeuron = output;
    }
}

[System.Serializable]
public class Neuron_12
{
    public int Index;
    public List<Synapse_12> InputSynapse = new List<Synapse_12>();
    public List<Synapse_12> OutputSynapse = new List<Synapse_12>();

    private NetworkController_12 _nc;
    private Layer_12 _layer;

    public Neuron_12()
    {
        //Debug.Log("Neuron init!");
    }

    internal void Init(int index, Layer_12 layer, NetworkController_12 nc)
    {
        InputSynapse = new List<Synapse_12>();
        OutputSynapse = new List<Synapse_12>();
        Index = index;
        _layer = layer;
        _nc = nc;

        if (index + 1 < nc.Layers.Count)
        {
            foreach (var item in nc.Layers[index + 1].Neurons)
            {
                var syn = new Synapse_12(this,item);
                item.InputSynapse.Add(syn);
                OutputSynapse.Add(syn);
            }
        }
    }
}

[System.Serializable]
public class Layer_12
{
    //[ReadOnly]
    public List<Neuron_12> Neurons;
    public int Index;

    private NetworkController_12  _nc;


    public Layer_12()
    {
        Neurons = new List<Neuron_12>();
        //Debug.Log("Layer init!");
    }

    internal void Init(int index,NetworkController_12 nc)
    {
        Index = index;
        _nc =nc;
        foreach (var item in Neurons)
        {
            item.Init(Neurons.IndexOf(item), this, _nc);
        }
    }
}

[ExecuteInEditMode]
public class NetworkController_12 : MonoBehaviour
{
    public bool init;
    public List<Layer_12> Layers;

    private void Start()
    {
        if (!init)
        {
            foreach (var item in Layers)
            {
                item.Init(Layers.IndexOf(item),this);
            }
        }
    }

    private void Update()
    {
    }
}
