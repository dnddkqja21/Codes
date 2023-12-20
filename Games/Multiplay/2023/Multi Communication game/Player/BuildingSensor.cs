using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 빌딩 센서
/// 빌딩이 앞에 있다면 감속, 오브젝트 충돌 무시 현상 때문
/// </summary>

public class BuildingSensor : MonoBehaviourPun
{
    MoveJoystick joystick;
    NavMeshAgent agent;
    PhotonView pv;
    const float SPEED_LIMIT = 3f;
    readonly Vector3 Gallery = new Vector3(-144f, 0.3f, -151f);
    readonly Vector3 Exit = new Vector3(0, 0.3f, 4.4f);
    readonly Vector3 Observatory = new Vector3(0, 6.3f, 4.4f);

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pv = GetComponent<PhotonView>();
    //    joystick = FindObjectOfType<MoveJoystick>();
    //    PhotonManagerWorld.Instance.PlayerCreated += SetPlayer;
    }

    // 접속 시 모든 플레이어에게 현재 좌표를 동기화한다. (전시관 때문에 생긴 것)
    //void SetPlayer()
    //{
    //    //pv.RPC("SetPosition", RpcTarget.All, transform.position);
    //}

    void OnTriggerEnter(Collider other)
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;
        
        if(other.CompareTag("InteractionBuilding"))
        {
            GameManager.Instance.buildingName = other.gameObject.name;
        }

        if (other.CompareTag("Gallery"))
        {
            pv.RPC("Teleport", RpcTarget.All, Gallery);
        }
        else if (other.CompareTag("Exit"))
        {
            pv.RPC("Teleport", RpcTarget.All, Exit);
        }
        else if (other.CompareTag("Observatory"))
        {
            pv.RPC("Teleport", RpcTarget.All, Observatory);
        }
    }

    [PunRPC]
    void Teleport(Vector3 pos)
    {
        //StartCoroutine(AgentEnable(pos));        
        AgentWarp(pos);

        if (!GetComponent<PhotonView>().IsMine)
            return;
        SoundManager.Instance.PlaySFX(SFX.OpenDoor);
        UIManager.Instance.fadeOut.SetActive(true);
    }

//    [PunRPC]
//    void SetPosition(Vector3 pos)
//    {
//        StartCoroutine(AgentEnable(pos));        
//    }

//    IEnumerator AgentEnable(Vector3 pos)
//    {
//#if UNITY_ANDROID && UNITY_IOS
//        joystick.enabled = false;
//#endif
//        agent.enabled = false;
//        yield return new WaitForSeconds(0.1f);
//        transform.position = pos;
//        yield return new WaitForSeconds(0.2f);
//        //agent.enabled = true;
//#if UNITY_ANDROID && UNITY_IOS
//        joystick.enabled = true;
//#endif
//    }

    // 에이전트의 좌표 이동 함수
    void AgentWarp(Vector3 pos)
    {
        agent.Warp(pos);
    }


    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward, Color.red, SPEED_LIMIT);

        if (!GetComponent<PhotonView>().IsMine)
            return;
        
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, SPEED_LIMIT))
        {
            if(hit.collider.CompareTag("Building"))
            {                
                //Debug.Log("3유닛 앞에 빌딩이 있다.");
                PlayerAttributes player = GetComponent<PlayerAttributes>();
                player.speed = SPEED_LIMIT;
            }
        }        
    }
}
