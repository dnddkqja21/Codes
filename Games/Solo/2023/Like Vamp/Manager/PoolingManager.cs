using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    [Header("프리펩")]
    public GameObject[] prefabs;
    [Header("몬스터 스포너")]
    public Transform spawner;

    List<GameObject>[] pools;

    void Awake()
    {
        // 풀 배열 초기화
        pools = new List<GameObject>[prefabs.Length];

        // 배열 안의 리스트 초기화
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject temp = null;

        // 비활성화된 오브젝트가 있다면 선택하여 활성화
        foreach(var item in pools[index])
        {
            if(!item.activeSelf)
            {
                temp = item;
                temp.SetActive(true);
                break;
            }
        }

        // 모든 풀을 돌았으나 전부 활성화된 상태라면 생성
        if (!temp)
        {
            temp = Instantiate(prefabs[index], spawner.transform);
            pools[index].Add(temp);
        }

        return temp;
    }
}
