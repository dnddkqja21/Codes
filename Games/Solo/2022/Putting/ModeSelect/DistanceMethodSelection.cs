using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMethodSelection : MonoBehaviour
{
    // 거리 연습 방식
    [Header("거리 훈련 방식")]
    public DistanceMethod distanceMethod;

    public void OnDistanceMethodSelection()
    {
        GameOption.Instance.distanceMethod = (int)distanceMethod;
    }
}
