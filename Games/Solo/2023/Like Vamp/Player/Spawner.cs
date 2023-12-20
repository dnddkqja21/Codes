using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System�� ����ϸ� Random �� Mathf �� ���̺귯���� ����� ��ȣ���� ������־�� �Ѵ�.
// using Random = UnityEngine.Random;
// Serializable�� �ʿ��� ������ System�� ȣ������

public class Spawner : MonoBehaviour
{
    /* ������ ���� ����
    [Header("������ ����")]
    public float respawnTime = 2f;
    */

    public SpawnData[] spawnDatas;
    public float levelTime;

    int level;
    float timer;

    // ���ǿ����� ���� ����Ʈ ������Ʈ�� ������� �ѷ� �װ����� ���������� ��ȿ��
    // ���� ������ ũ�� �����¿�� ������ ���� �������� ����
    Vector3[] spawnPos = new Vector3[4];

    // ���� ������ ȭ�� ũ��� ���� �� ������ �� �� ���� �ڵ尡 �� �� ����.
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

        // �Ҽ��� �Ʒ� ������ int����ȯ
        // �ӽ� : �������� ���� -1 ��ŭ�� ���� �ö� �ε��� ���� ���ϱ� ����. ��, ����4������ �ö�
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / levelTime), spawnDatas.Length -1);
        //Debug.Log(level);
        Respawn();
    }

    void Respawn()
    {
        // ������ ���� ������ Ÿ�̹� ����
        if (timer > spawnDatas[level].spawnTime)
        {
            GameObject temp = GameManager.Instance.poolingManager.Get(0);
                                // ���ʹ� �������� �ϳ��� ���� �� �ڵ�
                                //Get(Random.Range(0, GameManager.Instance.poolingManager.enemys.Length));

            temp.transform.position =   GameManager.Instance.player.transform.position + 
                                        spawnPos[Random.Range(0, spawnPos.Length)];

            // �������� �� ���ʹ̸� ������ ���� �ʱ�ȭ
            temp.GetComponent<Enemy>().Init(spawnDatas[level]);

            timer = 0;
        }
    }
}

// ����ȭ (�ν����Ϳ��� ����)
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    
    // ���ʹ� Ŭ�������� ���
    public int spriteType;
    public int health;
    public float moveSpeed;
}
