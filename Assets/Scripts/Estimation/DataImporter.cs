using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataImporter : MonoBehaviour
{
    public TextAsset data;
    public List<FrameData> result;
    private GameObject[] joints;
    private int frame;
    private int nof;
    private int noj;

    void Start()
    {
        CSVReader reader = new CSVReader();
        result = reader.ReadFrameData(data);
        
        // get motion info
        frame = 0;
        nof = result.Count;
        noj = result[0].jointPositions.Count;
        
        // initiate joints
        joints = new GameObject[noj];
        for(int j = 0; j < noj; j++)
        {
            var joint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            joint.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            var jointRenderer = joint.GetComponent<Renderer>();
            jointRenderer.material.SetColor("_Color", Color.red);
            joints[j] = joint;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Draw estimation joint positions
        for(int j = 0; j < noj; j++)
        {
            Vector3 pos = 5.0f * result[frame].jointPositions[j];
            joints[j].transform.position = pos;
        }

        frame = (frame + 1) % nof;
    }
}
