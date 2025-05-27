using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIStructure))]
public class UIStructureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate Code"))
        {
            UICodeGeneration.GenerateCodeFromObject(((UIStructure)target).gameObject);
        }
    }
}