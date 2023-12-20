using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveGameObject : MonoBehaviour
{
    [Header("��Ȱ��ȭ ��ų ������Ʈ")]
    public GameObject[] gameObjects;

    public void Inactive()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(false);
        }
    }
}
