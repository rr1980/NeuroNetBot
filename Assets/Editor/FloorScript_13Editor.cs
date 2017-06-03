using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FloorScript_13))]
public class FloorScript_13Editor : Editor
{
    public override void OnInspectorGUI()
    {
        FloorScript_13 fs = (FloorScript_13)target;


        if (GUILayout.Button("Randomize"))
        {
            fs.Randomize();
        }

        //if (Event.current.type == EventType.MouseDown)
        //{
        //    Vector2 guiPosition = Event.current.mousePosition;
        //    Ray ray = HandleUtility.GUIPointToWorldRay(guiPosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray,out hit))
        //    {
        //        fs.Set(hit.point);
        //    }

        //}

        DrawDefaultInspector();

    }
}