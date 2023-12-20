using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject turtle;
    public GameObject skel;
    public GameObject orc;
    public GameObject mage;

    public Transform[] turtleRespawnPos;
    
    public Transform[] skelRespawnPos;
    
    public Transform[] orcRespawnPos;
    
    public Transform[] mageRespawnPos;
    

    const int maxTurtleCount = 5;
    const int maxSkelCount = 5;
    const int maxOrcCount = 5;
    const int maxMageCount = 5;

    public List<GameObject> turtleCount = new List<GameObject>();
    public List<GameObject> skelCount = new List<GameObject>();
    public List<GameObject> orcCount = new List<GameObject>();
    public List<GameObject> mageCount = new List<GameObject>();

    float spwanTime = 7f;
    float curTime;

    public static RespawnManager instanceRespawn;

    public bool[] isSpawnTurtle;
    public bool[] isSpawnSkel;
    public bool[] isSpawnOrc;
    public bool[] isSpawnMage;

    void Start()
    {
        instanceRespawn = this;

        isSpawnTurtle = new bool[turtleRespawnPos.Length];
        for (int i = 0; i < isSpawnTurtle.Length; i++)
        {
            isSpawnTurtle[i] = false;
        }

        isSpawnSkel = new bool[skelRespawnPos.Length];
        for(int i = 0; i < isSpawnSkel.Length; i++)
        {
            isSpawnSkel[i] = false;
        }

        isSpawnOrc = new bool[orcRespawnPos.Length];
        for (int i = 0; i < isSpawnOrc.Length; i++)
        {
            isSpawnOrc[i] = false;
        }

        isSpawnMage = new bool[mageRespawnPos.Length];
        for (int i = 0; i < isSpawnMage.Length; i++)
        {
            isSpawnMage[i] = false;
        }
    }

    
    void Update()
    {
        if(curTime >= spwanTime)
        {
            TurtleCount();
            SkelCount();
            OrcCount();
            MageCount();
        }
        curTime += Time.deltaTime;
    }

    void TurtleCount()
    {
        if (turtleCount.Count < maxTurtleCount)
        {
            int rand = Random.Range(0, turtleRespawnPos.Length);
            if (!isSpawnTurtle[rand])
            {
                SpawnTurtle(rand);
            }
        }
    }

    void SkelCount()
    {
        if (skelCount.Count < maxSkelCount)
        {
            int rand = Random.Range(0, skelRespawnPos.Length);
            if (!isSpawnSkel[rand])
            {
                SpawnSkel(rand);
            }
        }
    }

    void OrcCount()
    {
        if (orcCount.Count < maxOrcCount)
        {
            int rand = Random.Range(0, orcRespawnPos.Length);
            if (!isSpawnOrc[rand])
            {
                SpawnOrc(rand);
            }
        }
    }

    void MageCount()
    {
        if (mageCount.Count < maxMageCount)
        {
            int rand = Random.Range(0, mageRespawnPos.Length);
            if (!isSpawnMage[rand])
            {
                SpawnMage(rand);
            }
        }
    }

    void SpawnTurtle(int _random)
    {
        curTime = 0;
        GameObject tmp = Instantiate(turtle, turtleRespawnPos[_random]);
        turtleCount.Add(tmp);
        isSpawnTurtle[_random] = true;
    }

    void SpawnSkel(int _random)
    {
        curTime = 0;
        GameObject tmp = Instantiate(skel, skelRespawnPos[_random]);
        skelCount.Add(tmp);
        isSpawnSkel[_random] = true;
    }

    void SpawnOrc(int _random)
    {
        curTime = 0;
        GameObject tmp = Instantiate(orc, orcRespawnPos[_random]);
        orcCount.Add(tmp);
        isSpawnOrc[_random] = true;
    }

    void SpawnMage(int _random)
    {
        curTime = 0;
        GameObject tmp = Instantiate(mage, mageRespawnPos[_random]);
        mageCount.Add(tmp);
        isSpawnMage[_random] = true;
    }
}
