using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    /*
    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
    */


    /*
     1. Get number of frame
     2. Save list of FrameData
     3. Return list of FrameData as Result
     */

 
    //public List<FrameData> ReadFrameData(string file)
    public List<FrameData> ReadFrameData(TextAsset data)
    {
        var result = new List<FrameData>();
        //TextAsset data = Resources.Load("Assets/Resources/videopose_traj_result.csv") as TextAsset;

        var frames = Regex.Split(data.text, LINE_SPLIT_RE);

        if (frames.Length <= 1) return result;

        for (int i = 0; i < frames.Length; i++)
        {
            var values = Regex.Split(frames[i], SPLIT_RE);
            int n = values.Length;

            if (n == 0 || values[0] == "") continue;

            List<Vector3> jointPositions = new List<Vector3>(n);
            for (int j = 0; j < n / 3; j++)
            {
                Vector3 joint;
                joint.x = float.Parse(values[3 * j]);
                joint.y = float.Parse(values[3 * j + 1]);
                joint.z = float.Parse(values[3 * j + 2]);
                jointPositions.Add(joint);
            }
            FrameData frame = new FrameData(i, jointPositions);
            result.Add(frame);
        }
        return result;
    }
}
