using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 카메라 회전
/// 마우스 축 입력에 의한 카메라 회전
/// </summary>

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    CameraZoom cameraZoom;
    [SerializeField]
    MoveJoystick moveJoystick;

    float xRotate, yRotate, xRotateMove, yRotateMove;

    const float pcRotateSpeed = 300.0f;
    const float mobileRotateSpeedOne = 70.0f;
    const float mobileRotateSpeedTwo = 10.0f;
    const float Limit_High = 85f;
    const float Limit_Low = 1.5f;

    void LateUpdate()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE || UNITY_EDITOR_OSX
        if (Input.GetMouseButton(1))
        {
            xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * pcRotateSpeed;
            yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * pcRotateSpeed;

            yRotate = transform.eulerAngles.y + yRotateMove;
            xRotate = xRotate + xRotateMove;
            xRotate = Mathf.Clamp(xRotate, Limit_Low, Limit_High); // 상하 각도 제한

            transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
        }

#elif UNITY_ANDROID || UNITY_IOS
        // 이동 중이라면 터치가 2개일 때 회전
        if(moveJoystick.isMoving && Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            if (secondTouch.phase == TouchPhase.Moved)
            {
                // Calculate the rotation based on the touch movement
                float xRotateMove = -secondTouch.deltaPosition.y * Time.deltaTime * mobileRotateSpeedTwo;
                float yRotateMove = secondTouch.deltaPosition.x * Time.deltaTime * mobileRotateSpeedTwo;

                // Apply the rotation to the camera transform
                float yRotate = transform.eulerAngles.y + yRotateMove;
                float xRotate = transform.eulerAngles.x + xRotateMove;
                xRotate = Mathf.Clamp(xRotate, Limit_Low, Limit_High);

                transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
            }
        }
        // 이동 중 아닐 때
        else if(!moveJoystick.isMoving && !cameraZoom.isZooming)
        {
            // UI 위에 터치가 있다면 리턴 (모바일)
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                if (Input.touchCount == 2)
                    return;

                if (Input.GetMouseButton(0))
                {
                    xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * mobileRotateSpeedOne;
                    yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * mobileRotateSpeedOne;

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
