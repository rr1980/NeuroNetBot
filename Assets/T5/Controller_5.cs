﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller_5 : MonoBehaviour
{
    public enum ControllerState
    {
        Run,
        Stop,
        Standbye

    }

    public ControllerState State = ControllerState.Run;
    public GameObject Bot;
    public GameObject Food;
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
    public float mutateChance = 0.5f;
    public float mutateDividor = 2;
    public float crossFactor = 5;
    public float antiInzest = 50;
    [Space(5)]
    public int MaxBotCount = 20;
    //public int ChildBotCount = 5;
    public int NewBotCount = 2;
    [Space(5)]
    public int FoodCount = 100;
    [Space(5)]
    public Vector2 BotSpawnRange = new Vector2(-50, 50);
    public Vector2 FoodSpawnRange = new Vector2(-80, 80);

    [Space(10)]
    public bool SaveCrossDna = false;
    [Space(5)]
    public float RefreshTime = 0.5f;

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

    void FixedUpdate()
    {
        if (pufferTime())
        {
            var bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
            var best = getBestBots(bots).Select(b => b.gameObject.GetComponent<Renderer>()).Take(2).ToList();

            foreach (var item in bots.Select(b => b.gameObject.GetComponent<Renderer>()).Where(r => r.material.color != Color.blue && r.material.color != Color.red && r.material.color != Color.black))
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

            foreach (var item in best.Select(b => b.gameObject.GetComponent<Renderer>()))
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
            var bestNNs = bestGOs.Select(b => b.gameObject.GetComponent<NetworkController_5>()).ToList();


            //buildGameObject(bestNNs[1].net, 2);
            //count++;

            //for (int i = 0; i <= (int)(ChildBotCount/2)-1 ; i++)
            //{
            //    childs.AddRange(NNAnalyzer.Crossover(bestNNs[0], bestNNs[i], crossFactor, mutateChance, mutateDividor, SaveCrossDna));
            //    SaveCrossDna = false;
            //}

            List<NeuralNet> childs = new List<NeuralNet>();


            var ind1 = 1;
            while (childs.Count < MaxBotCount / 2 && ind1 < bestNNs.Count)
            {
                var rrr = NNAnalyzer.Crossover(bestNNs[0].net, bestNNs[ind1].net, crossFactor, mutateChance, mutateDividor, bestNNs[0].Bias, antiInzest, SaveCrossDna);
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
            //Debug.Log("New 0:x = " + (ind1 - 1) + " : "+ childs.Count);

            //var ind2 = 2;
            ////childs = new List<NeuralNet>();
            //while (childs.Count < MaxBotCount - (MaxBotCount / 4) && ind2 < bestNNs.Count)
            //{
            //    var rrr = NNAnalyzer.Crossover(bestNNs[1].net, bestNNs[ind2].net, crossFactor, mutateChance, mutateDividor, bestNNs[0].Bias, antiInzest, SaveCrossDna);
            //    SaveCrossDna = false;
            //    if (rrr != null)
            //    {
            //        childs.AddRange(rrr);
            //    }
            //    ind2++;
            //}

            //foreach (var item in childs)
            //{
            //    buildGameObject(item, 1);
            //    count++;
            //}
            //Debug.Log("New 1:x = " + (ind1 - 2) + " : " + childs.Count);




            buildGameObject();
            count++;
            buildGameObject(bestNNs[0].net, 2);
            count++;

            while ((MaxBotCount - count) > 0)
            {
                var chrnd = NNAnalyzer.Crossover(bestNNs[0].net, new NeuralNet(bestNNs[0].Bias, bestNNs[0].InpuNeuron, bestNNs[0].HiddenNeuron1, bestNNs[0].HiddenNeuron2,0, bestNNs[0].OutputNeuron), crossFactor, mutateChance, mutateDividor, bestNNs[0].Bias, antiInzest, SaveCrossDna);
                buildGameObject(chrnd[0], 3);
                count++;
                if ((MaxBotCount - count) > 0)
                {
                    buildGameObject(chrnd[1], 3);
                    count++;
                }
            }


            //for (int i = 1; i < (bestNNs.Count/2)-2; i++)
            //{
            //    var rrr = NNAnalyzer.Crossover(bestNNs[0].net, bestNNs[i].net, crossFactor, mutateChance, mutateDividor, bestNNs[0].Bias, SaveCrossDna);
            //    childs.AddRange();
            //    SaveCrossDna = false;
            //}


            //buildGameObject(bestNNs[0].net, 2);
            //count++;
            //var chrnd= NNAnalyzer.Crossover(bestNNs[0].net, new NeuralNet(bestNNs[0].Bias, bestNNs[0].InpuNeuron, bestNNs[0].HiddenNeuron1, bestNNs[0].HiddenNeuron2, bestNNs[0].OutputNeuron), crossFactor, mutateChance, mutateDividor, bestNNs[0].Bias, SaveCrossDna);
            //buildGameObject(chrnd[0], 3);
            //count++;
            //buildGameObject(chrnd[1], 3);
            //count++;
            //List<NeuralNet> childs = NNAnalyzer.Crossover(bestNNs[0].net, bestNNs[1].net, crossFactor, mutateChance, mutateDividor, bestNNs[0].Bias, SaveCrossDna);

            //childs.AddRange(NNAnalyzer.Crossover(bestNNs[0].net, bestNNs[2].net, crossFactor, mutateChance, mutateDividor, bestNNs[0].Bias, SaveCrossDna));
            //childs.AddRange(NNAnalyzer.Crossover(bestNNs[1].net, bestNNs[2].net, crossFactor, mutateChance, mutateDividor, bestNNs[0].Bias, SaveCrossDna));



            //for (int i = 0; i <= NewBotCount-1; i++)
            //{
            //    buildGameObject();
            //    count++;
            //}

            //Debug.Log(count);
            //for (int i = 0; i <= (MaxBotCount - count)-1; i++)
            //{
            //    buildGameObject();
            //    //buildGameObject(bestNNs[i], 2);
            //}

            //List<NeuralNet> childs1 = NNAnalyzer.Crossover(bestNNs[0], bestNNs[1], crossFactor, mutateChance, mutateDividor, SaveCrossDna);
            //List<NeuralNet> childs2 = NNAnalyzer.Crossover(bestNNs[0], bestNNs[2], crossFactor, mutateChance, mutateDividor, SaveCrossDna=false);
            //List<NeuralNet> childs3 = NNAnalyzer.Crossover(bestNNs[0], bestNNs[3], crossFactor, mutateChance, mutateDividor);
            //List<NeuralNet> childs4 = NNAnalyzer.Crossover(bestNNs[1], bestNNs[2], crossFactor, mutateChance, mutateDividor);
            //List<NeuralNet> childs5 = NNAnalyzer.Crossover(bestNNs[1], bestNNs[3], crossFactor, mutateChance, mutateDividor);


            //buildGameObject(bestNNs[0], 2);
            //count++;

            //foreach (var item in childs1)
            //{
            //    buildGameObject(item, 1);
            //    count++;
            //}

            //foreach (var item in childs2)
            //{
            //    buildGameObject(item, 1);
            //    count++;
            //}

            //foreach (var item in childs3)
            //{
            //    buildGameObject(item, 1);
            //    count++;
            //}

            //foreach (var item in childs4)
            //{
            //    buildGameObject(item, 1);
            //    count++;
            //}

            //foreach (var item in childs5)
            //{
            //    buildGameObject(item, 1);
            //    count++;
            //}

            //for (int i = 0; i < MaxBotCount - count; i++)
            //{
            //    buildGameObject();
            //}

            destroyBot(bots);
            Generation++;
            startTime = Time.realtimeSinceStartup;
        }

        TimeRemaining = (float)Math.Round(Time.realtimeSinceStartup - startTime, 2);
        State = ControllerState.Run;
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
        NetworkController_5 nc = bot.GetComponent<NetworkController_5>();
        var inpuNeuron = nc.InpuNeuron;
        var hiddenNeuron1 = nc.HiddenNeuron1;
        var hiddenNeuron2 = nc.HiddenNeuron2;
        var outputNeuron = nc.OutputNeuron;

        if (nn == null)
        {
            nn = new NeuralNet(nc.Bias, inpuNeuron, hiddenNeuron1, hiddenNeuron2,0, outputNeuron);
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

    private void destroyBot(List<GameObject> bots)
    {
        foreach (var bot in bots)
        {
            Destroy(bot);
        }
    }

    private List<GameObject> getBestBots(List<GameObject> bots)
    {
        return bots.OrderByDescending(b => b.gameObject.GetComponent<Bot_5>().Health).ToList();
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