using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveGameObject : MonoBehaviour
{
    [Header("비활성화 시킬 오브젝트")]
    public GameObject[] gameObjects;

    public void Inactive()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(false);
        }
    }
}
