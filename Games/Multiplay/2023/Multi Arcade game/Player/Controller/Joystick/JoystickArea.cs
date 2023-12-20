using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 조이스틱의 영역을 화면 좌 하단 구석으로 설정
/// </summary>

public class JoystickArea : MonoBehaviour
{
    RectTransform canvas;
    CanvasScaler canvasScaler;
    RectTransform joystickArea;

    void Start()
    {
        canvas = UIManagerWorld.Instance.canvas;
        canvasScaler = canvas.GetComponent<CanvasScaler>();
        joystickArea = GetComponent<RectTransform>();

        SetJoystickArea();
    }

    void SetJoystickArea()
    {
        float widthRatio = Screen.width / canvasScaler.referenceResolution.x;
        float heightRatio = Screen.height / canvasScaler.referenceResolution.y;

        // 결과 비율 값
        float ratio = widthRatio * (1f - canvasScaler.matchWidthOrHeight) + heightRatio * (canvasScaler.matchWidthOrHeight);

        // 현재 스크린에서 렉트트랜스폼의 실제 너비와 높이
        float pixelWidth = canvas.rect.width * ratio;
        float pixelHeight = canvas.rect.height * ratio;

        joystickArea.sizeDelta = new Vector2(pixelWidth * 0.25f, pixelHeight * 0.5f);
    }
}
