using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 건물 이름 UI가 항상 카메라를 보도록 함
/// </summary>

public class NameLookAtCamera : MonoBehaviour
{
    // 차후 여기서 로컬라이징, 
    // 고려해볼 사항 : 건물 이름 가져와서 tmp에 띄우기
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
