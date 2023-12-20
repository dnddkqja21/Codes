using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActualGradientSelection : MonoBehaviour
{
    // ���� ���� ����
    [Header("���� ����")]
    public ActualGradient actualGradient;
    [Header("���� �ؽ�Ʈ")]
    public TextMeshProUGUI gradient;

    private void Start()
    {
        gradient.text = EnumToData.Instance.ActualGradientToKor((int)actualGradient);
    }

    public void OnActualGradientSelection()
    {
        GameOption.Instance.actualGradient = (int)actualGradient;
    }
}
