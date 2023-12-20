using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    static ShootingManager instance = null;
    public static ShootingManager Instance { get { return instance; } }

    [SerializeField]
    GameObject laserPrefab;

    int poolSize = 5;
    List<GameObject> laserPool = new List<GameObject>();

    public Collider[] laserWalls;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        InitObjectPool();
    }  

    void InitObjectPool()
    {     
        for (int i = 0; i < poolSize; i++)
        {
            GameObject laser = Instantiate(laserPrefab, transform);            
            laser.SetActive(false);
            laserPool.Add(laser);
        }
    }

    public GameObject GetLaser()
    {
        for (int i = 0; i < laserPool.Count; i++)
        {
            if (!laserPool[i].activeInHierarchy)
            {
                return laserPool[i];
            }
        }

        // 모든 레이저가 사용 중이면 새로운 레이저를 생성하고 풀에 추가
        GameObject newLaser = Instantiate(laserPrefab, transform);
        laserPool.Add(newLaser);
        return newLaser;
    } 
    
    public void TriggerOnOff(bool isOn)
    {
        foreach (Collider col in laserWalls)
        {
            col.isTrigger = isOn;
        }
    }
}
