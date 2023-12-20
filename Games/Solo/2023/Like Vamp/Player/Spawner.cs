using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System을 사용하면 Random 및 Mathf 등 라이브러리의 사용이 모호해져 명시해주어야 한다.
// using Random = UnityEngine.Random;
// Serializable이 필요한 곳에서 System을 호출하자

public class Spawner : MonoBehaviour
{
    /* 리스폰 로직 변경
    [Header("리스폰 간격")]
    public float respawnTime = 2f;
    */

    public SpawnData[] spawnDatas;
    public float levelTime;

    int level;
    float timer;

    // 강의에서는 스폰 포인트 오브젝트를 사방으로 둘러 그곳에서 생성하지만 비효율
    // 스폰 지점을 크게 상하좌우로 나눠서 랜덤 지점에서 생성
    Vector3[] spawnPos = new Vector3[4];

    // 스폰 지점을 화면 크기로 정할 수 있으면 좀 더 나은 코드가 될 것 같음.
    float limitX = 10f;
    float limitY = 6f;

    void Start()
    {
        spawnPos[0] = new Vector3(Random.Range(-limitX, limitX), limitY, 0);
        spawnPos[1] = new Vector3(Random.Range(-limitX, limitX), -limitY, 0);
        spawnPos[2] = new Vector3(-limitX, Random.Range(-limitY, limitY), 0);
        spawnPos[3] = new Vector3(limitX, Random.Range(-limitY, limitY), 0);

        levelTime = GameManager.Instance.maxGameTime / spawnDatas.Length;
    }

    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;
        //Debug.Log(timer);

        // 소숫점 아래 버리고 int형변환
        // 임시 : 데이터의 길이 -1 만큼만 레벨 올라감 인덱스 에러 피하기 위함. 즉, 레벨4까지만 올라감
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / levelTime), spawnDatas.Length -1);
        //Debug.Log(level);
        Respawn();
    }

    void Respawn()
    {
        // 레벨에 따른 리스폰 타이밍 상이
        if (timer > spawnDatas[level].spawnTime)
        {
            GameObject temp = GameManager.Instance.poolingManager.Get(0);
                                // 에너미 프리펩을 하나로 묶기 전 코드
                                //Get(Random.Range(0, GameManager.Instance.poolingManager.enemys.Length));

            temp.transform.position =   GameManager.Instance.player.transform.position + 
                                        spawnPos[Random.Range(0, spawnPos.Length)];

            // 리스폰될 때 에너미를 레벨에 따라 초기화
            temp.GetComponent<Enemy>().Init(spawnDatas[level]);

            timer = 0;
        }
    }
}

// 직렬화 (인스펙터에서 노출)
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    
    // 에너미 클래스에서 사용
    public int spriteType;
    public int health;
    public float moveSpeed;
}
