using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataImporter : MonoBehaviour
{
    public TextAsset data;
    // Start is called before the first frame update
    void Start()
    {
        CSVReader reader = new CSVReader();
        List<FrameData> result = reader.ReadFrameData(data);

        print(result[0]);
        print(result[0].frameNum);
        print(result[0].jointPositions[0].x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
