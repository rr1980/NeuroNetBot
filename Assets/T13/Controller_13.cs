using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Controller_13 : MonoBehaviour
{
    public float RoundTime = 20;
    [ReadOnly]
    public float Generation = 0;
    [Space(5)]
    public float mutateChance = 0.15f;
    public float perbetuation = 0.2f;
    public int crossFactor = 3;
    public bool SaveCrossDna = false;
    [ReadOnly]
    public float TimeRemaining;
    public int MaxBotCount = 20;
    public int FoodCount = 100;
    [Space(5)]
    public Vector2 BotSpawnRangeX = new Vector2(-50, 50);
    public Vector2 BotSpawnRangeY = new Vector2(-50, 50);
    public Vector2 FoodSpawnRange = new Vector2(-80, 80);
    [Space(5)]
    public GameObject Food;
    public GameObject Bot;

    private GameObject floor;
    private int old_foodCount;
    private Vector2 old_foodRange;
    private float startTime = 0;

    private List<GameObject> bots;

    private void Start()
    {
        floor = GameObject.FindGameObjectWithTag("Floor");
        setFood();

        for (int i = 0; i < MaxBotCount; i++)
        {
            buildGameObject();
        }
        bots = GameObject.FindGameObjectsWithTag("Bot").ToList();

    }

    private void Update()
    {
        if (startTime == 0)
        {
            startTime = Time.realtimeSinceStartup;
        }

        if (!bots.Any(b => b.activeSelf) || RoundTime < (Time.realtimeSinceStartup - startTime))
        {
            updateFood();

            //var bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
            bots.ForEach(s => s.SetActive(false));
            var bestGOs = getBestBots(bots);
            var bestNNs = bestGOs.Select(b => b.gameObject.GetComponentInChildren<Bot_13>()).ToList();

            List<NN_13> childs = new List<NN_13>();

            childs.Add(bestNNs[0].NN);

            var rawnn0 = DNAAnalyzer_13.ReadNN(bestNNs[0].NN);
            var rawnn1 = DNAAnalyzer_13.ReadNN(bestNNs[1].NN);
            List<int> hs = getHiddenCount(bestNNs[0]);

            for (int i = 0; i < Mathf.CeilToInt(MaxBotCount / 2); i++)
            {
                var newnn = DNAAnalyzer_13.Sex(rawnn0, rawnn1, crossFactor, mutateChance, perbetuation);
                NN_13 n1 = DNAAnalyzer_13.BuildNN(newnn[0], bestNNs[0].NN.Layers[0].Neurons.Count, hs, bestNNs[0].NN.Layers.Last().Neurons.Count);
                NN_13 n2 = DNAAnalyzer_13.BuildNN(newnn[1], bestNNs[0].NN.Layers[0].Neurons.Count, hs, bestNNs[0].NN.Layers.Last().Neurons.Count);

                if (SaveCrossDna)
                {
                    DNAAnalyzer_13.WriteDnasToCsv(rawnn0, rawnn1, newnn[0], newnn[1]);
                    SaveCrossDna = false;
                }

                childs.Add(n1);
                childs.Add(n2);
            }


            foreach (var item in childs)
            {
                buildGameObject(item);
            }

            destroyItems(bots);
            Generation++;
            bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
            startTime = Time.realtimeSinceStartup;
        }
        
        TimeRemaining = (float)Math.Round(Time.realtimeSinceStartup - startTime, 2);
    }


    #region privates
    private void buildGameObject(NN_13 n = null)
    {
        var x = UnityEngine.Random.Range(BotSpawnRangeX.x, BotSpawnRangeX.y);
        var z = UnityEngine.Random.Range(BotSpawnRangeY.x, BotSpawnRangeY.y);
        var t = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
        GameObject bot = Instantiate(Bot, new Vector3(x, 1, z), t);
        bot.SetActive(false);
        Bot_13 bc = bot.GetComponentInChildren<Bot_13>();
        if (n == null)
        {
            n = ScriptableObject.CreateInstance("NN_13") as NN_13;
            n.Init(bc.Inputs, bc.Hiddens, bc.Outputs);
        }
        bc.Init(n);
        bot.SetActive(true);
    }

    private List<int> getHiddenCount(Bot_13 n)
    {
        var ll = n.NN.Layers.Count - 2;
        List<int> hs = new List<int>();
        for (int i = 1; i <= ll; i++)
        {
            hs.Add(n.NN.Layers[i].Neurons.Count);
        }

        return hs;
    }

    private List<GameObject> getBestBots(List<GameObject> bots)
    {
        return bots.OrderByDescending(b => b.gameObject.GetComponent<Bot_13>().Fitness).ToList();
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
            destroyItems(foods);
            setFood();
        }
    }

    private void destroyItems(List<GameObject> items)
    {
        foreach (var item in items)
        {
            Destroy(item);
        }
    }
    #endregion
}