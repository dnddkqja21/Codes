using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager_PF : MonoBehaviour
{
    public ResourceManager_PF resourceManager;  // 게임 매니저는 리소스 매니저를 알고있다.
    public ObjectPool_PF pool;


    List<Player_PF> playerList = new List<Player_PF>();

    const int MAX_PLAYER_COUNT = 1; // 플레이어 캐릭터의 중복 생성 제한

    public Player_PF player;

    Vector3 startingPoint = new Vector3(-216f, 1.7f, 27f);

    public Player_PF PLAYER   // 다른 곳에서도 사용할 수 있게끔 퍼블릭 프로퍼티
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
        if (playerList.Count >= MAX_PLAYER_COUNT)   // 플레이어 캐릭터의 중복 생성 제한
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
