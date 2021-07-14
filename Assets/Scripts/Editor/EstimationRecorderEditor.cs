using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoseEstimationRecorder))]
public class EstimationRecorderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PoseEstimationRecorder poseEstimationRecorder = (PoseEstimationRecorder)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Export"))
            (target as PoseEstimationRecorder)?.StartRecordingEstimation();
            //poseEstimationRecorder.StartRecordingEstimation();
    }
}