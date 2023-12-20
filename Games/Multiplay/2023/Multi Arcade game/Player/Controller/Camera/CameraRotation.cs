using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 플랫폼에 따른 카메라 회전
/// </summary>

public class CameraRotation : MonoBehaviour
{    
    CameraZoomming cameraZoom;
    PlayerMoveOnMobile moveJoystick;

    float xRotate, yRotate, xRotateMove, yRotateMove;

    const float pcRotateSpeed = 300.0f;
    const float mobileRotateSpeedOneFinger = 70.0f;
    const float mobileRotateSpeedTwoFinger = 10.0f;
    const float Limit_High = 85f;
    const float Limit_Low = -30f;

    void Start()
    {
        cameraZoom = UIManagerWorld.Instance.cameraZoom;
        moveJoystick = UIManagerWorld.Instance.moveJoystick;
    }

    void LateUpdate()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE || UNITY_EDITOR_OSX
        if (Input.GetMouseButton(1) && !TeleportManager.Instance.isTeleport)
        {
            xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * pcRotateSpeed;
            yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * pcRotateSpeed;

            yRotate = transform.eulerAngles.y + yRotateMove;
            xRotate = xRotate + xRotateMove;
            xRotate = Mathf.Clamp(xRotate, Limit_Low, Limit_High); // 상하 각도 제한

            transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
        }

#elif UNITY_ANDROID || UNITY_IOS
        // 이동 중이라면 터치가 2개
        if(moveJoystick.isMoving && Input.touchCount == 2 && !TeleportManager.Instance.isTeleport)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            if (secondTouch.phase == TouchPhase.Moved)
            {
                // 터치에 의한 회전 계산
                float xRotateMove = -secondTouch.deltaPosition.y * Time.deltaTime * mobileRotateSpeedTwoFinger;
                float yRotateMove = secondTouch.deltaPosition.x * Time.deltaTime * mobileRotateSpeedTwoFinger;
                               
                float yRotate = transform.eulerAngles.y + yRotateMove;
                float xRotate = transform.eulerAngles.x + xRotateMove;
                xRotate = Mathf.Clamp(xRotate, Limit_Low, Limit_High);

                transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
            }
        }
        // 이동 중 아닐 때
        else if(!moveJoystick.isMoving && !cameraZoom.isZoomming && !TeleportManager.Instance.isTeleport)
        {
            // UI 위에 터치가 있다면 리턴 (모바일)
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                // 터치가 2개일 때 화면이 의도치 않게 회전하는 오작동을 일으킬 수 있음.
                if (Input.touchCount == 2)
                    return;

                if (Input.GetMouseButton(0))
                {
                    xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * mobileRotateSpeedOneFinger;
                    yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * mobileRotateSpeedOneFinger;

                    yRotate = transform.eulerAngles.y + yRotateMove;
                    xRotate = xRotate + xRotateMove;
                    xRotate = Mathf.Clamp(xRotate, Limit_Low, Limit_High);

                    transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
                }
            }
        }
#endif
    }
}
