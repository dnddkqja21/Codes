using UnityEngine;
using TMPro;

public class DisplayGradient : MonoBehaviour
{
    [Header("���� �ؽ�Ʈ")]
    public TextMeshProUGUI[] gradientTexts;

    private void OnEnable()
    {
        for (int i = 0; i < gradientTexts.Length; i++)
        {
            gradientTexts[i].text = GameOption.Instance.gradients[i].ToString();
        }
    }
}
