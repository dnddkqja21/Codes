using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D -> 2D로 이동할 URL
/// </summary>

public class SaveURL : MonoBehaviour
{
    static SaveURL instance;
    public static SaveURL Instance { get { return instance; } }

    public string URL;

    void Awake()
    {
        if (instance == null) 
        { 
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetURL()
    {
        URL = EnumToData.Instance.BuildingNameToURL(GameManager.Instance.buildingName);
    }
}
