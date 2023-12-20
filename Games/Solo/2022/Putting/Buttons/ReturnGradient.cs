using UnityEngine;
using TMPro;

public class ReturnGradient : MonoBehaviour
{
    [Header("���� �ؽ�Ʈ")]
    public TextMeshProUGUI[] gradientTexts;
    public void SetReturnGradients()
    {
        for (int i = 0; i < GameOption.Instance.gradients.Length; i++)
        {
            GameOption.Instance.gradients[i] = GameOption.Instance.originGradients[i];
            gradientTexts[i].text = GameOption.Instance.gradients[i].ToString();
        }
    }
}
