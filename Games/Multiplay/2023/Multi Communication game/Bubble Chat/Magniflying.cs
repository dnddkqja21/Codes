using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버블 오브젝트 확대 효과
/// </summary>

public class Magniflying : MonoBehaviour
{
    Renderer renderers;
    Camera cam;

    void Start()
    {
        renderers = GetComponent<Renderer>();
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(transform.position);
        screenPoint.x = screenPoint.x / Screen.width;
        screenPoint.y = screenPoint.y / Screen.height;
        renderers.material.SetVector("_ObjScreenPos", screenPoint);
    }
}
