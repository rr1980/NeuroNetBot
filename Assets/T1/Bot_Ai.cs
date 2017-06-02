//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Bot_Ai : MonoBehaviour
//{
//    public bool Train = true;
//    public float Speed = 1;
//    [Range(10,50)]
//    public float RoundSpeed = 10f;
//    public int TrainRounds = 1;
//    public GameObject Target;

//    [SerializeField]
//    public NeuralNet NeuralNet;



//    // Use this for initialization
//    void Start()
//    {
//        NeuralNet = new NeuralNet(4, 8, 1);

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Target != null)
//        {
//            float dir = AngleDir(transform.position, Target.transform.position, Vector3.up);
//            double[] input = new double[] { transform.position.x, transform.position.z, Target.transform.position.x, Target.transform.position.z };
//            if (Train)
//            {
//                train(input, dir);
//            }
//            float xr = (compute(input) * RoundSpeed);

//            transform.Rotate(0, xr * Time.deltaTime, 0);
//            transform.position += transform.forward * Speed * Time.deltaTime;
//        }
//    }

//    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
//    {
//        var relativePoint = transform.InverseTransformPoint(Target.transform.position);
//        if (relativePoint.x < 0.0)
//            return 0;
//        else if (relativePoint.x > 0.0)
//            return 1;
//        else
//            return 0.5f;
//    }

//    void train(double[] input, float soll)
//    {
//        List<DataSets> ds = new List<DataSets>() { new DataSets(input, new double[] { soll }) };

//        NeuralNet.TrainC(ds, TrainRounds);
//    }

//    int compute(double[] input)
//    {
//        var resultT = NeuralNet.Compute(input)[0];

//        if (resultT > 0.5)
//        {
//            return 1;
//        }
//        else if (resultT < 0.5)
//        {
//            return -1;
//        }
//        else
//        {
//            return 0;
//        }
//    }

//    private void OnDrawGizmos()
//    {
//        DebugExtension.DrawArrow(transform.position, transform.forward);
//    }
//}
