using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstimationPlayer : MonoBehaviour
{
    public TextAsset data;
    public List<FrameData> result;
    
    private GameObject[] joints;
    private int frame;
    private int nof;
    private int noj;

    public float fps=30;
    public float dt;

    public void PlayCoroutine()
    {
        CSVReader reader = new CSVReader();
        result = reader.ReadFrameData(data);
        
        // get motion info
        frame = 0;
        nof = result.Count;
        noj = result[0].jointPositions.Count;
        dt = 1f / fps;

        // initiate joints
        if(joints != null)
            ClearArray(joints);
        joints = new GameObject[noj];
        for(int j = 0; j < noj; j++)
        {
            var joint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            joint.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            var jointRenderer = joint.GetComponent<Renderer>();
            jointRenderer.material.SetColor("_Color", Color.red);
            joints[j] = joint;
        }

        StartCoroutine(Play());
    }


    private IEnumerator Play()
    {
        foreach (var frameData in result)
        {
            // Visualize estimated joint positions
            for (int j = 0; j < noj; j++)
            {
                Vector3 pos = 5.0f * frameData.jointPositions[j];
                pos.y *= -1;
                joints[j].transform.position = pos;
            }

            yield return new WaitForSeconds(dt);
        }
       
    }

    public static void ClearArray(GameObject[] array)
    {
        foreach (GameObject obj in array)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }
}
