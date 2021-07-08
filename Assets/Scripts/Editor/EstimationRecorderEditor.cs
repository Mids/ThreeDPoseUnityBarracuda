using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoseEstimationRecorder))]
public class EstimationRecorderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Export"))
            (target as PoseEstimationRecorder)?.Estimate();
    }
}