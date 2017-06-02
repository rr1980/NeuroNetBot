using UnityEngine;

public class Motor_9 : MonoBehaviour
{
    public enum MotorDirection
    {
        Left,
        Front,
        Right
    }

    public MotorDirection Direction;
    public GameObject Output;

    private Output_9 output;
    private MotorBank_9 mb;
    private BotController_9 bc;
    private Controller_9 c;

    private void Start()
    {
        output = Output.GetComponent<Output_9>();
        mb = transform.parent.GetComponent<MotorBank_9>();
        bc = transform.root.GetComponent<BotController_9>();
        c = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_9>();
    }

    private void Update()
    {
        Vector3 d = Vector3.zero;

        switch (Direction)
        {
            case MotorDirection.Left:
                d += new Vector3(0, -output.Value, 0) * mb.RotateSpeed * Time.deltaTime;
                bc.Health -= c.RotateCost;
                transform.root.Rotate(d);
                break;
            case MotorDirection.Front:
                transform.root.position += (transform.root.forward * Time.deltaTime * mb.Speed) * output.Value;
                bc.Health -= c.MoveCost;
                break;
            case MotorDirection.Right:
                d += new Vector3(0, output.Value, 0) * mb.RotateSpeed * Time.deltaTime;
                bc.Health -= c.RotateCost;
                transform.root.Rotate(d);
                break;
            default:
                break;
        }
    }
}