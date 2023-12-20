using UnityEngine;
using TMPro;

public class ResetGradient : MonoBehaviour
{
    [Header("버튼 셋")]
    public GameObject[] buttonSets;
    [Header("기울기 텍스트")]
    public TextMeshProUGUI[] gradientTexts;
    [Header("훈련 중 그린 기울기 텍스트")]
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
