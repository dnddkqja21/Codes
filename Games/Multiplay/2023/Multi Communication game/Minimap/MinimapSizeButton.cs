using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미니맵 사이즈 조절 기능
/// </summary>

public class MinimapSizeButton : MonoBehaviour
{    
    Camera minimapCamera;

    const float MIN_ZOOM = 10f;
    const float MAX_ZOOM = 40f;

    void Awake()
    {
        minimapCamera = GetComponent<Camera>();
    }

    public void ZoomIn()
    {
        minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize -5f, MIN_ZOOM);
    }
    public void ZoomOut()
    {
        minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize + 5f, MAX_ZOOM);
    }
}
