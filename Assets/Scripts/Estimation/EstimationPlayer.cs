using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstimationPlayer : MonoBehaviour
{
    public TextAsset data;
    public List<FrameData> result;
    
    private GameObject[] joints;
    private GameObject joint_vis;
    private GameObject forward_vis;
    private int frame;
    private int nof;
    private int noj;

    public float fps=30;
    public float dt;
    public int vis_index = 0;

    public void ReadData()
    {
        CSVReader reader = new CSVReader();
        result = reader.ReadFrameData(data);

        // get motion info
        frame = 0;
        nof = result.Count;
        noj = result[0].jointPositions.Count;
        print(nof);
        print(noj);
        dt = 1f / fps;
    }

    public void VisualizeCoroutine()
    {
        // Read data if empty
        if (result == null)
            ReadData();

        // Initiate spheres if empty
        if (joints == null)
        {
            joints = new GameObject[noj];
            for (int j = 0; j < noj; j++)
            {
                var joint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                joint.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                var jointRenderer = joint.GetComponent<Renderer>();
                jointRenderer.material.SetColor("_Color", Color.red);
                joints[j] = joint;
            }
        }

        if(joint_vis == null)
        {
            joint_vis = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            joint_vis.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            var jointRenderer = joint_vis.GetComponent<Renderer>();
            jointRenderer.material.SetColor("_Color", Color.blue);
        }

        StartCoroutine(Visualize());
    }


    private IEnumerator Visualize()
    {
        foreach (var frameData in result)
        {
            // Visualize estimated joint positions
            for (int j = 0; j < noj; j++)
            {
                Vector3 pos = 3.0f * frameData.jointPositions[j];
                pos.y *= -1;
                joints[j].transform.position = pos;
            }

            Vector3 current = 5.0f * frameData.jointPositions[vis_index];
            current.y *= -1;
            joint_vis.transform.position = current;

            yield return new WaitForSeconds(dt);
        }
       
    }


    public EstimationCharacter estimationCharacter;
    private EstimationCharacter.JointPoint[] jointPoints;

    public void PlayCoroutine()
    {
        // Read data if empty
        if (result == null)
        {
            ReadData();
            print("Read all data!");
        }

        // Init model with jointPoints
        jointPoints = estimationCharacter.Init();


        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        if(forward_vis == null)
        {
            forward_vis = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            forward_vis.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            var forwardRenderer = forward_vis.GetComponent<Renderer>();
            forwardRenderer.material.SetColor("_Color", Color.red);
        }

        foreach (var frameData in result)
        {
            frame = frameData.frameNum;
            print(frame);

            // Set Pose3D values for all jointPoints
            SetPose(jointPoints, frame);

            // Update pose
            estimationCharacter.PoseUpdate(forward_vis);

            yield return new WaitForSeconds(dt);

        }
    }

    private void SetPose(EstimationCharacter.JointPoint[] jointPoints, int frame)
    {
        SetJointPosition(jointPoints[PositionIndex.hip.Int()], result[frame].jointPositions[0]);
        SetJointPosition(jointPoints[PositionIndex.spine.Int()], result[frame].jointPositions[7]);
        SetJointPosition(jointPoints[PositionIndex.neck.Int()], result[frame].jointPositions[8]);
        SetJointPosition(jointPoints[PositionIndex.head.Int()], result[frame].jointPositions[10]);
        
        SetJointPosition(jointPoints[PositionIndex.rThighBend.Int()], result[frame].jointPositions[1]);
        SetJointPosition(jointPoints[PositionIndex.rShin.Int()], result[frame].jointPositions[2]);
        SetJointPosition(jointPoints[PositionIndex.rFoot.Int()], result[frame].jointPositions[3]);

        SetJointPosition(jointPoints[PositionIndex.lThighBend.Int()], result[frame].jointPositions[4]);
        SetJointPosition(jointPoints[PositionIndex.lShin.Int()], result[frame].jointPositions[5]);
        SetJointPosition(jointPoints[PositionIndex.lFoot.Int()], result[frame].jointPositions[6]);

        SetJointPosition(jointPoints[PositionIndex.lShldrBend.Int()], result[frame].jointPositions[11]);
        SetJointPosition(jointPoints[PositionIndex.lForearmBend.Int()], result[frame].jointPositions[12]);
        SetJointPosition(jointPoints[PositionIndex.lHand.Int()], result[frame].jointPositions[13]);

        SetJointPosition(jointPoints[PositionIndex.rShldrBend.Int()], result[frame].jointPositions[14]);
        SetJointPosition(jointPoints[PositionIndex.rForearmBend.Int()], result[frame].jointPositions[15]);
        SetJointPosition(jointPoints[PositionIndex.rHand.Int()], result[frame].jointPositions[16]);

    }

    private void SetJointPosition(EstimationCharacter.JointPoint jointPoint, Vector3 position)
    {
        int y_scale = -3;
        position.y *= y_scale;
        jointPoint.Pos3D = position;
    }
}
