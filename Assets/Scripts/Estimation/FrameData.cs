using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameData
{
    public int frameNum;
    public List<Vector3> jointPositions;

    public FrameData(int num, List<Vector3> positions)
    {
        frameNum = num;
        jointPositions = positions;
    }
}
