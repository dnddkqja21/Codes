using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 조이스틱 영역을 구분
/// </summary>

public class JoysticArea : MonoBehaviour
{
    [Header("캔버스 스케일러")]
    [SerializeField]
    CanvasScaler scaler;

    [Header("캔버스 렉트 트랜스폼")]
    [SerializeField]
    RectTransform canvasRect;

    RectTransform joystickArea;

    void Awake()
    {
        joystickArea = GetComponent<RectTransform>();
    }

    void Start()
    {
        //Debug.Log("Screen.width : " + Screen.width);
        //Debug.Log("Screen.height : " + Screen.height);

        float widthRatio = Screen.width / scaler.referenceResolution.x;
        float heightRatio = Screen.height / scaler.referenceResolution.y;

        // 결과 비율 값
        float ratio = widthRatio * (1f - scaler.matchWidthOrHeight) + heightRatio * (scaler.matchWidthOrHeight);

        // 현재 스크린에서 렉트트랜스폼의 실제 너비와 높이
        float pixelWidth = canvasRect.rect.width * ratio;
        float pixelHeight = canvasRect.rect.height * ratio;

        // 조이스틱의 영역 재설정
        //joystickArea.sizeDelta = new Vector2(pixelWidth * 0.5f, pixelHeight);    
        //joystickArea.sizeDelta = new Vector2(Screen.width * 0.25f, Screen.height * 0.5f);
        joystickArea.sizeDelta = new Vector2(pixelWidth * 0.25f, pixelHeight * 0.5f);
    }
}
