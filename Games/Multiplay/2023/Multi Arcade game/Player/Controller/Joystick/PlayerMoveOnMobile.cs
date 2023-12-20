using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 플레이어 이동 모바일 버전
/// </summary>

public class PlayerMoveOnMobile : MonoBehaviour
{
    Transform cameraArm;
    Transform player;
    DynamicJoystick joystick;
    CameraZoomming cameraZoom;
    PhotonView pv;
    NavMeshAgent agent;

    public bool isMoving;
    float moveSpeed;

    void Start()
    {
        cameraArm = UIManagerWorld.Instance.cameraArm;
        joystick = GetComponent<DynamicJoystick>();
        cameraZoom = cameraArm.GetComponentInChildren<CameraZoomming>();
        PhotonManager.Instance.PlayerCreated += SetPlayer;
    }

#if UNITY_ANDROID
    void Update()
    {
        if (cameraZoom.isZoomming || player == null)
            return;

        // 로컬 체크
        if (!pv.IsMine)
            return;

        Vector2 joystickDir = joystick.Direction;
        if (joystickDir == Vector2.zero)
        {
            isMoving = false;
            agent.enabled = false;
            //GameManager.Instance.SetIdle();
            return;
        }

        isMoving = true;
        agent.enabled = true;
        //GameManager.Instance.SetAni();

        // 삼각함수에 의해 코드 정리
        float thetaEuler = Mathf.Acos(joystickDir.y / joystickDir.magnitude) *
                            (180 / Mathf.PI) * Mathf.Sign(joystickDir.x);

        Vector3 moveAngle = Vector3.up * (cameraArm.transform.rotation.eulerAngles.y + thetaEuler);
        player.transform.rotation = Quaternion.Euler(moveAngle);

        moveSpeed = GameManager.Instance.agent.speed;
        player.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }
#endif

    void SetPlayer()
    {
        player = PhotonManager.Instance.player.transform;
        pv = player.GetComponent<PhotonView>();
        agent = player.GetComponent<NavMeshAgent>();
    }
}
