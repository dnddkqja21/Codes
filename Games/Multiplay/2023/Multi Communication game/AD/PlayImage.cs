using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayImage : MonoBehaviour
{
    [SerializeField]
    Material material;

    void Start()
    {
        string imageFilePath = Application.persistentDataPath + "/tempImage.png";
        Texture texture = LoadTextureFromFile(imageFilePath);

        if (File.Exists(imageFilePath) && texture != null)
        {
            material.mainTexture = texture;

            material.mainTextureScale = new Vector2(1, 1);
            material.mainTextureOffset = new Vector2(0, 0);
        }
        else
        {
            Debug.Log("이미지 파일 존재하지 않음");
        }        
    }


    Texture2D LoadTextureFromFile(string filePath)
    {
        byte[] imageData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2,2);
        if (texture.LoadImage(imageData))
        {
            return texture;
        }
        else
        {
            Debug.LogError("Failed to load image from file: " + filePath);
            return null;
        }
    }
}
