using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetSortOrderByY))]
public class SetYByPosition_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Set Sorting Order"))
        {
            ((SetSortOrderByY)target).Set();
        }
    }
}
