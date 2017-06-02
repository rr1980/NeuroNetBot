using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebuggerController : MonoBehaviour
{
    public GameObject Selected;
    public bool _new;

    void Start()
    {
         
    }

    void Update()
    {
        if(_new)
        {
            _new = false;

            NeuralNet_9 n = Selected.GetComponentInChildren<NeuralNet_9>();
            if (n == null)
            {
                Debug.Log("FUCK");
            }



        }
    }
}
