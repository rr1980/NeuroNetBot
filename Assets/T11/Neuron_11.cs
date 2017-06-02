using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Neuron_11 : MonoBehaviour
{
    public Layer_11 Layer;
    //public List<Synapse_11> InputSynapses;
    //public List<Synapse_11> OutputSynapses;
    public double Value;

    private MeshRenderer Render;

    private void OnEnable()
    {

    }

    void Start()
    {
        Render = GetComponent<MeshRenderer>();

        //if (InputSynapses == null)
        //{
        //    InputSynapses = new List<Synapse_11>();
        //    OutputSynapses = new List<Synapse_11>();
        //}

        Layer = transform.parent.GetComponent<Layer_11>();

        if(gameObject.name=="Sphere")
        {
            //var rfgfd = 0;
        }

        if (Layer.NextLayer)
        {
            foreach (var item in Layer.NextLayer.GetComponentsInChildren<Neuron_11>())
            {
                var kl = item.GetComponentsInChildren<Synapse_11>().FirstOrDefault(i=>i.InputNeuron == this);
                if (kl != null) {
                    //Debug.Log(kl.name);
                    //Debug.Log("Yes: " + Layer.name + " - " + this.name + "_Synapse_" + item.Layer.name + " - " + item.name);
                }
                else
                {
                    GameObject g = new GameObject(this.name + "_Synapse_" + item.name);
                    Synapse_11 syn = g.AddComponent<Synapse_11>();
                    syn.Init(this, item);
                    g.transform.parent = item.transform;
                    //item.InputSynapses.Add(syn);
                    //OutputSynapses.Add(syn);
                    //Debug.Log("NO: "+Layer.name+ " - " + this.name + "_Synapse_" + item.Layer.name + " - " + item.name);
                }

                //if (item.InputSynapses.FirstOrDefault(s => s.name == this.name + "_Synapse_" + item.name))
                //{
                //    //Debug.Log(Layer.name + " - " +this.name + "_Synapse_" + item.name);
                //}

                //GameObject g = new GameObject(this.name+"_Synapse_"+item.name);
                //Synapse_11 syn =  g.AddComponent<Synapse_11>();
                //syn.Init(this, item);
                //g.transform.parent = transform;
                //item.InputSynapses.Add(syn);
                //OutputSynapses.Add(syn);
            }
        }
    }

    void Update()
    {
        var v = getValue();
        if (GetComponentsInChildren<Synapse_11>().Any())
        {
            transform.localScale = new Vector3(v, v, v);
        }
    }

    private float getValue()
    {
        var v = (float)Value;
        if (v < 0)
        {
            v *= -1;
        }
        v += 0.5f;
        return v;
    }

    internal double CalculateValue()
    {
        var insy = GetComponentsInChildren<Synapse_11>();
        return Value = Sigmoid.Output(insy.Sum(a => a.Weight * a.InputNeuron.Value));
        //return Value = Sigmoid.Output(InputSynapses.Sum(a => a.Weight * a.InputNeuron.Value));
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (Value > 0.5f)
            {
                Render.material.color = Color.red;
            }
            else
            {
                Render.material.color = Color.green;
            }
        }
        else
        {
            if (Value > 0.5f)
            {
                Render.sharedMaterial.color = Color.red;
            }
            else
            {
                Render.sharedMaterial.color = Color.green;
            }
        }
    }
}
