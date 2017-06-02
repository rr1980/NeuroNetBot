//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class BotController_2 : MonoBehaviour
//{
//    public enum StateEnum
//    {
//        Stop,
//        Run
//    }

//    [Header("Settings")]
//    public float RefreshTime = 0.5f;
//    public bool debug = true;
//    public StateEnum State = StateEnum.Run;
//    public int BotCount = 1;
//    public int FoodCount = 1;
//    public GameObject Bot;
//    public GameObject Food;

//    [Header("Sim")]
//    public float RoundTime;
//    public float Generation = 0;

//    private float startTime = 0;
//    private List<double> newgens;
//    private int InpuNeuron;
//    private int HiddenNeuron1;
//    //private int HiddenNeuron2;
//    private int OutputNeuron;
//    private int index = -1;
//    private float elapsed;

//    void Start()
//    {
//        for (int i = 0; i < BotCount; i++)
//        {
//            buildGameObject();
//        }

//        var floor = GameObject.FindGameObjectWithTag("Floor");

//        for (int i = 0; i < FoodCount; i++)
//        {
//            GameObject food = Instantiate(Food);
//            food.transform.parent = floor.transform;
//            var x = UnityEngine.Random.Range(-80, 80);
//            var z = UnityEngine.Random.Range(-80, 80);
//            food.transform.position = new Vector3(x, 0.5f, z);
//        }
//    }

//    void Update()
//    {
//        if (pufferTime())
//        {
//            var bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
//            var best = getBest(bots, 2).Select(b => b.gameObject.GetComponent<Renderer>()).ToList();

//            foreach (var item in bots.Select(b => b.gameObject.GetComponent<Renderer>()).Where(r => r.material.color != Color.blue))
//            {
//                item.material.color = Color.white;
//            }

//            foreach (var item in best)
//            {
//                item.material.color = Color.yellow;
//            }
//        }

//        if (startTime == 0)
//        {
//            startTime = Time.realtimeSinceStartup;
//        }

//        if (RoundTime > (Time.realtimeSinceStartup - startTime))
//        {
//        }
//        else
//        {
//            var bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
//            var best = getBest(bots, 2, false).Select(b => b.gameObject.GetComponent<Network_3>().net).ToList();



//            for (int i = 0; i < 10; i++)
//            {

//                NeuralNet newnn1 = crossOver(new List<NeuralNet>() { best[0], best[1] });
//                NeuralNet newnn2 = crossOver(new List<NeuralNet>() { best[1], best[0] });

//                buildGameObject(newnn1, true);
//                buildGameObject(newnn2, true);

//                NeuralNet newnn3 = crossOver(new List<NeuralNet>() { best[0], best[1] });
//                NeuralNet newnn4 = crossOver(new List<NeuralNet>() { best[1], best[0] });

//            }





//            for (int i = 0; i < BotCount - 20; i++)
//            {
//                buildGameObject();
//            }

//            Generation++;

//            destroyBot(bots);

//            startTime = Time.realtimeSinceStartup;
//        }
//    }

//    private void buildGameObject(NeuralNet nn = null, bool mark = false)
//    {


//        GameObject bot = Instantiate(Bot);

//        if (mark)
//        {
//            bot.GetComponent<Renderer>().material.color = Color.blue;
//        }


//        var nw = bot.GetComponent<Network_3>();
//        InpuNeuron = nw.InpuNeuron;
//        HiddenNeuron1 = nw.HiddenNeuron1;
//        //HiddenNeuron2 = nw.HiddenNeuron2;
//        OutputNeuron = nw.OutputNeuron;


//        if (nn == null)
//        {
//            nn = new NeuralNet(InpuNeuron, HiddenNeuron1,  OutputNeuron);
//        }

//        nw.Init(nn);
//        var x = UnityEngine.Random.Range(-45, 45);
//        var z = UnityEngine.Random.Range(-45, 45);
//        bot.transform.position = new Vector3(x, 1, z);
//    }

//    private NeuralNet crossOver(List<NeuralNet> nns)
//    {
//        List<List<double>> gens = new List<List<double>>();
//        for (int i = 0; i < nns.Count; i++)
//        {
//            gens.Add(readNN(nns[i]));
//        }

//        return cross(gens);
//    }

//    private NeuralNet cross(List<List<double>> gens)
//    {
//        newgens = new List<double>();
//        index = -1;
//        for (int i = 0; i < gens[0].Count; i++)
//        {
//            double result = 0;

//            var rnd = UnityEngine.Random.Range(0, (gens.Count) + 3);

//            if (rnd >= gens.Count)
//            {
//                //Debug.Log("GRÖSSER: ");
//                rnd = 0;
//            }
//            //else
//            //{
//            //    //Debug.Log("KLEINER: ");
//            //}

//            //Debug.Log("RND: " + rnd);


//            result = gens[rnd][i];
//            newgens.Add(result);

//        }

//        //Debug.Log("------------------------------");

//        //  mutation
//        for (int u = 0; u < (newgens.Count / 100) * 2; u++)
//        {
//            newgens[UnityEngine.Random.Range(0, newgens.Count)] = newgens[UnityEngine.Random.Range(0, newgens.Count)] * -1;
//        }

//        return buildNN();
//    }

//    private NeuralNet buildNN()
//    {
//        var nn = new NeuralNet(InpuNeuron, HiddenNeuron1,  OutputNeuron);

//        for (int j = 0; j < nn.InputLayer.Count; j++)
//        {
//            nn.InputLayer[j] = buildNeuron(nn.InputLayer[j]);
//        }

//        for (int j = 0; j < nn.HiddenLayer1.Count; j++)
//        {
//            nn.HiddenLayer1[j] = buildNeuron(nn.HiddenLayer1[j]);
//        }

//        //for (int j = 0; j < nn.HiddenLayer2.Count; j++)
//        //{
//        //    nn.HiddenLayer2[j] = buildNeuron(nn.HiddenLayer2[j]);
//        //}

//        for (int j = 0; j < nn.OutputLayer.Count; j++)
//        {
//            nn.OutputLayer[j] = buildNeuron(nn.OutputLayer[j]);
//        }

//        return nn;
//    }

//    private Neuron buildNeuron(Neuron n)
//    {
//        for (int i = 0; i < n.OutputSynapses.Count; i++)
//        {
//            n.OutputSynapses[i].Weight = nextGen();
//        }

//        return n;
//    }

//    private double nextGen()
//    {
//        index++;
//        return newgens[index];
//    }

//    private List<double> readNN(NeuralNet nn)
//    {
//        List<double> gens = new List<double>();

//        for (int j = 0; j < nn.InputLayer.Count; j++)
//        {
//            gens.AddRange(readNeuron(nn.InputLayer[j]));
//        }

//        for (int j = 0; j < nn.HiddenLayer1.Count; j++)
//        {
//            gens.AddRange(readNeuron(nn.HiddenLayer1[j]));
//        }

//        //for (int j = 0; j < nn.HiddenLayer2.Count; j++)
//        //{
//        //    gens.AddRange(readNeuron(nn.HiddenLayer2[j]));
//        //}

//        for (int j = 0; j < nn.OutputLayer.Count; j++)
//        {
//            gens.AddRange(readNeuron(nn.OutputLayer[j]));
//        }

//        return gens;
//    }

//    private List<double> readNeuron(Neuron n)
//    {
//        List<double> gens = new List<double>();

//        for (int i = 0; i < n.OutputSynapses.Count; i++)
//        {
//            var osyn = n.OutputSynapses[i];
//            gens.Add(osyn.Weight);
//        }

//        return gens;
//    }

//    private void destroyBot(List<GameObject> bots)
//    {
//        foreach (var bot in bots)
//        {
//            Destroy(bot);
//        }
//    }

//    private List<GameObject> getBest(List<GameObject> bots, int bestCount, bool debug = false)
//    {
//        var bb = bots.OrderByDescending(b => b.gameObject.GetComponent<Bot_3>().Health).Take(bestCount).ToList();

//        if (debug)
//        {
//            var bmin = bb.First().GetComponent<Bot_3>();
//            var bmax = bb.Last().GetComponent<Bot_3>();

//            Debug.Log("First: " + bmin.Health);
//            Debug.Log("Last: " + bmax.Health);
//            Debug.Log("-----------------");
//        }

//        return bb;
//    }

//    private bool pufferTime()
//    {
//        elapsed += Time.deltaTime;
//        if (elapsed > RefreshTime)
//        {
//            elapsed = 0;
//            return true;
//        }

//        return false;
//    }
//}
