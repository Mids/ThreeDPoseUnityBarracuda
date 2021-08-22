using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EstimationPlayer))]
public class EstimationPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Play"))
            (target as EstimationPlayer)?.PlayCoroutine();
    }
}
