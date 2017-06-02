using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Bot_13))]
public class Bot_13Editor : Editor
{
    public override void OnInspectorGUI()
    {
        Bot_13 bot = (Bot_13)target;


        if (GUILayout.Button("Compute"))
        {
            bot.Compute();
        }

        DrawDefaultInspector();
    }

}
