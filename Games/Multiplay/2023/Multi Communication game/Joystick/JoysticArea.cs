using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���̽�ƽ ������ ����
/// </summary>

public class JoysticArea : MonoBehaviour
{
    [Header("ĵ���� �����Ϸ�")]
    [SerializeField]
    CanvasScaler scaler;

    [Header("ĵ���� ��Ʈ Ʈ������")]
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

        // ��� ���� ��
        float ratio = widthRatio * (1f - scaler.matchWidthOrHeight) + heightRatio * (scaler.matchWidthOrHeight);

        // ���� ��ũ������ ��ƮƮ�������� ���� �ʺ�� ����
        float pixelWidth = canvasRect.rect.width * ratio;
        float pixelHeight = canvasRect.rect.height * ratio;

        // ���̽�ƽ�� ���� �缳��
        //joystickArea.sizeDelta = new Vector2(pixelWidth * 0.5f, pixelHeight);    
        //joystickArea.sizeDelta = new Vector2(Screen.width * 0.25f, Screen.height * 0.5f);
        joystickArea.sizeDelta = new Vector2(pixelWidth * 0.25f, pixelHeight * 0.5f);
    }
}
