using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMinimap : MonoBehaviour
{
    // 앵커프리셋 사용하면 안 됨 (포지션 기준이 달라지기 때문)
    [Header("미니맵 세팅")]
    public RectTransform minimapBottomLeft;
    public RectTransform minimapCenter;
    public RectTransform playerMinimap;

    [Header("월드맵 세팅")]
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
        // 월드 맵 거리, 미니맵은 2d이므로 y축은 무시
        Vector3 distanceWorldVector = worldBottomLeft.position - worldCenter.position;
        distanceWorldVector.y = 0f;
        
        float distanceWorld = distanceWorldVector.magnitude;

        // 미니맵 거리
        float distanceMinimap = Mathf.Sqrt( 
            Mathf.Pow((minimapBottomLeft.anchoredPosition.x - minimapCenter.anchoredPosition.x), 2) + 
            Mathf.Pow((minimapBottomLeft.anchoredPosition.y - minimapCenter.anchoredPosition.y), 2));

        minimapRatio = distanceMinimap / distanceWorld;
    }
}
