using UnityEngine;
using TMPro;

public class LoadGradient : MonoBehaviour
{
    [Header("그린 세팅 창 기울기 텍스트")]
    public TextMeshProUGUI[] gradientTexts;
    [Header("훈련 중 그린 기울기 텍스트")]
    public TextMeshProUGUI[] inProgressGradientTexts;

    public int[] gradient = new int[4] { 0, 0, 0, 0};

    public void LoadGradientText()
    {        
        for (int i = 0; i < gradientTexts.Length; i++)
        {
            GameOption.Instance.gradients[i] = gradient[i];
            GameOption.Instance.originGradients[i] = gradient[i];
            //Debug.Log("불러온 기울기" + GameOption.Instatnce.gradients[i]);
            gradientTexts[i].text = gradient[i].ToString();
            inProgressGradientTexts[i].text = gradient[i].ToString();

            KioskPanelManager.Instance.greenSetting.SetActive(false);
        }
    }             
}
