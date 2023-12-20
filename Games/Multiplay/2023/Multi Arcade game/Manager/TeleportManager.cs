using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TeleportManager : MonoBehaviour
{
    static TeleportManager instance = null;
    public static TeleportManager Instance { get { return instance; } }

    [SerializeField]
    GameObject portal;  

    FollowPlayer followPlayer;
    Transform player;
    NavMeshAgent agent;
    Animator animator;
    PhotonView pv;

    float scaleSpeed = 1f;
    float forwardSpeed = 1.5f;
    Vector3 originPortalAngle;
    Vector3 targetCameraAngle = new Vector3(0, -90, 0);
    public bool isTeleport;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        PhotonManager.Instance.PlayerCreated += SetPlayer;
        originPortalAngle = portal.transform.localEulerAngles;
        followPlayer = FindObjectOfType<FollowPlayer>();        
    }

    //void Update()
    //{
    //    // test
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Teleport();
    //    }
    //}

    void SetPlayer()
    {
        player = PhotonManager.Instance.player.transform;
        agent = player.GetComponent<NavMeshAgent>();
        animator = player.GetComponent<Animator>();
        pv = player.GetComponent<PhotonView>();
    }

    public void Teleport(Vector3 teleportPos = default(Vector3))
    {
        if (!pv.IsMine)
            return;

        if(agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
        GameManager.Instance.SetIdle();

        // 텔레포트 시작
        isTeleport = true;
        StartCoroutine(SmoothRotation());
        UIManagerWorld.Instance.untouchable.SetActive(true);
        followPlayer.player = null;
        player.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
        portal.transform.position = player.transform.position + new Vector3(-1, 1, 0);
        StartCoroutine(ScaleUp(portal.transform, 1f, ()=> Floating(teleportPos)));
        SoundManager.Instance.PlaySFX(SFX.PortalOpen);
    }

    IEnumerator ScaleUp(Transform obj, float scale, Action func = null)
    {        
        while (obj.localScale.z < scale)
        {
            obj.localScale += Vector3.one * Time.deltaTime * scaleSpeed;
            yield return null;
        }
        obj.localScale = Vector3.one * scale;

        if (func != null)
        {
            func.Invoke();
        }
    }

    void Floating(Vector3 teleportPos = default(Vector3))
    {
        animator.SetTrigger(Config.Floating);
        UIManagerWorld.Instance.fadeIn.SetActive(true);        
        StartCoroutine(GoForward(player.transform, teleportPos));
    }

    IEnumerator GoForward(Transform obj, Vector3 teleportPos = default(Vector3))
    {
        yield return new WaitForSeconds(1);

        while (obj.localScale.z > 0)
        {
            obj.position += new Vector3(-Time.deltaTime * forwardSpeed, Time.deltaTime * forwardSpeed * 1.5f, 0);
            obj.localScale -= Vector3.one * Time.deltaTime * scaleSpeed;
            yield return null;
        }
        obj.localScale = Vector3.zero;

        StartCoroutine(ScaleDown(portal.transform, TeleportEffect(teleportPos)));
    }

    IEnumerator ScaleDown(Transform obj, IEnumerator cor = null)
    {
        SoundManager.Instance.PlaySFX(SFX.PortalClose);
        while (obj.localScale.z > 0)
        {
            obj.localScale -= Vector3.one * Time.deltaTime * scaleSpeed;
            yield return null;
        }
        obj.localScale = Vector3.zero;

        if (cor != null)
        {
            StartCoroutine(cor);
        }
    }

    IEnumerator TeleportEffect(Vector3 teleportPos = default(Vector3))
    {
        UIManagerWorld.Instance.loadingPanel.SetActive(true);
        yield return new WaitForSeconds(2.5f);

        UIManagerWorld.Instance.loadingPanel.SetActive(false);
        UIManagerWorld.Instance.fadeIn.SetActive(false);
        UIManagerWorld.Instance.fadeOut.SetActive(true);
        yield return new WaitForSeconds(1f);

        // 플레이어와 카메라 연결, 텔레포트 
        isTeleport = false;
        followPlayer.player = player.transform;

        player.transform.position = PositionSelection(teleportPos);
        //Debug.Log("출력 좌표 : " + PositionSelection(teleportPos));

        // 포탈의 포지션, 각도 변경, 플레이어의 위치 변경
        portal.transform.position = player.transform.position + Vector3.up * 1.5f;
        portal.transform.localEulerAngles = new Vector3(65f, 20f, 90f);
        player.transform.position += Vector3.up * 1.5f;

        StartCoroutine(ScaleUp(portal.transform, 1f));
        SoundManager.Instance.PlaySFX(SFX.PortalOpen);

        player.transform.localEulerAngles = new Vector3(0, -30f, 0);
        animator.SetTrigger(Config.Falling);
        yield return new WaitForSeconds(1f);

        StartCoroutine(PositionDown(player.transform));
        StartCoroutine(ScaleUp(player.transform, 0.5f));
        yield return new WaitForSeconds(2.5f);

        animator.SetTrigger(Config.Fall);
        agent.enabled = true;
        UIManagerWorld.Instance.untouchable.SetActive(false);
        StartCoroutine(ScaleDown(portal.transform, InitPortalRot()));
    }    

    IEnumerator PositionDown(Transform obj)
    {
        float t = 0;
        while (t < 0.7f)
        {
            obj.Translate(0, -Time.deltaTime, 0);
            t += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator InitPortalRot()
    {
        yield return null;
        portal.transform.localEulerAngles = originPortalAngle;
    }

    // 카메라의 각을 부드럽게 초기화
    IEnumerator SmoothRotation()
    {
        Quaternion fromRotation = followPlayer.transform.localRotation;
        Quaternion toRotation = Quaternion.Euler(targetCameraAngle);
        float elapsedTime = 0f;

        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            followPlayer.transform.localRotation = Quaternion.Slerp(fromRotation, toRotation, elapsedTime / 1);
            yield return null;
        }
        followPlayer.transform.localRotation = toRotation;
    }

    // 랜덤 좌표 
    Vector3 PositionSelection(Vector3 teleportPos = default(Vector3))
    {
        float x, z;

        // 파라미터 없는 경우 랜덤 좌표
        if(teleportPos == Vector3.zero)
        {
            x = Random.Range(-90, 30);
            z = Random.Range(-50, 50);

            NavMeshHit hit;
            bool isPointValid = NavMesh.SamplePosition(new Vector3(x, 0, z), out hit, 0.1f, NavMesh.AllAreas);
            if (isPointValid)
            {
                teleportPos = new Vector3(x, 0, z);
                Debug.Log("텔레포트 좌표 : " + teleportPos);
                return teleportPos;                
            }
            else
            {
                Debug.Log("유효하지 않은 좌표, 재귀 호출");
                return PositionSelection();
            }
        }
        return teleportPos;
    }
}
