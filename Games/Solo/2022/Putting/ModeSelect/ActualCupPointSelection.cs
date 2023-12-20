using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActualCupPointSelection : MonoBehaviour
{
    // ���� �� ��ġ 
    [Header("���� �� ��ġ")]
    public ActualCupPoint actualCupPoint;
    [Header("�� �ؽ�Ʈ")]
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
