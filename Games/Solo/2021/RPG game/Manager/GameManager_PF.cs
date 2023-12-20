using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager_PF : MonoBehaviour
{
    public ResourceManager_PF resourceManager;  // ���� �Ŵ����� ���ҽ� �Ŵ����� �˰��ִ�.
    public ObjectPool_PF pool;


    List<Player_PF> playerList = new List<Player_PF>();

    const int MAX_PLAYER_COUNT = 1; // �÷��̾� ĳ������ �ߺ� ���� ����

    public Player_PF player;

    Vector3 startingPoint = new Vector3(-216f, 1.7f, 27f);

    public Player_PF PLAYER   // �ٸ� �������� ����� �� �ְԲ� �ۺ� ������Ƽ
    { get { return player; } }

    void Start()
    {
        //CreatePlayer();
    }

    void Update()
    {   

        if (Input.GetKeyDown(KeyCode.F2))
        {
            //pool.CreateMob();
        }
    }



    void CreatePlayer()
    {
        if (playerList.Count >= MAX_PLAYER_COUNT)   // �÷��̾� ĳ������ �ߺ� ���� ����
            return;

        GameObject _obj = resourceManager.GetPlayerRC();

        if (_obj != null)
        {
            GameObject obj = Instantiate<GameObject>(_obj);
            Player_PF tmp = obj.GetComponent<Player_PF>();            
            tmp.name = "PLAYER";
            player = tmp;
            tmp.transform.localPosition = startingPoint;
            tmp.gameObject.AddComponent<NavMeshAgent>();
            playerList.Add(tmp);
        }
    }
}
