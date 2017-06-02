//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class BotController_4 : MonoBehaviour
//{
//    public enum ControllerState
//    {
//        Run,
//        Stop,
//        Standbye

//    }

//    public string buildDna;
//    public string bestDna;
    
//    [Header("Settings")]
//    public float RefreshTime = 0.5f;
//    public int debug = 0;
//    public ControllerState State = ControllerState.Run;
//    [Space(5)]
//    public GameObject Bot;
//    public GameObject Food;

//    [Header("Sim")]
//    public float RoundTime;
//    public float TimeRemaining;
//    [Space(5)]
//    public float Generation = 0;
//    public float BiggestHealth = 0;
//    [Space(5)]
//    public bool SaveCrossDnaCsv = false;
//    [Header("SimSettings")]
//    public int MaxBotCount = 20;
//    [Space(5)]
//    public int NewBotCount = 3;
//    public Vector2 BotSpawnRange = new Vector2(-150, 150);
//    [Space(5)]
//    public int FoodCount = 1;
//    public Vector2 FoodSpawnRange = new Vector2(-150, 150);
//    [Space(5)]
//    public float mutateChance = 3;
//    public float mutateDividor = 2;
//    public float crossFactor = 1;
//    [Space(5)]
//    public float foodGod = 10000;
//    public float wallPain = 1000;
//    public float botPain = 100;
//    public float permaPain = 10;

//    private float startTime = 0;
//    private List<double> newgens1;
//    private List<double> newgens2;
//    [HideInInspector]
//    public int InpuNeuron;
//    [HideInInspector]
//    public int HiddenNeuron1;
//    [HideInInspector]
//    public int HiddenNeuron2;
//    [HideInInspector]
//    public int OutputNeuron;
//    private int index1 = -1;
//    private int index2 = -1;
//    private float elapsed;
//    private GameObject floor;
//    private int old_foodCount;
//    private Vector2 old_foodRange;
//    private bool init = false;
//    private int csvCounter = 0;

//    void Start()
//    {
//        Init();
//    }

//    private void Init()
//    {
//        floor = GameObject.FindGameObjectWithTag("Floor");

//        for (int i = 0; i < MaxBotCount; i++)
//        {
//            buildGameObject();
//        }

//        setFood();

//        init = true;
//    }

//    private void setFood()
//    {
//        old_foodRange = FoodSpawnRange;
//        old_foodCount = FoodCount;
//        for (int i = 0; i < FoodCount; i++)
//        {
//            GameObject food = Instantiate(Food);
//            food.transform.parent = floor.transform;
//            var x = UnityEngine.Random.Range(FoodSpawnRange.x, FoodSpawnRange.y);
//            var z = UnityEngine.Random.Range(FoodSpawnRange.x, FoodSpawnRange.y);
//            food.transform.position = new Vector3(x, 0.5f, z);
//        }
//    }

//    void Update()
//    {
//        if (State == ControllerState.Stop || State == ControllerState.Standbye)
//        {
//            return;
//        }

//        if (!init)
//        {
//            Init();
//        }


//        if (pufferTime())
//        {
//            var bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
//            var best = getBest(bots, 2).Select(b => b.gameObject.GetComponent<Renderer>()).ToList();

//            foreach (var item in bots.Select(b => b.gameObject.GetComponent<Renderer>()).Where(r => r.material.color != Color.blue && r.material.color != Color.red && r.material.color != Color.black))
//            {
//                item.material.color = Color.white;
//            }

//            foreach (var item in best.Select(b => b.gameObject.GetComponent<Renderer>()).Where(r => r.material.color != Color.black))
//            {
//                item.material.color = Color.yellow;
//            }
//        }

//        if (startTime == 0)
//        {
//            startTime = Time.realtimeSinceStartup;
//        }

//        if (RoundTime < (Time.realtimeSinceStartup - startTime))
//        {
//            var count = 0;
//            if (debug > 0)
//            {
//                Debug.Log("--------------------------------------------------------------");
//            }

//            if (old_foodCount != FoodCount || old_foodRange != FoodSpawnRange)
//            {
//                var foods = GameObject.FindGameObjectsWithTag("Food").ToList();
//                destroyBot(foods);
//                setFood();
//            }


//            var bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
//            var _best = getBest(bots, 4, true);
//            var best = _best.Select(b => b.gameObject.GetComponent<Bot_4>().net).ToList();


//            if (String.IsNullOrEmpty(bestDna) || BiggestHealth < _best[0].GetComponent<Bot_4>().Health)
//            {
//                BiggestHealth = _best[0].GetComponent<Bot_4>().Health;
//                buildCsv(best[0]);
//            }

//            //buildGameObject(best[0], 2);
//            //count++;
//            //buildGameObject(best[1], 2);
//            //count++;

//            //Debug.Log("calc: "+ Mathf.Floor((float)(MaxBotCount - count) / 4));

//            var cc = count;

//            //for (int i = 0; i < Mathf.Floor((float)(MaxBotCount - cc) / 2); i++)
//            //{

//            //Debug.Log("Count A: " + count);
//            List<NeuralNet> newnn1 = crossOver(new List<NeuralNet>() { best[0], best[1] });
//            List<NeuralNet> newnn2 = crossOver(new List<NeuralNet>() { best[0], best[2] });
//            List<NeuralNet> newnn3 = crossOver(new List<NeuralNet>() { best[0], best[3] });
//            List<NeuralNet> newnn4 = crossOver(new List<NeuralNet>() { best[1], best[2] });

//            foreach (var item in newnn1)
//            {
//                buildGameObject(item, 1);
//                count++;
//            }

//            foreach (var item in newnn2)
//            {
//                buildGameObject(item, 1);
//                count++;
//            }

//            foreach (var item in newnn3)
//            {
//                buildGameObject(item, 1);
//                count++;
//            }

//            foreach (var item in newnn4)
//            {
//                buildGameObject(item, 1);
//                count++;
//            }

//            //}

//            //Debug.Log("Count: "+count);

//            for (int i = 0; i < MaxBotCount - count; i++)
//            {
//                buildGameObject();
//            }

//            Generation++;

//            destroyBot(bots);

//            startTime = Time.realtimeSinceStartup;
//        }

//        TimeRemaining = (float)Math.Round(Time.realtimeSinceStartup - startTime, 2);
//    }

//    private void buildCsv(NeuralNet nn)
//    {
//        List<double> gens = readNN(nn);


//        var dna = String.Join(",", gens.Select(x => x.ToString()).ToArray());
//        bestDna = dna;
//        //System.IO.File.WriteAllText("CSVData.csv", dna);
//    }

//    public void buildGameObject(NeuralNet nn = null, int mark = 0)
//    {


//        GameObject bot = Instantiate(Bot);

//        if (mark==1)
//        {
//            bot.GetComponent<Renderer>().material.color = Color.blue;
//        }
//        else if (mark == 2)
//        {
//            bot.GetComponent<Renderer>().material.color = Color.red;
//        }
//        else if (mark == 3)
//        {
//            bot.GetComponent<Renderer>().material.color = Color.black;
//        }

//        Bot_4 bb = bot.GetComponent<Bot_4>();
//        InpuNeuron = bb.InpuNeuron;
//        HiddenNeuron1 = bb.HiddenNeuron1;
//        HiddenNeuron2 = bb.HiddenNeuron2;
//        OutputNeuron = bb.OutputNeuron;


//        if (nn == null)
//        {
//            nn = new NeuralNet(InpuNeuron, HiddenNeuron1, HiddenNeuron2, OutputNeuron);
//        }

//        if (!bb.init)
//        {
//            bb.Init(nn);
//        }

//        var x = UnityEngine.Random.Range(BotSpawnRange.x, BotSpawnRange.y);
//        var z = UnityEngine.Random.Range(BotSpawnRange.x, BotSpawnRange.y);
//        bot.transform.position = new Vector3(x, 1, z);
//    }

//    private List<NeuralNet> crossOver(List<NeuralNet> nns)
//    {
//        List<List<double>> gens = new List<List<double>>();
//        for (int i = 0; i < nns.Count; i++)
//        {
//            gens.Add(readNN(nns[i]));
//        }

//        return cross(gens);
//    }

//    private void mix(List<double> gensA, List<double> gensB, int a, int b)
//    {
//        for (int i = a; i <= b; ++i)
//        {
//            newgens1.Add(gensA[i]);
//            newgens2.Add(gensB[i]);
//        }
//    }

//    private List<NeuralNet> cross(List<List<double>> gens)
//    {
//        newgens1 = new List<double>();
//        newgens2 = new List<double>();
//        index1 = -1;
//        index2 = -1;

//        List<int> crosses = new List<int>();
//        //crosses.Add(0);

//        for (int i = 0; i < crossFactor; i++)
//        {
//            crosses.Add(UnityEngine.Random.Range(1, gens[0].Count - 1));
//        }

//        crosses = crosses.OrderBy(c => c).ToList();
//        int a = 0;
//        var checker = false;
//        for (int i = 0; i < crosses.Count; i++)
//        {
//            //if (i > 0)
//            //{
//            //    a = crosses[i];
//            //}
//            int c=0;
//            int b;
//            if (i + 1 > crosses.Count-1)
//            {
//                b = gens[0].Count-1;
//            }
//            else
//            {
//                b = crosses[i + 1];
//                c = crosses[i + 1];
//            }

//            if (checker)
//            {
//                mix(gens[1], gens[0], a, b);
//            }
//            else
//            {
//                mix(gens[0], gens[1], a, b);
//            }

//            a = c+1;

//            checker = !checker;
//        }

//        //for (int i = 0; i <= cross; ++i)
//        //{
//        //    newgens1.Add(gens[0][i]);
//        //    newgens2.Add(gens[1][i]);
//        //}

//        //for (int i = cross + 1; i < gens[0].Count; ++i)
//        //{
//        //    newgens1.Add(gens[1][i]);
//        //    newgens2.Add(gens[0][i]);
//        //}





//        //for (int i = 0; i <= cross; ++i)
//        //{
//        //    newgens2.Add(gens[1][i]);
//        //}
//        //for (int i = cross + 1; i < gens[0].Count; ++i)
//        //{
//        //    newgens1.Add(gens[1][i]);
//        //}

//        //for (int i = 0; i <= cross; ++i)
//        //{
//        //    newgens1.Add(gens[0][i]);
//        //}

//        //for (int i = cross + 1; i < gens[0].Count; ++i)
//        //{
//        //    newgens2.Add(gens[0][i]);
//        //}

//        //for (int i = 0; i <= cross; ++i)
//        //{
//        //    newgens2.Add(gens[1][i]);
//        //}
//        //for (int i = cross + 1; i < gens[0].Count; ++i)
//        //{
//        //    newgens1.Add(gens[1][i]);
//        //}


//        //for (int i = 0; i < gens[0].Count; i++)
//        //{
//        //    double result1 = 0;
//        //    double result2 = 0;
//        //    //Mathf.Ceil
//        //    var rnd = Mathf.CeilToInt(UnityEngine.Random.Range(0, (gens.Count) + crossFactor));

//        //    if (rnd >= gens.Count)
//        //    {
//        //        //Debug.Log("GRÖSSER: ");
//        //        rnd = 0;
//        //    }
//        //    //else
//        //    //{
//        //    //    //Debug.Log("KLEINER: ");
//        //    //}
//        //    if (debug > 2)
//        //    {
//        //        Debug.Log("RND: " + rnd);
//        //    }

//        //    if (rnd == 0)
//        //    {
//        //        result1 = gens[0][i];
//        //        result2 = gens[1][i];
//        //    }
//        //    else
//        //    {
//        //        result1 = gens[1][i];
//        //        result2 = gens[0][i];
//        //    }


//        //    newgens1.Add(result1);
//        //    newgens2.Add(result2);

//        //}

//        //Debug.Log("------------------------------");

//        //  mutation

//        mutation(newgens1);
//        mutation(newgens2);

//        if (SaveCrossDnaCsv)        // ÄNDERN!!!
//        {
//            if (csvCounter < 1)
//            {
//                csvCounter++;
//                var csv = new Dictionary<string, List<double>>();
//                csv.Add("parent_1", gens[0]);
//                csv.Add("parent_2", gens[1]);
//                csv.Add("child_1", newgens1);
//                csv.Add("child_2", newgens2);

//                buildCrossDnaCsv(csv);
//            }
//        }
//        else
//        {
//            csvCounter = 0;
//        }

//        return new List<NeuralNet>() { buildNN1(), buildNN2() };
//    }

//    private void buildCrossDnaCsv(Dictionary<string, List<double>> csv)
//    {
//        var str = String.Empty;
//        for (int i = 0; i < csv["parent_1"].Count; i++)
//        {
//            str += csv["parent_1"][i].ToString() + "," + csv["parent_2"][i].ToString() + "," + csv["child_1"][i].ToString() + "," + csv["child_2"][i].ToString() + Environment.NewLine;
//        }

//        System.IO.File.WriteAllText("CrossDna_"+ csvCounter+".csv", str);
//    }
    
//    #region buildNN
//    //private NeuralNet buildNN1(List<double> dna, int inpuNeuron, int hiddenNeuron1, int hiddenNeuron2, int outputNeuron)
//    //{
//    //    newgens1 = new List<double>();
//    //    newgens1 = dna;
//    //    index1 = -1;
//    //    InpuNeuron = inpuNeuron;
//    //    HiddenNeuron1 = hiddenNeuron1;
//    //    HiddenNeuron2 = hiddenNeuron2;
//    //    OutputNeuron = outputNeuron;

//    //    return buildNN1();
//    //}

//    public NeuralNet buildNN1()
//    {
//        var nn = new NeuralNet(InpuNeuron, HiddenNeuron1, HiddenNeuron2, OutputNeuron);

//        for (int j = 0; j < nn.InputLayer.Count; j++)
//        {
//            nn.InputLayer[j].Bias = nextGen1();
//            nn.InputLayer[j] = buildNeuron1(nn.InputLayer[j]);
//        }

//        for (int j = 0; j < nn.HiddenLayer1.Count; j++)
//        {
//            nn.HiddenLayer1[j].Bias = nextGen1();
//            nn.HiddenLayer1[j] = buildNeuron1(nn.HiddenLayer1[j]);
//        }

//        if (nn.HiddenLayer2 != null)
//        {
//            for (int j = 0; j < nn.HiddenLayer2.Count; j++)
//            {
//                nn.HiddenLayer2[j].Bias = nextGen1();
//                nn.HiddenLayer2[j] = buildNeuron1(nn.HiddenLayer2[j]);
//            }
//        }

//        for (int j = 0; j < nn.OutputLayer.Count; j++)
//        {
//            nn.OutputLayer[j].Bias = nextGen1();
//            nn.OutputLayer[j] = buildNeuron1(nn.OutputLayer[j]);
//        }

//        return nn;
//    }

//    private NeuralNet buildNN2()
//    {
//        var nn = new NeuralNet(InpuNeuron, HiddenNeuron1, HiddenNeuron2, OutputNeuron);

//        for (int j = 0; j < nn.InputLayer.Count; j++)
//        {
//            nn.InputLayer[j].Bias = nextGen2();
//            nn.InputLayer[j] = buildNeuron2(nn.InputLayer[j]);
//        }

//        for (int j = 0; j < nn.HiddenLayer1.Count; j++)
//        {
//            nn.HiddenLayer1[j].Bias = nextGen2();
//            nn.HiddenLayer1[j] = buildNeuron2(nn.HiddenLayer1[j]);
//        }

//        if (nn.HiddenLayer2 != null)
//        {
//            for (int j = 0; j < nn.HiddenLayer2.Count; j++)
//            {
//                nn.HiddenLayer2[j].Bias = nextGen2();
//                nn.HiddenLayer2[j] = buildNeuron2(nn.HiddenLayer2[j]);
//            }
//        }

//        for (int j = 0; j < nn.OutputLayer.Count; j++)
//        {
//            nn.OutputLayer[j].Bias = nextGen2();
//            nn.OutputLayer[j] = buildNeuron2(nn.OutputLayer[j]);
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

//    private Neuron buildNeuron2(Neuron n)
//    {
//        for (int i = 0; i < n.OutputSynapses.Count; i++)
//        {
//            n.OutputSynapses[i].Weight = nextGen2();
//        }

//        return n;
//    }

//    private double nextGen1()
//    {
//        index1++;
//        return newgens1[index1];
//    }

//    private double nextGen2()
//    {
//        index2++;
//        return newgens2[index2];
//    }

//    private void mutation(List<double> n)
//    {
//        var p = Mathf.Ceil((mutateChance / n.Count) * 100);
//        if (debug > 1)
//        {
//            Debug.Log("p  : " + p);
//        }
//        for (int u = 0; u < p; u++)
//        {
//            var m = UnityEngine.Random.Range(0, n.Count);
//            if (debug > 1)
//            {
//                Debug.Log("m  : " + m);
//                Debug.Log("m-r: " + n[m] * -1);
//            }
//            n[m] = (n[m] / mutateDividor) * -1;
//        }
//    }
//    #endregion

//    #region readNN
//    private List<double> readNN(NeuralNet nn)
//    {
//        List<double> gens = new List<double>();

//        for (int j = 0; j < nn.InputLayer.Count; j++)
//        {
//            gens.Add(nn.InputLayer[j].Bias);
//            gens.AddRange(readNeuron(nn.InputLayer[j]));
//        }

//        for (int j = 0; j < nn.HiddenLayer1.Count; j++)
//        {
//            gens.Add(nn.HiddenLayer1[j].Bias);
//            gens.AddRange(readNeuron(nn.HiddenLayer1[j]));
//        }

//        if (nn.HiddenLayer2 != null)
//        {
//            for (int j = 0; j < nn.HiddenLayer2.Count; j++)
//            {
//                gens.Add(nn.HiddenLayer2[j].Bias);
//                gens.AddRange(readNeuron(nn.HiddenLayer2[j]));
//            }
//        }

//        for (int j = 0; j < nn.OutputLayer.Count; j++)
//        {
//            gens.Add(nn.OutputLayer[j].Bias);
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
//    #endregion

//#region sonstiges
//    private void destroyBot(List<GameObject> bots)
//    {
//        foreach (var bot in bots)
//        {
//            Destroy(bot);
//        }
//    }

//    private List<GameObject> getBest(List<GameObject> bots, int bestCount, bool deb = false)
//    {
//        var bb = bots.OrderByDescending(b => b.gameObject.GetComponent<Bot_4>().Health);

//        if (debug > 0 && deb)
//        {
//            var bbest = bb.First().GetComponent<Bot_4>();
//            var blast = bb.Last().GetComponent<Bot_4>();

//            //Debug.Log("-----------------");
//            Debug.Log("Best: " + bbest.Health);
//            Debug.Log("Last: " + blast.Health);
//        }

//        return bb.Take(bestCount).ToList();
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
//#endregion
//}
