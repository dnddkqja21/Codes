using UnityEngine;
using TMPro;

public class LoadGradient : MonoBehaviour
{
    [Header("�׸� ���� â ���� �ؽ�Ʈ")]
    public TextMeshProUGUI[] gradientTexts;
    [Header("�Ʒ� �� �׸� ���� �ؽ�Ʈ")]
    public TextMeshProUGUI[] inProgressGradientTexts;

    public int[] gradient = new int[4] { 0, 0, 0, 0};

    public void LoadGradientText()
    {        
        for (int i = 0; i < gradientTexts.Length; i++)
        {
            GameOption.Instance.gradients[i] = gradient[i];
            GameOption.Instance.originGradients[i] = gradient[i];
            //Debug.Log("�ҷ��� ����" + GameOption.Instatnce.gradients[i]);
            gradientTexts[i].text = gradient[i].ToString();
            inProgressGradientTexts[i].text = gradient[i].ToString();

            KioskPanelManager.Instance.greenSetting.SetActive(false);
        }
    }             
}
