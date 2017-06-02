using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NNAnalyzer
{
    internal static List<NeuralNet> Crossover(NeuralNet parent1, NeuralNet parent2, float crossFactor, float mutateChance, float perbetuation, bool bias, float antiInzest, bool writeCsv = false)
    {
        List<double> p1 = ReadNN(parent1);
        List<double> p2 = ReadNN(parent2);

        bool check = checkCross(p1, p2, antiInzest);
        if (!check)
        {
            Debug.Log("ABORT!");
            return null;
        }
        else
        {
            //Debug.Log("Cross!");
        }

        List<List<double>> childs = sex(p1, p2, crossFactor, mutateChance, perbetuation, parent1.InputLayer.Count, parent1.HiddenLayer1.Count, parent1.HiddenLayer2 != null ? parent1.HiddenLayer2.Count : 0, parent1.HiddenLayer3 != null ? parent1.HiddenLayer3.Count : 0, parent1.OutputLayer.Count, writeCsv);

        //bool check2 = checkCross(childs[0], childs[1], antiInzest);

        var c1 = BuildNN(childs[0], bias, parent1.InputLayer.Count, parent1.HiddenLayer1.Count, parent1.HiddenLayer2 != null ? parent1.HiddenLayer2.Count : 0, parent1.HiddenLayer3 != null ? parent1.HiddenLayer3.Count : 0, parent1.OutputLayer.Count);
        var c2 = BuildNN(childs[1], bias, parent1.InputLayer.Count, parent1.HiddenLayer1.Count, parent1.HiddenLayer2 != null ? parent1.HiddenLayer2.Count : 0, parent1.HiddenLayer3 != null ? parent1.HiddenLayer3.Count : 0, parent1.OutputLayer.Count);

        return new List<NeuralNet>() { c1, c2 };
    }

    private static bool checkCross(List<double> p1, List<double> p2, float v)
    {
        float count = 0;

        for (int i = 0; i < p1.Count; i++)
        {
            if (p1[i] == p2[i])
            {
                count++;
            }
        }

        float res = (count / (float)p1.Count) * 100;
        //Debug.Log("ABORT: "+res);
        return res < v;
    }

    public static NeuralNet BuildNN(List<double> nnn, bool bias, int inpuNeuron, int hiddenNeuron1, int hiddenNeuron2, int hiddenNeuron3, int outputNeuron)
    {
        var nn = new NeuralNet(bias, inpuNeuron, hiddenNeuron1, hiddenNeuron2, hiddenNeuron3, outputNeuron);
        int index = 0;

        for (int j = 0; j < nn.InputLayer.Count; j++)
        {
            nn.InputLayer[j].Bias = nnn[index];
            index++;

            for (int i = 0; i < nn.InputLayer[j].OutputSynapses.Count; i++)
            {
                nn.InputLayer[j].OutputSynapses[i].Weight = nnn[index];
                index++;
            }

            //var nres = buildNeuron(nn.InputLayer[j], nnn, index);
            //index = nres.index;
            //nn.InputLayer[j] = nres.n;
        }

        for (int j = 0; j < nn.HiddenLayer1.Count; j++)
        {
            nn.HiddenLayer1[j].Bias = nnn[index];
            index++;

            for (int i = 0; i < nn.HiddenLayer1[j].OutputSynapses.Count; i++)
            {
                nn.HiddenLayer1[j].OutputSynapses[i].Weight = nnn[index];
                index++;
            }

            //var nres = buildNeuron(nn.HiddenLayer1[j], nnn, index);
            //index = nres.index;
            //nn.HiddenLayer1[j] = nres.n;
        }

        if (nn.HiddenLayer2 != null)
        {
            for (int j = 0; j < nn.HiddenLayer2.Count; j++)
            {
                nn.HiddenLayer2[j].Bias = nnn[index];
                index++;

                for (int i = 0; i < nn.HiddenLayer2[j].OutputSynapses.Count; i++)
                {
                    nn.HiddenLayer2[j].OutputSynapses[i].Weight = nnn[index];
                    index++;
                }

                //var nres = buildNeuron(nn.HiddenLayer2[j], nnn, index);
                //index = nres.index;
                //nn.HiddenLayer2[j] = nres.n;
            }
        }

        if (nn.HiddenLayer3 != null)
        {
            for (int j = 0; j < nn.HiddenLayer3.Count; j++)
            {
                nn.HiddenLayer3[j].Bias = nnn[index];
                index++;

                for (int i = 0; i < nn.HiddenLayer3[j].OutputSynapses.Count; i++)
                {
                    nn.HiddenLayer3[j].OutputSynapses[i].Weight = nnn[index];
                    index++;
                }

                //var nres = buildNeuron(nn.HiddenLayer2[j], nnn, index);
                //index = nres.index;
                //nn.HiddenLayer2[j] = nres.n;
            }
        }

        for (int j = 0; j < nn.OutputLayer.Count; j++)
        {
            nn.OutputLayer[j].Bias = nnn[index];
            index++;

            for (int i = 0; i < nn.OutputLayer[j].OutputSynapses.Count; i++)
            {
                nn.OutputLayer[j].OutputSynapses[i].Weight = nnn[index];
                index++;
            }

            //var nres = buildNeuron(nn.OutputLayer[j], nnn, index);
            //index = nres.index;
            //nn.OutputLayer[j] = nres.n;
        }
        return nn;
    }

    private class buildNeuronResult
    {
        public Neuron n;
        public int index;

        public buildNeuronResult(Neuron n, int index)
        {
            this.n = n;
            this.index = index;
        }
    }

    private static buildNeuronResult buildNeuron(Neuron n, List<double> nnn, int index)
    {
        for (int i = 0; i < n.OutputSynapses.Count; i++)
        {
            n.OutputSynapses[i].Weight = nnn[index];
            index++;
        }

        return new buildNeuronResult(n, index);
    }

    private static List<List<double>> sex(List<double> p1, List<double> p2, float crossFactor, float mutateChance, float perbetuation, int inpuNeuron, int hiddenNeuron1, int hiddenNeuron2, int hiddenNeuron3, int outputNeuron, bool writeCsv)
    {
        List<double> c1 = new List<double>();
        List<double> c2 = new List<double>();


        List<int> crosses = new List<int>();


        for (int i = 0; i < crossFactor + 1; i++)
        {
            crosses.Add(UnityEngine.Random.Range(1, p1.Count - 1));
        }

        crosses = crosses.OrderBy(c => c).ToList();
        int a = 0;
        var checker = false;
        for (int i = 0; i < crosses.Count; i++)
        {
            int c = 0;
            int b;
            if (i + 1 > crosses.Count - 1)
            {
                b = p1.Count - 1;
            }
            else
            {
                b = crosses[i];
                c = crosses[i];
            }

            if (checker)
            {
                var lists = mix(c1, c2, p2, p1, a, b);
                c1 = lists[0];
                c2 = lists[1];
            }
            else
            {
                var lists = mix(c1, c2, p1, p2, a, b);
                c1 = lists[0];
                c2 = lists[1];
            }

            a = c + 1;

            checker = !checker;
        }

        c1 = mutation(c1, mutateChance, perbetuation);
        c2 = mutation(c2, mutateChance, perbetuation);

        if (writeCsv)
        {
            writeDnasToCsv(p1, p2, c1, c2, inpuNeuron, hiddenNeuron1, hiddenNeuron2, hiddenNeuron3, outputNeuron);
        }

        var results = new List<List<double>>();
        results.Add(c1);
        results.Add(c2);

        return results;
    }

    private static void writeDnasToCsv(List<double> p1, List<double> p2, List<double> c1, List<double> c2, int inpuNeuron, int hiddenNeuron1, int hiddenNeuron2, int hiddenNeuron3, int outputNeuron)
    {
        var csv = new Dictionary<string, List<double>>();
        csv.Add("parent_1", p1);
        csv.Add("parent_2", p2);
        csv.Add("child_1", c1);
        csv.Add("child_2", c2);

        buildCrossDnaCsv(csv, inpuNeuron, hiddenNeuron1, hiddenNeuron2, hiddenNeuron3, outputNeuron);
    }

    private static void buildCrossDnaCsv(Dictionary<string, List<double>> csv, int inpuNeuron, int hiddenNeuron1, int hiddenNeuron2, int hiddenNeuron3, int outputNeuron)
    {
        var str = String.Empty;
        for (int i = 0; i < csv["parent_1"].Count; i++)
        {
            str += csv["parent_1"][i].ToString() + "," + csv["parent_2"][i].ToString() + "," + csv["child_1"][i].ToString() + "," + csv["child_2"][i].ToString() + Environment.NewLine;
        }

        System.IO.File.WriteAllText("CrossDna_" + inpuNeuron + "_" + hiddenNeuron1 + "_" + hiddenNeuron2 + "_" + hiddenNeuron3 + "_" + outputNeuron + ".csv", str);
    }

    private static List<List<double>> mix(List<double> c1, List<double> c2, List<double> p1, List<double> p2, int a, int b)
    {
        for (int i = a; i <= b; ++i)
        {
            c1.Add(p1[i]);
            c2.Add(p2[i]);
        }

        return new List<List<double>>() { c1, c2 };
    }

    internal static List<double> ReadNN(NeuralNet nn)
    {
        List<double> gens = new List<double>();

        for (int j = 0; j < nn.InputLayer.Count; j++)
        {
            gens.Add(nn.InputLayer[j].Bias);
            gens.AddRange(readNeuron(nn.InputLayer[j]));
        }

        for (int j = 0; j < nn.HiddenLayer1.Count; j++)
        {
            gens.Add(nn.HiddenLayer1[j].Bias);
            gens.AddRange(readNeuron(nn.HiddenLayer1[j]));
        }

        if (nn.HiddenLayer2 != null)
        {
            for (int j = 0; j < nn.HiddenLayer2.Count; j++)
            {
                gens.Add(nn.HiddenLayer2[j].Bias);
                gens.AddRange(readNeuron(nn.HiddenLayer2[j]));
            }
        }

        if (nn.HiddenLayer3 != null)
        {
            for (int j = 0; j < nn.HiddenLayer3.Count; j++)
            {
                gens.Add(nn.HiddenLayer3[j].Bias);
                gens.AddRange(readNeuron(nn.HiddenLayer3[j]));
            }
        }

        for (int j = 0; j < nn.OutputLayer.Count; j++)
        {
            gens.Add(nn.OutputLayer[j].Bias);
            gens.AddRange(readNeuron(nn.OutputLayer[j]));
        }
        return gens;
    }

    private static List<double> readNeuron(Neuron n)
    {
        List<double> gens = new List<double>();

        for (int i = 0; i < n.OutputSynapses.Count; i++)
        {
            var osyn = n.OutputSynapses[i];
            gens.Add(osyn.Weight);
        }

        return gens;
    }

    //private static System.Random rnd = new System.Random();

    private static List<double> mutation(List<double> n, float mutateChance, float perbetuation)
    {
        for (int i = 0; i < n.Count; i++)
        {
            if (UnityEngine.Random.Range(0f,1f) < mutateChance)
            {
                var t = UnityEngine.Random.Range(-1f, 1f) * perbetuation;
                //var t = Math.Min(1f, Math.Max(21.5f, -1f));
                //n[i] += t;
                n[i] = Math.Min(0.99f, Math.Max(n[i]+t, -0.99f));
            }
        }

        //float ppp = (float)n.Count / 100;
        //float pp = ppp * mutateChance;
        //var p = Mathf.CeilToInt(pp);
        ////var p = Mathf.CeilToInt((float)((n.Count / 100) * mutateChance));

        ////    Debug.Log("p  : " + p);

        //for (int u = 0; u < p; u++)
        //{
        //    var m = UnityEngine.Random.Range(0, n.Count);

        //    //Debug.Log("m  : " + m);
        //    //Debug.Log("m-r: " + n[m] * -1);

        //    //n[m] = (n[m] / mutateDividor) * -1;

        //    //float value = UnityEngine.Random.Range(0.0f, 1.0f); // value between 0.6 and 2.0
        //    //float sign = UnityEngine.Random.value < 0.5f ? -1f : 1f; // select a negative or positive value

        //    ////return sign * value;

        //    //Debug.Log("Old1: " + UnityEngine.Random.Range(-1, 1));
        //    //Debug.Log("Old2: " + UnityEngine.Random.Range(-1f,1f));
        //    //Debug.Log("NEW: "+ sign * value);

        //    n[m] = UnityEngine.Random.Range(-1f, 1f);
        //}

        return n;
    }

}


public static class NNAnalyzer_6
{

    internal static List<NeuralNet> Crossover(NeuralNet net1, NeuralNet net2, float crossFactor, float mutateChance, float mutateDividor, bool bias, float antiInzest, bool saveCrossDna)
    {
        NeuralNet n = net1.InputLayer.Count > net2.InputLayer.Count ? net2 : net1;
        int maxCount = ReadNN(n).Count;
        var mutC = Mathf.Ceil(((float)maxCount / 100) * mutateChance);
        NeuralNet c1 = new NeuralNet(net1.Bias, n.InputLayer.Count, net1.HiddenLayer1.Count, net1.HiddenLayer2 != null ? net1.HiddenLayer2.Count : 0,0, net1.OutputLayer.Count);
        NeuralNet c2 = new NeuralNet(net1.Bias, n.InputLayer.Count, net1.HiddenLayer1.Count, net1.HiddenLayer2 != null ? net1.HiddenLayer2.Count : 0,0, net1.OutputLayer.Count);
        List<int> crosses = getRndCrosses(crossFactor, maxCount);
        List<int> mutates = getRndMutate(mutC, maxCount);
        var crossIndex = 0;
        var muateIndex = 0;
        var pos = 0;
        var cross = false;

        for (int j = 0; j < n.InputLayer.Count; j++)
        {
            //-----------------------------------

            if (crosses[crossIndex] == pos)
            {
                crossIndex += crossIndex < crosses.Count - 1 ? 1 : 0;
                cross = !cross;
            }

            if (cross)
            {
                c1.InputLayer[j].Bias = net1.InputLayer[j].Bias;
                c2.InputLayer[j].Bias = net2.InputLayer[j].Bias;
            }
            else
            {
                c1.InputLayer[j].Bias = net2.InputLayer[j].Bias;
                c2.InputLayer[j].Bias = net1.InputLayer[j].Bias;
            }

            if (mutates[muateIndex] == pos)
            {
                c1.InputLayer[j].Bias = (c1.InputLayer[j].Bias / mutateDividor) * -1;
                c2.InputLayer[j].Bias = (c2.InputLayer[j].Bias / mutateDividor) * -1;
                muateIndex += muateIndex < mutates.Count - 1 ? 1 : 0;
            }
            pos++;


            for (int i = 0; i < n.InputLayer[j].OutputSynapses.Count; i++)
            {
                if (crosses[crossIndex] == pos)
                {
                    crossIndex += crossIndex < crosses.Count - 1 ? 1 : 0;
                    cross = !cross;
                }

                if (cross)
                {
                    c1.InputLayer[j].OutputSynapses[i].Weight = net1.InputLayer[j].OutputSynapses[i].Weight;
                    c2.InputLayer[j].OutputSynapses[i].Weight = net2.InputLayer[j].OutputSynapses[i].Weight;
                    crossIndex += crossIndex < crosses.Count - 1 ? 1 : 0;
                }
                else
                {
                    c1.InputLayer[j].OutputSynapses[i].Weight = net2.InputLayer[j].OutputSynapses[i].Weight;
                    c2.InputLayer[j].OutputSynapses[i].Weight = net1.InputLayer[j].OutputSynapses[i].Weight;
                }

                if (mutates[muateIndex] == pos)
                {
                    c1.InputLayer[j].OutputSynapses[i].Weight = (c1.InputLayer[j].OutputSynapses[i].Weight / mutateDividor) * -1;
                    c2.InputLayer[j].OutputSynapses[i].Weight = (c2.InputLayer[j].OutputSynapses[i].Weight / mutateDividor) * -1;
                    muateIndex += muateIndex < mutates.Count - 1 ? 1 : 0;
                }
                pos++;
            }
        }

        //--------------------------------
        for (int j = 0; j < n.HiddenLayer1.Count; j++)
        {
            if (crosses[crossIndex] == pos)
            {
                crossIndex += crossIndex < crosses.Count - 1 ? 1 : 0;
                cross = !cross;
            }

            if (cross)
            {
                c1.HiddenLayer1[j].Bias = net1.HiddenLayer1[j].Bias;
                c2.HiddenLayer1[j].Bias = net2.HiddenLayer1[j].Bias;
            }
            else
            {
                c1.HiddenLayer1[j].Bias = net2.HiddenLayer1[j].Bias;
                c2.HiddenLayer1[j].Bias = net1.HiddenLayer1[j].Bias;
            }

            if (mutates[muateIndex] == pos)
            {
                c1.HiddenLayer1[j].Bias = (c1.HiddenLayer1[j].Bias / mutateDividor) * -1;
                c2.HiddenLayer1[j].Bias = (c2.HiddenLayer1[j].Bias / mutateDividor) * -1;
                muateIndex += muateIndex < mutates.Count - 1 ? 1 : 0;
            }
            pos++;

            for (int i = 0; i < n.HiddenLayer1[j].OutputSynapses.Count; i++)
            {
                if (crosses[crossIndex] == pos)
                {
                    crossIndex += crossIndex < crosses.Count - 1 ? 1 : 0;
                    cross = !cross;
                }

                if (cross)
                {
                    c1.HiddenLayer1[j].OutputSynapses[i].Weight = net1.HiddenLayer1[j].OutputSynapses[i].Weight;
                    c2.HiddenLayer1[j].OutputSynapses[i].Weight = net2.HiddenLayer1[j].OutputSynapses[i].Weight;
                }
                else
                {
                    c1.HiddenLayer1[j].OutputSynapses[i].Weight = net2.HiddenLayer1[j].OutputSynapses[i].Weight;
                    c2.HiddenLayer1[j].OutputSynapses[i].Weight = net1.HiddenLayer1[j].OutputSynapses[i].Weight;
                }

                if (mutates[muateIndex] == pos)
                {
                    c1.HiddenLayer1[j].OutputSynapses[i].Weight = (c1.HiddenLayer1[j].OutputSynapses[i].Weight / mutateDividor) * -1;
                    c2.HiddenLayer1[j].OutputSynapses[i].Weight = (c2.HiddenLayer1[j].OutputSynapses[i].Weight / mutateDividor) * -1;
                    muateIndex += muateIndex < mutates.Count - 1 ? 1 : 0;
                }
                pos++;
            }
        }

        //--------------------------------

        if (n.HiddenLayer2 != null)
        {
            Debug.Log("HÄÄÄÄ!?");
            for (int j = 0; j < n.HiddenLayer2.Count; j++)
            {
                if (crosses[crossIndex] == pos)
                {
                    crossIndex += crossIndex < crosses.Count - 1 ? 1 : 0;
                    cross = !cross;
                }

                if (cross)
                {
                    c1.HiddenLayer2[j].Bias = net1.HiddenLayer2[j].Bias;
                    c2.HiddenLayer2[j].Bias = net2.HiddenLayer2[j].Bias;
                }
                else
                {
                    c1.HiddenLayer2[j].Bias = net2.HiddenLayer2[j].Bias;
                    c2.HiddenLayer2[j].Bias = net1.HiddenLayer2[j].Bias;
                }

                if (mutates[muateIndex] == pos)
                {
                    c1.HiddenLayer2[j].Bias = (c1.HiddenLayer2[j].Bias / mutateDividor) * -1;
                    c2.HiddenLayer2[j].Bias = (c2.HiddenLayer2[j].Bias / mutateDividor) * -1;
                    muateIndex += muateIndex < mutates.Count - 1 ? 1 : 0;
                }
                pos++;

                for (int i = 0; i < n.HiddenLayer2[j].OutputSynapses.Count; i++)
                {
                    if (crosses[crossIndex] == pos)
                    {
                        crossIndex += crossIndex < crosses.Count - 1 ? 1 : 0;
                        cross = !cross;
                    }

                    if (cross)
                    {
                        c1.HiddenLayer2[j].OutputSynapses[i].Weight = net1.HiddenLayer2[j].OutputSynapses[i].Weight;
                        c2.HiddenLayer2[j].OutputSynapses[i].Weight = net2.HiddenLayer2[j].OutputSynapses[i].Weight;
                    }
                    else
                    {
                        c1.HiddenLayer2[j].OutputSynapses[i].Weight = net2.HiddenLayer2[j].OutputSynapses[i].Weight;
                        c2.HiddenLayer2[j].OutputSynapses[i].Weight = net1.HiddenLayer2[j].OutputSynapses[i].Weight;
                    }

                    if (mutates[muateIndex] == pos)
                    {
                        c1.HiddenLayer2[j].OutputSynapses[i].Weight = (c1.HiddenLayer2[j].OutputSynapses[i].Weight / mutateDividor) * -1;
                        c2.HiddenLayer2[j].OutputSynapses[i].Weight = (c2.HiddenLayer2[j].OutputSynapses[i].Weight / mutateDividor) * -1;
                        muateIndex += muateIndex < mutates.Count - 1 ? 1 : 0;
                    }
                    pos++;
                }
            }
        }

        //--------------------------------

        for (int j = 0; j < n.OutputLayer.Count; j++)
        {
            if (crosses[crossIndex] == pos)
            {
                crossIndex += crossIndex < crosses.Count - 1 ? 1 : 0;
                cross = !cross;
            }

            if (cross)
            {
                c1.OutputLayer[j].Bias = net1.OutputLayer[j].Bias;
                c2.OutputLayer[j].Bias = net2.OutputLayer[j].Bias;
            }
            else
            {
                c1.OutputLayer[j].Bias = net2.OutputLayer[j].Bias;
                c2.OutputLayer[j].Bias = net1.OutputLayer[j].Bias;
            }

            if (mutates[muateIndex] == pos)
            {
                c1.OutputLayer[j].Bias = (c1.OutputLayer[j].Bias / mutateDividor) * -1;
                c2.OutputLayer[j].Bias = (c2.OutputLayer[j].Bias / mutateDividor) * -1;
                muateIndex += muateIndex < mutates.Count - 1 ? 1 : 0;
            }
            pos++;

            for (int i = 0; i < n.OutputLayer[j].OutputSynapses.Count; i++)
            {
                if (crosses[crossIndex] == pos)
                {
                    crossIndex += crossIndex < crosses.Count - 1 ? 1 : 0;
                    cross = !cross;
                }

                if (cross)
                {
                    c1.OutputLayer[j].OutputSynapses[i].Weight = net1.OutputLayer[j].OutputSynapses[i].Weight;
                    c2.OutputLayer[j].OutputSynapses[i].Weight = net2.OutputLayer[j].OutputSynapses[i].Weight;
                }
                else
                {
                    c1.OutputLayer[j].OutputSynapses[i].Weight = net2.OutputLayer[j].OutputSynapses[i].Weight;
                    c2.OutputLayer[j].OutputSynapses[i].Weight = net1.OutputLayer[j].OutputSynapses[i].Weight;
                }

                if (mutates[muateIndex] == pos)
                {
                    c1.OutputLayer[j].OutputSynapses[i].Weight = (c1.OutputLayer[j].OutputSynapses[i].Weight / mutateDividor) * -1;
                    c2.OutputLayer[j].OutputSynapses[i].Weight = (c2.OutputLayer[j].OutputSynapses[i].Weight / mutateDividor) * -1;
                    muateIndex += muateIndex < mutates.Count - 1 ? 1 : 0;
                }
                pos++;
            }
        }


        if (saveCrossDna)
        {
            writeDnasToCsv(net1, net2, c1, c2);
        }

        return new List<NeuralNet>() { c1, c2 };
    }


    private static void writeDnasToCsv(NeuralNet p1, NeuralNet p2, NeuralNet c1, NeuralNet c2)
    {
        var csv = new Dictionary<string, List<double>>();
        csv.Add("parent_1", ReadNN(p1));
        csv.Add("parent_2", ReadNN(p2));
        csv.Add("child_1", ReadNN(c1));
        csv.Add("child_2", ReadNN(c2));

        buildCrossDnaCsv(csv);
    }

    private static void buildCrossDnaCsv(Dictionary<string, List<double>> csv)
    {
        var str = String.Empty;

        for (int i = 0; i < csv["parent_1"].Count-1; i++)
        {
            var p1 = "";
            var p2 = "";

            if (i < csv["parent_1"].Count)
            {
                p1 = csv["parent_1"][i].ToString();
            }

            if (i < csv["parent_2"].Count)
            {
                p2 = csv["parent_2"][i].ToString();
            }

            str += p1 + "," + p2 + "," + csv["child_1"][i].ToString() + "," + csv["child_2"][i].ToString() + Environment.NewLine;
        }

        System.IO.File.WriteAllText("CrossDna.csv", str);
    }


    private static List<int> getRndMutate(float matatefactor, int count)
    {
        List<int> mutates = new List<int>();
        for (int i = 0; i < matatefactor; i++)
        {
            mutates.Add(UnityEngine.Random.Range(1, count - 1));
        }
        mutates = mutates.OrderBy(c => c).ToList();
        return mutates;
    }

    private static List<int> getRndCrosses(float crossFactor, int count)
    {
        List<int> crosses = new List<int>();
        for (int i = 0; i < crossFactor + 1; i++)
        {
            crosses.Add(UnityEngine.Random.Range(1, count - 1));
        }
        crosses = crosses.OrderBy(c => c).ToList();
        return crosses;
    }

    internal static NeuralNet CopyNNInNN(NeuralNet oldNet, int inpuNeuron, int hiddenNeuron1, int hiddenNeuron2, int outputNeuron)
    {
        var newn = new NeuralNet(oldNet.Bias, inpuNeuron, hiddenNeuron1, hiddenNeuron2,0, outputNeuron);

        for (int j = 0; j < oldNet.InputLayer.Count; j++)
        {
            newn.InputLayer[j].Bias = oldNet.InputLayer[j].Bias;

            for (int i = 0; i < oldNet.InputLayer[j].OutputSynapses.Count; i++)
            {
                newn.InputLayer[j].OutputSynapses[i].Weight = oldNet.InputLayer[j].OutputSynapses[i].Weight;
            }
        }

        for (int j = 0; j < oldNet.HiddenLayer1.Count; j++)
        {
            newn.HiddenLayer1[j].Bias = oldNet.HiddenLayer1[j].Bias;

            for (int i = 0; i < oldNet.HiddenLayer1[j].OutputSynapses.Count; i++)
            {
                newn.HiddenLayer1[j].OutputSynapses[i].Weight = oldNet.HiddenLayer1[j].OutputSynapses[i].Weight;
            }
        }

        if (oldNet.HiddenLayer2 != null)
        {
            for (int j = 0; j < oldNet.HiddenLayer2.Count; j++)
            {
                newn.HiddenLayer2[j].Bias = oldNet.HiddenLayer2[j].Bias;

                for (int i = 0; i < oldNet.HiddenLayer2[j].OutputSynapses.Count; i++)
                {
                    newn.HiddenLayer2[j].OutputSynapses[i].Weight = oldNet.HiddenLayer2[j].OutputSynapses[i].Weight;
                }
            }
        }

        for (int j = 0; j < oldNet.OutputLayer.Count; j++)
        {
            newn.OutputLayer[j].Bias = oldNet.OutputLayer[j].Bias;

            for (int i = 0; i < oldNet.OutputLayer[j].OutputSynapses.Count; i++)
            {
                newn.OutputLayer[j].OutputSynapses[i].Weight = oldNet.OutputLayer[j].OutputSynapses[i].Weight;
            }
        }

        return newn;
    }

    internal static List<double> ReadNN(NeuralNet nn)
    {
        List<double> gens = new List<double>();

        for (int j = 0; j < nn.InputLayer.Count; j++)
        {
            gens.Add(nn.InputLayer[j].Bias);
            gens.AddRange(readNeuron(nn.InputLayer[j]));
        }

        for (int j = 0; j < nn.HiddenLayer1.Count; j++)
        {
            gens.Add(nn.HiddenLayer1[j].Bias);
            gens.AddRange(readNeuron(nn.HiddenLayer1[j]));
        }

        if (nn.HiddenLayer2 != null)
        {
            for (int j = 0; j < nn.HiddenLayer2.Count; j++)
            {
                gens.Add(nn.HiddenLayer2[j].Bias);
                gens.AddRange(readNeuron(nn.HiddenLayer2[j]));
            }
        }

        for (int j = 0; j < nn.OutputLayer.Count; j++)
        {
            gens.Add(nn.OutputLayer[j].Bias);
            gens.AddRange(readNeuron(nn.OutputLayer[j]));
        }
        return gens;
    }


    private static List<double> readNeuron(Neuron n)
    {
        List<double> gens = new List<double>();

        for (int i = 0; i < n.OutputSynapses.Count; i++)
        {
            var osyn = n.OutputSynapses[i];
            gens.Add(osyn.Weight);
        }

        return gens;
    }

}