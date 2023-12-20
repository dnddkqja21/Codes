using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMinimap : MonoBehaviour
{
    // ��Ŀ������ ����ϸ� �� �� (������ ������ �޶����� ����)
    [Header("�̴ϸ� ����")]
    public RectTransform minimapBottomLeft;
    public RectTransform minimapCenter;
    public RectTransform playerMinimap;

    [Header("����� ����")]
    public Transform worldBottomLeft;
    public Transform worldCenter;
    public Transform playerWorld;

    float minimapRatio;

    void Start()
    {
        CalculateMapRatio();
    }

    void Update()
    {
        playerMinimap.anchoredPosition = minimapBottomLeft.anchoredPosition + new Vector2((playerWorld.position.x - worldBottomLeft.position.x) *
            minimapRatio, (playerWorld.position.z - worldBottomLeft.position.z) * minimapRatio); 
    }

    private void CalculateMapRatio()
    {
        // ���� �� �Ÿ�, �̴ϸ��� 2d�̹Ƿ� y���� ����
        Vector3 distanceWorldVector = worldBottomLeft.position - worldCenter.position;
        distanceWorldVector.y = 0f;
        
        float distanceWorld = distanceWorldVector.magnitude;

        // �̴ϸ� �Ÿ�
        float distanceMinimap = Mathf.Sqrt( 
            Mathf.Pow((minimapBottomLeft.anchoredPosition.x - minimapCenter.anchoredPosition.x), 2) + 
            Mathf.Pow((minimapBottomLeft.anchoredPosition.y - minimapCenter.anchoredPosition.y), 2));

        minimapRatio = distanceMinimap / distanceWorld;
    }
}
