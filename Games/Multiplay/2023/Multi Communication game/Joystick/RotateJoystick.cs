using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라 회전 조이스틱
/// </summary>

public class RotateJoystick : MonoBehaviour
{
    [Header("카메라 암")]
    [SerializeField]
    Transform cameraArm;

    public bool isRotate;

    DynamicJoystick joystick;

    float rotationSpeed = 25f;
    const float Limit_High = 85f;
    const float Limit_Low = 1.5f;

    float currentRotationX = 0f;
    float currentRotationY = 0f;

    void Start()
    {
        joystick = GetComponent<DynamicJoystick>();
    }

    void LateUpdate()
    {
#if UNITY_ANDROID || UNITY_IOS
        Vector2 direction = joystick.Direction;

        if(direction == Vector2.zero )
        {
            //Debug.Log("디렉션 제로, isRotate : " + isRotate);
            isRotate = false;
        }

        if (direction != Vector2.zero)
        {
            isRotate = true;
            // Calculate the rotation angles based on the joystick direction
            float angleX = direction.y * rotationSpeed * Time.deltaTime;
            float angleY = direction.x * rotationSpeed * Time.deltaTime;

            // Update the current rotation angles
            currentRotationX -= angleX;
            currentRotationY += angleY;

            // Clamp the vertical rotation angle
            currentRotationX = Mathf.Clamp(currentRotationX, Limit_Low, Limit_High);

            // Apply the rotation to the camera
            cameraArm.transform.rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        }
#endif
    }
}
