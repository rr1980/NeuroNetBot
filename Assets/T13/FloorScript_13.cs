using System;
using System.Collections.Generic;
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

    public GameObject Wall;
    //public List<FromTo> FromTos;
    public int RndCount = 1;

    void Start()
    {

    }

    void Update()
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

        var r = GetComponent<MeshRenderer>();
        int size = (int)r.bounds.size.x / 2;
        float high = 0.5f;

        for (int i = -size+1; i <= size-1; i++)
        {
            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cube = Instantiate(Wall);
            cube.isStatic = true;
            cube.transform.position = new Vector3(i, high, -size);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        for (int i = -size - 1; i <= size - 1; i++)
        {
            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cube = Instantiate(Wall);
            cube.isStatic = true;
            cube.transform.position = new Vector3(-size, high, i);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        for (int i = -size; i <= size; i++)
        {
            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cube = Instantiate(Wall);
            cube.isStatic = true;
            cube.transform.position = new Vector3(i, high, size);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        for (int i = -size - 1; i <= size - 1; i++)
        {
            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cube = Instantiate(Wall);
            cube.isStatic = true;
            cube.transform.position = new Vector3(size, high, i);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }


        // ----------------

        for (int i = 0; i < RndCount; i++)
        {
            var x = UnityEngine.Random.Range(-size + 10, size);
            var z = UnityEngine.Random.Range(-size, size);

            var cube = Instantiate(Wall);
            cube.isStatic = true;
            cube.transform.position = new Vector3(x, high, z);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }

        for (int i = 0; i < RndCount/100; i++)
        {
            var x = UnityEngine.Random.Range(-size , - size + 10);
            var z = UnityEngine.Random.Range(-size+10, size);

            var cube = Instantiate(Wall);
            cube.isStatic = true;
            cube.transform.position = new Vector3(x, high, z);
            cube.tag = "Wall";
            cube.transform.parent = gameObject.transform;
        }


    }


    //void OnSceneGUI()
    //{
    //    Vector3 mousePosition = Event.current.mousePosition;
    //    mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
    //    mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
    //    mousePosition.y = -mousePosition.y;
    //}
}

