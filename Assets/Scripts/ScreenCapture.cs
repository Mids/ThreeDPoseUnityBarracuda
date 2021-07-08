using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCapture: MonoBehaviour
{
    public bool capture = false;
    public string capturePath = "";

    void Start()
    {
        
    }

    void Update()
    {
        if (capture)
        {
            StartCoroutine(captureScreenShot());
        }
    }

    int screenShotCount = 0;
    IEnumerator captureScreenShot()
    {
        yield return new WaitForEndOfFrame();

        string path = capturePath + "file_" + "_" + Screen.width + "X" + Screen.height + "_file" + screenShotCount.ToString("D4") + ".png";

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Save image to file
        System.IO.File.WriteAllBytes(path, imageBytes);

        screenShotCount++;
    }
}
