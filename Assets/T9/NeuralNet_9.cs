using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TagValueDict
{
    public string Tag;
    public int Index;

    public TagValueDict(string tag, int index)
    {
        Tag = tag;
        Index = index;
    }
}

[ExecuteInEditMode]
public class NeuralNet_9 : MonoBehaviour
{
    public bool Bias = false;

    [Space(5)]
    public List<Input_9> Inputs;

    public List<TagValueDict> Tags;

    [Space(5)]
    [ReadOnly]
    public int InputNeurons;

    public int HiddensNeurons;
    public int HiddensNeurons2 = 0;
    public int HiddensNeurons3 = 0;

    [ReadOnly]
    public int OutputNeurons;

    //[Space(5)]

    private List<Output_9> Outputs;
    internal NeuralNet net;
    internal bool networkInit = false;

    public void Init(NeuralNet nn = null)
    {
        if (nn == null)
        {
            //Debug.Log("Init3");
            InputNeurons = Inputs.Where(i=>i.Type=="I").ToList().Count * Tags.Count;
            OutputNeurons = Outputs.Count;
            net = new NeuralNet(Bias, InputNeurons, HiddensNeurons, HiddensNeurons2, HiddensNeurons3, OutputNeurons);
        }
        else
        {
            //Debug.Log("Init2");
            net = nn;
        }

        networkInit = true;
    }

    private void Start()
    {
        //Inputs = transform.GetComponentsInChildren<Input_9>().ToList();
        Outputs = transform.GetComponentsInChildren<Output_9>().ToList();
        InputNeurons = Inputs.Where(i => i.Type == "I").ToList().Count * Tags.Count;
        OutputNeurons = Outputs.Count;

        //net = new NeuralNet(Bias, InputNeurons, HiddensNeurons, HiddensNeurons2, OutputNeurons);
    }

    private void Update()
    {
        if (net == null)
        {
            Debug.Log("Init WRONG!!!");
            return;
            //Init();
        }

        double[] _inputs = new double[InputNeurons];

        //var x = Inputs.Count / 3;

        for (int i = 0; i < Inputs.Count-3; i++)
        {
            //Debug.Log(Inputs.Count);
            //Debug.Log(i);
            var indT = Tags.FirstOrDefault(t => t.Tag == Inputs[i].Tag);
            if (indT != null)
            {
                _inputs[(i * Tags.Count) + indT.Index] = Inputs[i].Value;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            _inputs[(InputNeurons - 3) + i] = Inputs[(Inputs.Count - 3) + i].Value;
        }

            //for (int i = 0; i < Inputs.Count; i++)
            //{
            //    _inputs[i] = Inputs[i].Value;
            //}

            var results = net.Compute(_inputs);

        for (int i = 0; i < Outputs.Count; i++)
        {
            Outputs[i].Value = (float)results[i];
        }
    }
}