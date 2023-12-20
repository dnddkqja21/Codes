using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

/// <summary>
/// PC버전 이동
/// 네비메쉬
/// </summary>

public class PlayerMoveOnPC : MonoBehaviourPunCallbacks
{
    PhotonView photon;
    NavMeshAgent agent;
    GameObject destinationArrow;
    Vector3 target;


#if UNITY_EDITOR_WIN || UNITY_STANDALONE || UNITY_EDITOR_OSX    // pc 클릭 이동

    void Start()
    {
        agent           = GetComponent<NavMeshAgent>();        
        //originSpeed     = agent.speed;
        photon          = GetComponent<PhotonView>();

        // 네비 에이전트 회전 제한
        agent.updateRotation = false;
    }

    void Update()
    {        
        if (Input.GetMouseButtonDown(0))
        {            
            // 로컬 체크
            if (!photon.IsMine)
                return;

            // 마우스가 UI위에 있을 시 이동 X
            if (EventSystem.current.IsPointerOverGameObject())
                return;            

            // 클릭한 곳이 플레이어일 때 이동 X
            int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
            Ray playerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit playerHit;
            if(Physics.Raycast(playerRay, out playerHit, Mathf.Infinity, playerLayerMask))
            {
                if (playerHit.collider.CompareTag("Player"))
                {
                    return;
                }
            }
            // 클릭한 곳이 액자일 때 이동 X
            int frameLayerMask = 1 << LayerMask.NameToLayer("PhotoFrame");
            Ray frameRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit frameHit;
            if (Physics.Raycast(frameRay, out frameHit, Mathf.Infinity, frameLayerMask))
            {
                return;
            }

            // 목표 지점 화살표
            if (destinationArrow  != null)
                destinationArrow.SetActive(false);

            //newSpeed += Time.deltaTime *0.1f;
            int groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.Log("마우스 포지션 : " + Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
            {                   
                //target = hit.point;
                // 정확한 계산을 위해 네비메쉬 샘플 포지션 사용
                NavMeshHit navMeshHit;
                if(NavMesh.SamplePosition(hit.point, out navMeshHit, 1f, NavMesh.AllAreas))
                {
                    agent.enabled = true;

                    target = navMeshHit.position;
                    agent.SetDestination(target);
                    //Debug.Log("클릭한 곳 좌표 : " + target);
                    if(destinationArrow == null)
                    {
                        destinationArrow = Instantiate(GameManager.Instance.destinationPrefab);
                    }
                    destinationArrow.SetActive(true);
                    destinationArrow.transform.position = new Vector3(target.x, target.y + 0.7f, target.z);
                    GameManager.Instance.destinationPosY = destinationArrow.transform.position.y;
                }
            }
        }
        // 네비 메쉬 회전값 대신 계산된 회전 값 사용
        Vector3 lookRotation = agent.steeringTarget - transform.position;
        if(lookRotation !=  Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), 5f * Time.deltaTime);
        }
    }
#endif

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            agent.enabled = false;
            other.gameObject.SetActive(false);
        }
    }
}
