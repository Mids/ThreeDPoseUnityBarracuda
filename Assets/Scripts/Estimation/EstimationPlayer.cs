using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstimationPlayer : MonoBehaviour
{
    public TextAsset data;
    public List<FrameData> result;
    
    private GameObject[] joints;
    private GameObject joint_vis;
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
        foreach (var frameData in result)
        {
            frame = frameData.frameNum;
            print(frame);

            // Set Now3D values for all jointPoints
            SetJointPositions(jointPoints, frame);

            // Update pose
            estimationCharacter.PoseUpdate();

            yield return new WaitForSeconds(dt);

        }
    }

    private void SetJointPositions(EstimationCharacter.JointPoint[] jointPoints, int frame)
    {
        int scale = -3;
        jointPoints[PositionIndex.hip.Int()].Pos3D= scale * result[frame].jointPositions[0];
        jointPoints[PositionIndex.spine.Int()].Pos3D= scale * result[frame].jointPositions[7];
        jointPoints[PositionIndex.neck.Int()].Pos3D= scale * result[frame].jointPositions[8];
        //jointPoints[PositionIndex.Nose.Int()].Pos3D= result[frame].jointPositions[9];
        jointPoints[PositionIndex.head.Int()].Pos3D= scale * result[frame].jointPositions[10];

        jointPoints[PositionIndex.rThighBend.Int()].Pos3D= scale * result[frame].jointPositions[1];
        jointPoints[PositionIndex.rShin.Int()].Pos3D= scale * result[frame].jointPositions[2];
        jointPoints[PositionIndex.rFoot.Int()].Pos3D= scale * result[frame].jointPositions[3];
        
        jointPoints[PositionIndex.lThighBend.Int()].Pos3D= scale * result[frame].jointPositions[4];
        jointPoints[PositionIndex.lShin.Int()].Pos3D= scale * result[frame].jointPositions[5];
        jointPoints[PositionIndex.lFoot.Int()].Pos3D= scale * result[frame].jointPositions[6];

        jointPoints[PositionIndex.lShldrBend.Int()].Pos3D= scale * result[frame].jointPositions[11];
        jointPoints[PositionIndex.lForearmBend.Int()].Pos3D= scale * result[frame].jointPositions[12];
        jointPoints[PositionIndex.lHand.Int()].Pos3D= scale * result[frame].jointPositions[13];

        jointPoints[PositionIndex.rShldrBend.Int()].Pos3D= scale * result[frame].jointPositions[14];
        jointPoints[PositionIndex.rForearmBend.Int()].Pos3D= scale * result[frame].jointPositions[15];
        jointPoints[PositionIndex.rHand.Int()].Pos3D= scale * result[frame].jointPositions[16];

    }

}
