using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanForNewImages : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ////first Make sure you're using RGB24 as your texture format
        //Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);

        ////then Save To Disk as PNG
        //byte[] bytes = texture.EncodeToPNG();
        //var dirPath = Application.dataPath + "/../SaveImages/";
        //if (!Directory.Exists(dirPath))
        //{
        //    Directory.CreateDirectory(dirPath);
        //}
        //File.WriteAllBytes(dirPath + "Image" + ".png", bytes);
    }
}
