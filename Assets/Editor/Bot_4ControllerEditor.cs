//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(BotController_4))]
//public class Bot_4ControllerEditor : Editor
//{
//    private int index1 = -1;
//    private List<double> newgens1;

//    public override void OnInspectorGUI()
//    {
//        BotController_4 bc = (BotController_4)target;


//        if (GUILayout.Button("Clear Best-Dna"))
//        {
//            bc.BiggestHealth = 0;
//            bc.bestDna = String.Empty;
//        }

//        if (GUILayout.Button("Clear Build-Dna"))
//        {
//            bc.buildDna = String.Empty;
//        }

//        if (GUILayout.Button("CreateBot from Best-Dna"))
//        {
//            if (String.IsNullOrEmpty(bc.bestDna))
//            {
//                return;
//            }
//            newgens1 = bc.bestDna.Split(',').Select(s => Convert.ToDouble(s)).ToList();
//            index1 = -1;

//            var net = buildNN1(bc);

//            bc.buildGameObject(net, 3);
//        }

//        if (GUILayout.Button("CreateBot from Build-Dna"))
//        {
//            if (String.IsNullOrEmpty(bc.buildDna))
//            {
//                return;
//            }
//            newgens1 = bc.buildDna.Split(',').Select(s => Convert.ToDouble(s)).ToList();
//            index1 = -1;

//            var net = buildNN1(bc);

//            bc.buildGameObject(net, 3);
//        }

//        DrawDefaultInspector();
//    }

//    public NeuralNet buildNN1(BotController_4 bc)
//    {
//        var nn = new NeuralNet(bc.InpuNeuron, bc.HiddenNeuron1, bc.HiddenNeuron2, bc.OutputNeuron);

//        for (int j = 0; j < nn.InputLayer.Count; j++)
//        {
//            nn.InputLayer[j].Bias = nextGen1();
//            nn.InputLayer[j] = buildNeuron1(nn.InputLayer[j]);
//        }

//        for (int j = 0; j < nn.HiddenLayer1.Count; j++)
//        {
//            nn.InputLayer[j].Bias = nextGen1();
//            nn.HiddenLayer1[j] = buildNeuron1(nn.HiddenLayer1[j]);
//        }

//        if (nn.HiddenLayer2 != null)
//        {
//            for (int j = 0; j < nn.HiddenLayer2.Count; j++)
//            {
//                nn.InputLayer[j].Bias = nextGen1();
//                nn.HiddenLayer2[j] = buildNeuron1(nn.HiddenLayer2[j]);
//            }
//        }

//        for (int j = 0; j < nn.OutputLayer.Count; j++)
//        {
//            nn.InputLayer[j].Bias = nextGen1();
//            nn.OutputLayer[j] = buildNeuron1(nn.OutputLayer[j]);
//        }

//        return nn;
//    }

//    private Neuron buildNeuron1(Neuron n)
//    {
//        for (int i = 0; i < n.OutputSynapses.Count; i++)
//        {
//            n.OutputSynapses[i].Weight = nextGen1();
//        }

//        return n;
//    }

//    private double nextGen1()
//    {
//        index1++;
//        return newgens1[index1];
//    }
//}

