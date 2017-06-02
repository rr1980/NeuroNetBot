using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class NetworkController_6 : MonoBehaviour
{
    public float RefreshTime = 0.5f;
    [Header("Network")]
    //public bool draw_Neuro = true;
    [ReadOnly]
    public int InpuNeuron = 15;
    public int HiddenNeuron1 = 12;
    public int HiddenNeuron2 = 8;
    [ReadOnly]
    public int OutputNeuron = 5;
    [Space(5)]
    public bool Bias = true;

    public List<NNResult> Results;

    private SensorController_6 sc;
    private float elapsed;

    internal NeuralNet net;

    internal bool networkInit = false;

    public void Init(NeuralNet nn = null)
    {
        if (nn == null)
        {
            net = new NeuralNet(Bias, InpuNeuron, HiddenNeuron1, HiddenNeuron2,0, OutputNeuron);
        }
        else
        {
            InpuNeuron = nn.InputLayer.Count;
            net = nn;
        }

        networkInit = true;
    }

    void Start()
    {
        sc = GetComponent<SensorController_6>();
        InpuNeuron = sc.Sensors.Count;
        OutputNeuron = sc.Sensors.Count;
    }


    private void OnEnable()
    {
        sc = GetComponent<SensorController_6>();
        InpuNeuron = sc.Sensors.Count;
        OutputNeuron = sc.Sensors.Count;
    }
    private void Update()
    {
        if (net == null)
        {
            Init();
        }

        if (pufferTime())
        {
            var inputs = buildInputs();
            var outputs = net.Compute(inputs);

            Results = buildResults(outputs);
        }
    }

    private List<NNResult> buildResults(double[] outputs)
    {
        List<NNResult> _results = new List<NNResult>();

        for (int i = 0; i < sc.Sensors.Count; i++)
        {
            if (outputs[i] > 0.5)
            {
                _results.Add(new NNResult(sc.Sensors[i].Name, outputs[i] - 0.5));
            }
        }
        //if (_results.Any(a=>a.Direction=="l"))
        //{
        //    Debug.Log("L");
        //}
        //else if (_results.Any(a => a.Direction == "r"))
        //{
        //    Debug.Log("r");
        //}
        //else if (_results.Any(a => a.Direction == "f"))
        //{
        //    Debug.Log("f");
        //}
        return _results;
    }

    private double[] buildInputs()
    {
        var target = sc.GetNearstTargetSensors();
        double[] _inputs = new double[InpuNeuron];

        if (target != null)
        {
            if (target.FoundIndex + 1 > InpuNeuron - 3)
            {
                InpuNeuron++;
                //net.InputLayer.Add(new Neuron(Bias));
                net = NNAnalyzer_6.CopyNNInNN(net, net.InputLayer.Count+1, HiddenNeuron1, HiddenNeuron2, OutputNeuron);
                _inputs = new double[InpuNeuron];
            }

            _inputs[target.SensorId] = 1;
            _inputs[target.FoundIndex+sc.Sensors.Count] = 1;
        }
        return _inputs;
    }

    private bool pufferTime()
    {
        elapsed += Time.deltaTime;
        if (elapsed > RefreshTime)
        {
            elapsed = 0;
            return true;
        }

        return false;
    }

}

