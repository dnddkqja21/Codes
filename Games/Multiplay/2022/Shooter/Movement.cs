using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviourPunCallbacks, IPunObservable
{
    // Ʈ�������� ĳ���ؼ� ����ϴ� ���� �� �����
    // �ֱٿ��� ����ȭ�� �Ǿ� �׳� transform���� �����ϴ� ���� ��õ�Ѵ�.
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

    // Ű���� �Է°� ����
    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");

    // �����
    PhotonView pv;

    // �ó׸ӽ� ī�޶�
    CinemachineVirtualCamera virtualCamera;

    // OnPhotonSerializeView �� �̿��� ��� (���� ������)
    // ������ ��ġ�� ȸ����
    Vector3 recivePos;
    Quaternion reciveRot;

    // ���ŵ� ��ǥ���� �̵� �� ȸ�� �ΰ���
    public float damping = 10f;

    void Start()
    {       
        //transform = GetComponent<Transform>();
        
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        // ���� �䰡 �ڽ��� ��� �ó׸ӽ� ���� ī�޶� ����
        if(pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }

        // ������ �ٴ��� ���ΰ� ��ġ�� �������� �����Ѵ�.
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
            // ���ŵ� ��ǥ�� ���� �̵� ó��
            transform.position = Vector3.Lerp(transform.position, recivePos, Time.deltaTime * damping);

            // ���ŵ� ȸ�������� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, reciveRot, Time.deltaTime * damping);
        }
    }

    void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        // �̵��� ���� ���� ���
        Vector3 moveDir = (cameraForward * v) + (cameraRight * h);
        moveDir.Set(moveDir.x, 0f, moveDir.z);

        // ĳ���� ��Ʈ�ѷ� ���� ����
        controller.SimpleMove(moveDir * moveSpeed);

        // �ִ�
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
        // ������ �ٴڿ� �浹�� ��ǥ ����
        hitPoint = ray.GetPoint(enter);

        // ȸ���ؾ� �� ���� ����
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0f;

        // ĳ������ ȸ���� ����
        transform.localRotation = Quaternion.LookRotation(lookDir);
    }

    // ���� ������ �̵� �� ȸ��
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // �ڽ��� ���� ĳ������ ��� �ڽ��� �����͸� �ٸ� ��Ʈ��ũ �������� �۽�
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
