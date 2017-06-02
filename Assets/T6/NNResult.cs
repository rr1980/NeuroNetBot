using UnityEngine;

[System.Serializable]
public class NNResult
{
    public string Direction;
    public float Value;

    public NNResult(string direction, double value)
    {
        Direction = direction;
        Value = (float)value;
    }
}

