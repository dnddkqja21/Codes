using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    // 카메라 줌 인 아웃
    float zoomSpeed = 3f;
    float zoomMax = 3f;
    float zoomMin = 15f;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        SetCameraZoom();
    }

    void SetCameraZoom()
    {
        float zoomDir = Input.GetAxis("Mouse ScrollWheel");

        // 카메라의 y로 줌인아웃 한계치 설정
        if (transform.position.y <= zoomMax && zoomDir > 0)
            return;
        if (transform.position.y >= zoomMin && zoomDir < 0)
            return;

        transform.position += transform.forward * zoomDir * zoomSpeed;
    }
}
