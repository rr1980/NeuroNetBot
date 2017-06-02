using UnityEngine;
public class Controller_13 : MonoBehaviour
{
    public int FoodCount = 100;
    public Vector2 FoodSpawnRange = new Vector2(-80, 80);
    [Space(5)]
    public GameObject Food;

    private GameObject floor;
    private int old_foodCount;
    private Vector2 old_foodRange;

    private void Start()
    {
        floor = GameObject.FindGameObjectWithTag("Floor");
        setFood();
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
}