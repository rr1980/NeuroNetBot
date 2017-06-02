using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class NN_11 : MonoBehaviour
{
    public List<Layer_11> Layers;
    int h = 1;

    void Start()
    {
        //foreach (var item in GameObject.FindObjectsOfType<Synapse_11>())
        //{

        //    if (Application.isPlaying)
        //    {
        //        GameObject.Destroy(item.gameObject);
        //    }
        //    else
        //    {
        //        GameObject.DestroyImmediate(item.gameObject);
        //    }
        //}
        h = Layers.Count - 2;
    }

    void Update()
    {
        //  Input bestücken...

        for (int i = 1; i <= h; i++)
        {
            foreach (var item in Layers[i].GetComponentsInChildren<Neuron_11>())
            {
                item.CalculateValue();
            }
        }

        foreach (var item in Layers.Last().GetComponentsInChildren<Neuron_11>())
        {
            item.CalculateValue();
        }
    }
}
