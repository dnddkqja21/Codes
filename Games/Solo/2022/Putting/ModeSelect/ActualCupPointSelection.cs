using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActualCupPointSelection : MonoBehaviour
{
    // 실전 컵 위치 
    [Header("실전 컵 위치")]
    public ActualCupPoint actualCupPoint;
    [Header("컵 텍스트")]
    public TextMeshProUGUI cup;

    private void Start()
    {
        cup.text = EnumToData.Instance.ActualCupToKor((int)actualCupPoint);
    }

    public void OnActualCupPointSelection()
    {
        GameOption.Instance.actualCupPoint = (int)actualCupPoint;
    }
}
