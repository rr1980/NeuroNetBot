using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class NetworkController_5 : MonoBehaviour
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
    public bool Bias=true;
    [Space(5)]
    public List<Results> Results;

    private SensorController_5 sc;
    private float elapsed;

    internal NeuralNet net;
    internal bool networkInit = false;

    public void Init(NeuralNet nn = null)
    {
        if (nn == null)
        {
            net = new NeuralNet(Bias,InpuNeuron, HiddenNeuron1, HiddenNeuron2,0, OutputNeuron);
        }
        else
        {
            net = nn;
        }

        networkInit = true;
    }

    void Start()
    {
        sc = GetComponent<SensorController_5>();
        InpuNeuron = sc.Sensors.Count * 3;
        OutputNeuron = sc.Sensors.Count;
    }

    void FixedUpdate()
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

    private List<Results> buildResults(double[] outputs)
    {
        List<Results> _results = new List<Results>();

        for (int i = 0; i < sc.Sensors.Count; i++)
        {
            if (outputs[i] > 0.5)
            {
                _results.Add(new Results(sc.Sensors[i].Name));
            }
        }

        return _results;
    }

    private double[] buildInputs()
    {
        var foodTargets = sc.GetTargetSensors("Food");
        var wallTargets = sc.GetTargetSensors("Wall");
        var botTargets = sc.GetTargetSensors("Bot");

        double[] _inputs = new double[InpuNeuron];

        if (foodTargets.Any())
        {
            _inputs[foodTargets.First().SensorId] = 1;
        }

        if (wallTargets.Any())
        {
            _inputs[wallTargets.First().SensorId + sc.Sensors.Count] = 1;
        }

        if (botTargets.Any())
        {
            _inputs[botTargets.First().SensorId + (sc.Sensors.Count * 2)] = 1;
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
