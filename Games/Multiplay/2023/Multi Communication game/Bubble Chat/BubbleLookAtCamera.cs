using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버블 오브젝트가 항상 카메라를 보도록 함/// 
/// </summary>

public class BubbleLookAtCamera : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.forward = mainCamera.transform.position - transform.position;
    }
}
