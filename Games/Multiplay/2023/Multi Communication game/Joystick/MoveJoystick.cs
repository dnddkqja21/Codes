using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 이동 조이스틱
/// </summary>

public class MoveJoystick : MonoBehaviour
{    
    PlayerAttributes player;

    [Header("카메라 암")]
    [SerializeField]
    Transform cameraArm;

    //DynamicJoystick joystick;
    FixedJoystick joystick;
    CameraZoom cameraZoom;

    public bool isMoving;
    
    float acceleration = 0.1f;

    const float MIN_SPEED = 3f;
    const float MAX_SPEED = 7f;

    NavMeshAgent agent;

    void Start()
    {
        joystick = GetComponent<FixedJoystick>();
        cameraZoom = cameraArm.GetComponentInChildren<CameraZoom>();        
        PhotonManagerWorld.Instance.PlayerCreated += SetPlayer;
    }

    void SetPlayer()
    {
        player = PhotonManagerWorld.Instance.player.GetComponent<PlayerAttributes>();
        agent = PhotonManagerWorld.Instance.player.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (cameraZoom.isZooming || player == null)
            return;

        Vector2 joystickDir = joystick.Direction;
        if (joystickDir == Vector2.zero)
        {
            if(agent.enabled)
            {
                agent.enabled = false;
                Debug.Log("에이전트 비활성");
            }

            isMoving = false;
            acceleration = 0.1f;
            player.speed = MIN_SPEED;
            return;
        }

        if(!agent.enabled)
        {
            agent.enabled = true;     
            Debug.Log("에이전트 활성");
        }

        isMoving = true;
        acceleration += Time.deltaTime;        
        player.speed += acceleration * Time.deltaTime * 0.5f;
        player.speed = Mathf.Clamp(player.speed, MIN_SPEED, MAX_SPEED);

        // 삼각함수에 의해 코드 정리
        float thetaEuler = Mathf.Acos(joystickDir.y / joystickDir.magnitude) *
                            (180 / Mathf.PI) * Mathf.Sign(joystickDir.x);

        Vector3 moveAngle = Vector3.up * (cameraArm.transform.rotation.eulerAngles.y + thetaEuler);
        player.transform.rotation = Quaternion.Euler(moveAngle);
        player.transform.Translate(Vector3.forward * Time.deltaTime * player.speed);
#endif
    }
}
