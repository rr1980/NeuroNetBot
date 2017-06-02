using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BotController_9))]
public class Bot_9Editor : Editor
{
    public override void OnInspectorGUI()
    {
        BotController_9 bot = (BotController_9)target;

        var c = GameObject.FindGameObjectWithTag("MainCamera");
        Cam cam = c.GetComponent<Cam>();

        var d = GameObject.FindGameObjectWithTag("Debugger");
        DebuggerController debugger = d.GetComponent<DebuggerController>();


        if (GUILayout.Button("Debugger"))
        {
            debugger.Selected = bot.gameObject;
            debugger._new = true;
        }

        if (GUILayout.Button("Cam Follow"))
        {
            cam.target = bot.transform;
        }

        DrawDefaultInspector();
    }

}
