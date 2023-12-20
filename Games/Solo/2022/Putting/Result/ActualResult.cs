using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActualResult : MonoBehaviour
{
    [Header("컵 포인트")]
    public TextMeshProUGUI actualCupPoint;
    [Header("시작 포인트")]
    public TextMeshProUGUI actualStartPoint;
    [Header("기울기")]
    public TextMeshProUGUI actualGradient;
    [Header("훈련 수")]
    public TextMeshProUGUI tranningCount;
    [Header("성공 수")]
    public TextMeshProUGUI successCount;
    [Header("성공률")]
    public TextMeshProUGUI successRate;
}
