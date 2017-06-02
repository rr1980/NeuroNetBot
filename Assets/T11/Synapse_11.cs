using UnityEngine;

[ExecuteInEditMode]
public class Synapse_11 : MonoBehaviour
{
    public Neuron_11 InputNeuron;
    public Neuron_11 OutputNeuron;
    public double Weight;

    LineRenderer lr;

    public void Init(Neuron_11 input, Neuron_11 output)
    {
        this.InputNeuron = input;
        this.OutputNeuron = output;
        Weight = NeuralNet.GetRandom();
    }

    private void Start()
    {
        if(InputNeuron==null || OutputNeuron == null)
        {
            if (Application.isPlaying)
            {
                Destroy(this);
            }
            else
            {
                DestroyImmediate(this);
            }
        }

        lr = gameObject.GetComponent<LineRenderer>();
        if (lr == null)
        {
            lr = gameObject.AddComponent<LineRenderer>();
        }

        lr.material = new Material(Shader.Find("Standard"));
        lr.startColor = getColor();
        lr.sharedMaterial.color = getColor();
        lr.startWidth = getWeight();
        lr.endWidth = getWeight();
        lr.SetPosition(0, InputNeuron.transform.position);
        lr.SetPosition(1, OutputNeuron.transform.position);
    }

    private void Update()
    {
        //Debug.Log(name);
        if(name=="Sphere_Synapse_Neuron (1)")
        {
            //var fsfd = 0;
        }
        if (InputNeuron == null || OutputNeuron == null)
        {
            if (Application.isPlaying)
            {
                Destroy(this);
            }
            else
            {
                DestroyImmediate(this);
            }
        }

        lr.SetPosition(0, InputNeuron.transform.position);
        lr.SetPosition(1, OutputNeuron.transform.position);
        lr.sharedMaterial.color = getColor();
        lr.startColor = getColor();
        lr.startWidth = getWeight();
        lr.endWidth = getWeight();

        Debug.Log(InputNeuron.transform.lossyScale);
    }

    private float getWeight()
    {
        var v = (float)Weight/3;
        if (v < 0)
        {
            v *= -1;
        }

        var vv = InputNeuron.transform.lossyScale.x;
        return v*vv;
    }

    private Color getColor()
    {
        Color col;
        if (Weight > 0)
        {
            col = Color.red;
        }
        else
        {
            col = Color.green;
        }

        return col;
    }
}
