using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    // ī�޶� �� �� �ƿ�
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

        // ī�޶��� y�� ���ξƿ� �Ѱ�ġ ����
        if (transform.position.y <= zoomMax && zoomDir > 0)
            return;
        if (transform.position.y >= zoomMin && zoomDir < 0)
            return;

        transform.position += transform.forward * zoomDir * zoomSpeed;
    }
}
