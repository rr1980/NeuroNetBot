//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//public class NeuroDebug : EditorWindow
//{

//    private GameObject bot;
//    private int a;
//    // Add menu named "My Window" to the Window menu
//    [MenuItem("Window/NeuroDebug")]
//    static void Init()
//    {
//        // Get existing open window or if none, make a new one:
//        NeuroDebug window = (NeuroDebug)EditorWindow.GetWindow(typeof(NeuroDebug));
//        window.Show();
//    }

//    void OnGUI()
//    {
//        if (bot != null)
//        {
//            GUILayout.Label(bot.name, EditorStyles.boldLabel);

//            var h = bot.GetComponent<Bot_4>().Health;

//            EditorGUILayout.TextField("Text Field", h.ToString());
//        }

//        //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
//        //myBool = EditorGUILayout.Toggle("Toggle", myBool);
//        //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
//        //EditorGUILayout.EndToggleGroup();

//        //EditorUtility.SetDirty(component);
//        Handles.BeginGUI();
//        Handles.color = Color.red;
//        Handles.DrawLine(new Vector3(0, 0), new Vector3(300, 300));
//        Handles.EndGUI();
//    }

//    private void Update()
//    {
//        bot = Selection.gameObjects.FirstOrDefault(s=>s.tag=="Bot");
//        //bot = GameObject.FindGameObjectsWithTag("Bot").First();

//        Repaint();
//    }
//}
