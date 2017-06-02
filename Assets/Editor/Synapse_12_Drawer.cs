using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Synapse_12))]
public class Synapse_12_Drawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //GUI.enabled = false; // Disable fields
        //EditorGUI.PrefixLabel(position, label);
        //SerializedProperty input = property.FindPropertyRelative("InputNeuron.Count");
        SerializedProperty outputNeuron = property.FindPropertyRelative("OutputNeuron");
        SerializedProperty outputSynapse = outputNeuron.FindPropertyRelative("OutputSynapse");

        //if (input != null)
        //{
        //    Debug.Log(input.intValue);
        //}

        if (outputSynapse != null)
        {
            //var v = outputSynapse.FindPropertyRelative("Nanu");
            //Debug.Log(outputSynapse.arraySize);
            EditorGUI.TextField(position, new GUIContent("Syn"), outputSynapse.arraySize.ToString(), GUIStyle.none);
        }

        //Neuron_12 n = (Neuron_12) property.serializedObject.targetObject as Neuron_12;

        //EditorGUI.PropertyField(position,index, GUIContent.none);


        //GUI.enabled = true; // Enable fields
    }
}