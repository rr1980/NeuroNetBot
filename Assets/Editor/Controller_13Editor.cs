using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Controller_13))]
public class Controller_13Editor : Editor
{
    public override void OnInspectorGUI()
    {
        Controller_13 c = (Controller_13)target;


        if (GUILayout.Button("ResetBest"))
        {

            c.ResetBest();
        }

        DrawDefaultInspector();
    }

}