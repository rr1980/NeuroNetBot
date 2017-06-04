using UnityEngine;

[System.Serializable]
public class FromTo
{
    public GameObject From;
    public GameObject To;
}

//[ExecuteInEditMode]
public class FloorScript_13 : MonoBehaviour
{
    public bool isStatic;
    public GameObject Wall;
    public GameObject Food;

    //public List<FromTo> FromTos;
    public int RndCount = 1;

    private float xs = -60;
    private float zs = -60;
    private float xb = -74;
    private float zb = -74;

    private void Start()
    {
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log("+");
        //    var cube = Instantiate(Wall);
        //    cube.isStatic = true;
        //    cube.transform.position = Input.mousePosition;
        //    cube.tag = "Wall";
        //    cube.transform.parent = gameObject.transform;

        //    //RaycastHit hit;
        //    //if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        //    //{
        //    //    Terrain.SetBlock(hit, new BlockAir());
        //    //}
        //}
    }

    public void Randomize()
    {
        var ws = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var item in ws)
        {
            GameObject.DestroyImmediate(item.gameObject);
        }

        ws = GameObject.FindGameObjectsWithTag("Food");
        foreach (var item in ws)
        {
            GameObject.DestroyImmediate(item.gameObject);
        }

        var r = GetComponent<MeshRenderer>();
        int size = (int)r.bounds.size.x / 2;
        float high = 0.5f;

        for (int i = -size + 1; i <= size - 1; i++)
        {
            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cube = Instantiate(Wall);
            cube.isStatic = isStatic;
            cube.transform.position = new Vector3(i, high, -size);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        for (int i = -size - 1; i <= size - 1; i++)
        {
            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cube = Instantiate(Wall);
            cube.isStatic = isStatic;
            cube.transform.position = new Vector3(-size, high, i);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        for (int i = -size; i <= size; i++)
        {
            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cube = Instantiate(Wall);
            cube.isStatic = isStatic;
            cube.transform.position = new Vector3(i, high, size);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        for (int i = -size - 1; i <= size - 1; i++)
        {
            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cube = Instantiate(Wall);
            cube.isStatic = isStatic;
            cube.transform.position = new Vector3(size, high, i);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        // ----------------

        for (int i = 0; i < RndCount; i++)
        {
            var x = UnityEngine.Random.Range(-size + 2, size - 2);
            var z = UnityEngine.Random.Range(-size + 2, size - 2);

            if (UnityEngine.Random.Range(-1f, 1f) > 0)
            {
                var cube = Instantiate(Wall);
                cube.isStatic = isStatic;
                cube.transform.position = new Vector3(x, high, z);
                cube.tag = "Wall";
                cube.transform.parent = gameObject.transform;
            }
            else
            {
                GameObject food = Instantiate(Food);
                food.isStatic = isStatic;
                food.transform.position = new Vector3(x, high, z);
                food.transform.parent = gameObject.transform;
            }
        }

        var osW = GameObject.FindGameObjectsWithTag("Wall");
        var osF = GameObject.FindGameObjectsWithTag("Food");

        foreach (var item in osW)
        {
            var p = item.transform.position;

            if (p.x < xs && p.x > xb && p.z < zs && p.z > zb)
            {
                GameObject.DestroyImmediate(item.gameObject);
            }
        }

        foreach (var item in osF)
        {
            var p = item.transform.position;

            if (p.x < xs && p.x > xb && p.z < zs && p.z > zb)
            {
                GameObject.DestroyImmediate(item.gameObject);
            }
        }
    }
}