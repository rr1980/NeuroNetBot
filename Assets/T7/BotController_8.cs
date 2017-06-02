using UnityEngine;

[ExecuteInEditMode]
public class BotController_8 : MonoBehaviour
{
    [ReadOnly]
    public int TargetsCount;

    private SensorBank_8 sb;

    private void Start()
    {
        sb= GetComponentInChildren<SensorBank_8>();
    }

    private void Update()
    {
        TargetsCount = sb.Targets.Count;
    }
}
