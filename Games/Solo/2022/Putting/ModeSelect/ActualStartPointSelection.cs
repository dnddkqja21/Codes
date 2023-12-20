using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActualStartPointSelection : MonoBehaviour
{
    // 실전 시작 지점
    [Header("실전 시작 지점")]
    public ActualStartPoint actualStartPoint;
    [Header("시작 지점 텍스트")]
    public TextMeshProUGUI start;

    private void Start()
    {
        start.text = EnumToData.Instance.ActualStartToKor((int)actualStartPoint);
    }

    public void OnActualStartPointSelection()
    {
        GameOption.Instance.actualStartPoint = (int)actualStartPoint;
    }
}
