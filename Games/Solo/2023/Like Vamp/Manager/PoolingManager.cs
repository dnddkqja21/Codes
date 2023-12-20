using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    [Header("������")]
    public GameObject[] prefabs;
    [Header("���� ������")]
    public Transform spawner;

    List<GameObject>[] pools;

    void Awake()
    {
        // Ǯ �迭 �ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];

        // �迭 ���� ����Ʈ �ʱ�ȭ
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject temp = null;

        // ��Ȱ��ȭ�� ������Ʈ�� �ִٸ� �����Ͽ� Ȱ��ȭ
        foreach(var item in pools[index])
        {
            if(!item.activeSelf)
            {
                temp = item;
                temp.SetActive(true);
                break;
            }
        }

        // ��� Ǯ�� �������� ���� Ȱ��ȭ�� ���¶�� ����
        if (!temp)
        {
            temp = Instantiate(prefabs[index], spawner.transform);
            pools[index].Add(temp);
        }

        return temp;
    }
}
