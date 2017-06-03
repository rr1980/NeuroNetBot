using UnityEngine;

[ExecuteInEditMode]
public class FloorScript_13 : MonoBehaviour
{

    public GameObject Wall;

    void Start()
    {
        var all = GameObject.FindGameObjectsWithTag("Wall");

        foreach (var item in all)
        {
            DestroyImmediate(item.gameObject);
        }

        var r = GetComponent<MeshRenderer>();
        int size = (int)r.bounds.size.x / 2;

        for (int i = -size+1; i <= size-1; i++)
        {
            var cube = Instantiate(Wall);
            cube.isStatic = true;
            cube.transform.position = new Vector3(i, 0.5f, -7);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        for (int i = -size+1; i <= size-1; i++)
        {
            var cube = Instantiate(Wall);
            cube.isStatic = true;
            cube.transform.position = new Vector3(i, 0.5f, 7);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        for (int i = -7; i <= 7; i++)
        {
            var cube = Instantiate(Wall);
            cube.isStatic = true;
            cube.transform.position = new Vector3(size-1, 0.5f, i);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        for (int i = -7; i <= 7; i++)
        {
            var cube = Instantiate(Wall);
            cube.isStatic = true;
            cube.transform.position = new Vector3(-size+1, 0.5f, i);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        //for (int i = -size - 1; i <= size - 1; i++)
        //{
        //    //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    var cube = Instantiate(Wall);
        //    cube.isStatic = true;
        //    cube.transform.position = new Vector3(-size, 1, i);
        //    cube.tag = "Wall";
        //    cube.transform.parent = gameObject.transform;
        //}

        //for (int i = -size; i <= size; i++)
        //{
        //    //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    var cube = Instantiate(Wall);
        //    cube.isStatic = true;
        //    cube.transform.position = new Vector3(i, 1, size);
        //    cube.tag = "Wall";
        //    cube.transform.parent = gameObject.transform;
        //}

        //for (int i = -size - 1; i <= size - 1; i++)
        //{
        //    //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    var cube = Instantiate(Wall);
        //    cube.isStatic = true;
        //    cube.transform.position = new Vector3(size, 1, i);
        //    cube.tag = "Wall";
        //    cube.transform.parent = gameObject.transform;
        //}
    }

    void Update()
    {

    }
}
