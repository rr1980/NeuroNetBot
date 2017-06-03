using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Bot_13))]
public class Bot_13Editor : Editor
{
    public override void OnInspectorGUI()
    {
        Bot_13 bot = (Bot_13)target;


        if (GUILayout.Button("SetNN"))
        {
            var n = ScriptableObject.CreateInstance("NN_13") as NN_13;
            n.Init(bot.Inputs, bot.Hiddens, bot.Outputs);
            bot.Init(n);
        }

        DrawDefaultInspector();
    }

}
