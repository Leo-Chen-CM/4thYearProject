using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class ScanForNewImages : MonoBehaviour
{
    Texture2D image;
    Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Soldiers");

        //foreach (GameObject pref in prefabs)
        //{
        //    Texture2D td = AssetPreview.GetAssetPreview(pref);
        //    Rect rect = new Rect(0, 0, td.width, td.height);

        //    sprite = Sprite.Create(td, rect, new Vector2(0, 0));
        //    image = sprite.texture;
        //    //Save To Disk as PNG
        //    byte[] bytes = image.EncodeToPNG();
        //    var dirPath = Application.dataPath + "/../Sprites/";
        //    if (!Directory.Exists(dirPath))
        //    {
        //        Directory.CreateDirectory(dirPath);
        //    }
        //    File.WriteAllBytes(dirPath + pref.name + ".png", bytes);
        //}
    }
}
