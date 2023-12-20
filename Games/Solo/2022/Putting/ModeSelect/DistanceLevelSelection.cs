using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceLevelSelection : MonoBehaviour
{
    [Header("거리 훈련 단계")]
    public DistanceLevel distanceLevel;

    // 거리 훈련 난이도
    public void OnDistanceLevelSelection()
    {
        GameOption.Instance.distanceLevel = (int)distanceLevel;
    }
}
