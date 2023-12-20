using UnityEngine;
using TMPro;

public class ResetGradient : MonoBehaviour
{
    [Header("��ư ��")]
    public GameObject[] buttonSets;
    [Header("���� �ؽ�Ʈ")]
    public TextMeshProUGUI[] gradientTexts;
    [Header("�Ʒ� �� �׸� ���� �ؽ�Ʈ")]
    public TextMeshProUGUI[] inProgressGradientTexts;

    public void InitializeGradient()
    {
        for (int i = 0; i < GameOption.Instance.gradients.Length; i++)
        {
            GameOption.Instance.gradients[i] = 0;
            GameOption.Instance.originGradients[i] = 0;
            buttonSets[i].SetActive(false);
            gradientTexts[i].text = "0";
            inProgressGradientTexts[i].text = "0";
        }
    }
}
