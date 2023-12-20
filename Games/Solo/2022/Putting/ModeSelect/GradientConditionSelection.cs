using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GradientConditionSelection : MonoBehaviour
{
    // ���� �Ʒ� ȯ��
    [Header("���� �Ʒ� ȯ��")]
    public GradientCondition gradientCondition;
    [Header("���� ȯ�� �ؽ�Ʈ")]
    public TextMeshProUGUI levelTitle;

    private void Start()
    {
        levelTitle.text = EnumToData.Instance.GradientTitleToKor((int)gradientCondition);
    }

    public void OnGradientConditionSelection()
    {
        GameOption.Instance.gradientCondition = (int)gradientCondition;
        //Debug.Log(GameOption.Instance.gradientCondition);
    }
}
