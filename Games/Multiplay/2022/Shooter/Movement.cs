using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviourPunCallbacks, IPunObservable
{
    // 트랜스폼을 캐싱해서 사용하는 것은 옛 방식임
    // 최근에는 최적화가 되어 그냥 transform으로 접근하는 것을 추천한다.
    //new Transform transform;

    // Cache
    CharacterController controller;
    Animator animator;
    new Camera camera;

    // Ray Casting
    Plane plane;
    Ray ray;
    Vector3 hitPoint;

    [SerializeField]
    float moveSpeed = 10f;

    // 키보드 입력값 연결
    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");

    // 포톤뷰
    PhotonView pv;

    // 시네머신 카메라
    CinemachineVirtualCamera virtualCamera;

    // OnPhotonSerializeView 를 이용한 방법 (더욱 정교함)
    // 수정된 위치와 회전값
    Vector3 recivePos;
    Quaternion reciveRot;

    // 수신된 좌표로의 이동 및 회전 민감도
    public float damping = 10f;

    void Start()
    {       
        //transform = GetComponent<Transform>();
        
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        // 포톤 뷰가 자신일 경우 시네머신 가상 카메라 연결
        if(pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }

        // 가상의 바닥을 주인공 위치를 기준으로 생성한다.
        plane = new Plane(transform.up, transform.position);
    }

    void Update()
    {
        if (pv.IsMine)
        {            
            Move();
            Turn();
        }
        else
        {
            // 수신된 좌표로 보간 이동 처리
            transform.position = Vector3.Lerp(transform.position, recivePos, Time.deltaTime * damping);

            // 수신된 회전값으로 보간
            transform.rotation = Quaternion.Slerp(transform.rotation, reciveRot, Time.deltaTime * damping);
        }
    }

    void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        // 이동할 방향 벡터 계산
        Vector3 moveDir = (cameraForward * v) + (cameraRight * h);
        moveDir.Set(moveDir.x, 0f, moveDir.z);

        // 캐릭터 컨트롤러 심플 무브
        controller.SimpleMove(moveDir * moveSpeed);

        // 애니
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);

        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }

    void Turn()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        float enter = 0;

        plane.Raycast(ray, out enter);
        // 가상의 바닥에 충돌한 좌표 추출
        hitPoint = ray.GetPoint(enter);

        // 회전해야 할 방향 벡터
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0f;

        // 캐릭터의 회전값 지정
        transform.localRotation = Quaternion.LookRotation(lookDir);
    }

    // 더욱 정교한 이동 및 회전
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 자신의 로컬 캐릭터인 경우 자신의 데이터를 다른 네트워크 유저에게 송신
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            recivePos = (Vector3)stream.ReceiveNext();
            reciveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
