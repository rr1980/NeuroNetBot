using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Controller_9 : MonoBehaviour
{

    public enum ControllerState
    {
        Run,
        Stop,
        Standbye

    }

    public ControllerState State = ControllerState.Run;
    [Space(5)]
    public float RoundTime = 20;
    [ReadOnly]
    public float TimeRemaining;
    [ReadOnly]
    public float Generation = 0;
    [Space(5)]
    public float FoodGod = 1000;
    public float WallPain = 1000;
    public float BotPain = 100;
    public float MoveCost = 10;
    public float RotateCost = 10;
    public float StayCost = 10;
    [Space(5)]
    public float mutateChance = 0.15f;
    public float perbetuation = 0.3f;
    public float crossFactor = 5;
    public float antiInzest = 50;
    [Space(5)]
    public int MaxBotCount = 20;
    //public int ChildBotCount = 5;
    //public int NewBotCount = 2;
    [Space(5)]
    public int FoodCount = 100;
    [Space(5)]
    public Vector2 BotSpawnRange = new Vector2(-50, 50);
    public Vector2 FoodSpawnRange = new Vector2(-80, 80);

    [Space(10)]
    public bool SaveCrossDna = false;
    [Space(5)]
    public float RefreshTime = 0.5f;
    [Space(5)]
    public GameObject Bot;
    public GameObject Food;

    private GameObject floor;
    private int old_foodCount;
    private Vector2 old_foodRange;
    private float startTime = 0;
    private float elapsed;

    void Start()
    {
        elapsed = 0;
        RefreshTime = 0;
        floor = GameObject.FindGameObjectWithTag("Floor");
        setFood();

        for (int i = 0; i < MaxBotCount; i++)
        {
            buildGameObject();
        }
    }


    void Update()
    {
        if (pufferTime())
        {
            var bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
            var best = getBestBots(bots).Select(b => b.gameObject.GetComponentInChildren<Renderer>()).Take(2).ToList();

            foreach (var item in bots.Select(b => b.gameObject.GetComponentInChildren<Renderer>()).Where(r => r.material.color != Color.blue && r.material.color != Color.red && r.material.color != Color.black))
            {
                if (item.GetComponent<Renderer>().material.color == Color.cyan)
                {
                    item.material.color = Color.blue;
                }
                else if (item.GetComponent<Renderer>().material.color == Color.magenta)
                {
                    item.material.color = Color.red;
                }
                else if (item.GetComponent<Renderer>().material.color == Color.gray)
                {
                    item.material.color = Color.black;
                }
                else
                {
                    item.material.color = Color.white;
                }
            }

            foreach (var item in best.Select(b => b.gameObject.GetComponentInChildren<Renderer>()))
            {
                if (item.GetComponent<Renderer>().material.color == Color.blue)
                {
                    item.material.color = Color.cyan;
                }
                else if (item.GetComponent<Renderer>().material.color == Color.red)
                {
                    item.material.color = Color.magenta;
                }
                else if (item.GetComponent<Renderer>().material.color == Color.black)
                {
                    item.material.color = Color.gray;
                }
                else
                {
                    item.material.color = Color.yellow;
                }
            }
        }


        updateGeneration();
    }

    private void updateGeneration()
    {
        if (startTime == 0)
        {
            startTime = Time.realtimeSinceStartup;
        }

        if (RoundTime < (Time.realtimeSinceStartup - startTime))
        {
            State = ControllerState.Standbye;
            updateFood();
            var count = 0;

            var bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
            bots.ForEach(s => s.SetActive(false));

            var bestGOs = getBestBots(bots);
            var bestNNs = bestGOs.Select(b => b.gameObject.GetComponentInChildren<NeuralNet_9>()).ToList();
            //var besth = bestGOs.Select(b => b.GetComponent<BotController_9>()).ToList();

            List<NeuralNet> childs = new List<NeuralNet>();

            //var rrr = NNAnalyzer.Crossover(bestNNs[0].net, bestNNs[1].net, crossFactor, mutateChance, mutateDividor, bestNNs[0].Bias, antiInzest, SaveCrossDna);
            //SaveCrossDna = false;
            //childs.AddRange(rrr);

            var ind1 = 1;
            while (childs.Count < MaxBotCount / 2 && ind1 < bestNNs.Count)
            {
                var rrr = NNAnalyzer.Crossover(bestNNs[0].net, bestNNs[ind1].net, crossFactor, mutateChance, perbetuation, bestNNs[0].Bias, antiInzest, SaveCrossDna);
                SaveCrossDna = false;
                if (rrr != null)
                {
                    childs.AddRange(rrr);
                }
                ind1++;
            }
            foreach (var item in childs)
            {
                buildGameObject(item, 1);
                count++;
            }


            buildGameObject();
            count++;
            buildGameObject(bestNNs[0].net, 2);
            count++;

            while ((MaxBotCount - count) > 0)
            {
                var chrnd = NNAnalyzer.Crossover(bestNNs[0].net, new NeuralNet(bestNNs[0].Bias, bestNNs[0].InputNeurons, bestNNs[0].HiddensNeurons, bestNNs[0].HiddensNeurons2, bestNNs[0].HiddensNeurons3, bestNNs[0].OutputNeurons), crossFactor, mutateChance, perbetuation, bestNNs[0].Bias, antiInzest, SaveCrossDna);
                buildGameObject(chrnd[0], 3);
                count++;
                if ((MaxBotCount - count) > 0)
                {
                    buildGameObject(chrnd[1], 3);
                    count++;
                }
            }


            destroyBot(bots);
            Generation++;
            startTime = Time.realtimeSinceStartup;
        }

        TimeRemaining = (float)Math.Round(Time.realtimeSinceStartup - startTime, 2);
        State = ControllerState.Run;
    }

    private void buildGameObject(NeuralNet nn = null, int mark = 0)
    {
        var x = UnityEngine.Random.Range(BotSpawnRange.x, BotSpawnRange.y);
        var z = UnityEngine.Random.Range(BotSpawnRange.x, BotSpawnRange.y);
        var t = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
        GameObject bot = Instantiate(Bot, new Vector3(x, 1, z), t);

        if (mark == 1)
        {
            bot.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (mark == 2)
        {
            bot.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (mark == 3)
        {
            bot.GetComponent<Renderer>().material.color = Color.black;
        }
        //else if (mark == 4)
        //{
        //    bot.GetComponent<Renderer>().material.color = Color.;
        //}

        bot.SetActive(false);
        NeuralNet_9 nc = bot.GetComponentInChildren<NeuralNet_9>();

        if (nn == null)
        {
            var inpuNeuron = nc.InputNeurons;
            var hiddenNeuron1 = nc.HiddensNeurons;
            var hiddenNeuron2 = nc.HiddensNeurons2;
            var hiddenNeuron3 = nc.HiddensNeurons3;
            var outputNeuron = nc.OutputNeurons;
            nn = new NeuralNet(nc.Bias, inpuNeuron, hiddenNeuron1, hiddenNeuron2, hiddenNeuron3, outputNeuron);
        }

        if (!nc.networkInit)
        {
            nc.Init(nn);
        }


        //bot.transform.position = new Vector3(x, 1, z);

        //var rot = UnityEngine.Random.rotation;
        //bot.transform.rotation.SetLookRotation(new Vector3(0, rot.y, 0));
        //bot.transform.rotation = Quaternion.Euler(0,UnityEngine.Random.Range(-0, 10), 0);
        bot.SetActive(true);
    }

    private void setFood()
    {
        old_foodRange = FoodSpawnRange;
        old_foodCount = FoodCount;
        for (int i = 0; i < FoodCount; i++)
        {
            GameObject food = Instantiate(Food);
            food.transform.parent = floor.transform;
            var x = UnityEngine.Random.Range(FoodSpawnRange.x, FoodSpawnRange.y);
            var z = UnityEngine.Random.Range(FoodSpawnRange.x, FoodSpawnRange.y);
            food.transform.position = new Vector3(x, 0.5f, z);
        }
    }

    private void updateFood()
    {
        if (old_foodCount != FoodCount || old_foodRange != FoodSpawnRange)
        {
            var foods = GameObject.FindGameObjectsWithTag("Food").ToList();
            destroyBot(foods);
            setFood();
        }
    }

    private void destroyBot(List<GameObject> bots)
    {
        foreach (var bot in bots)
        {
            Destroy(bot);
        }
    }

    private List<GameObject> getBestBots(List<GameObject> bots)
    {
        return bots.OrderByDescending(b => b.gameObject.GetComponent<BotController_9>().Health).ToList();
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
