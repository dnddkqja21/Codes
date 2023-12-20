using UnityEngine;
using TMPro;

public class SetGradient : MonoBehaviour
{    
    [Header("기울기 텍스트")]
    public TextMeshProUGUI gradientText;
    [Header("인덱스(0 ~ 3)")]
    public int gradientIndex;

    // 기울기 최소, 최대값
    public int Min = -9;
    public int Max = 9;
    public void GradientUp()
    {
        if (GameOption.Instance.gradients[gradientIndex] >= Max)
        {
            return;
        }
            
        GameOption.Instance.gradients[gradientIndex] += 1;
        gradientText.text = GameOption.Instance.gradients[gradientIndex].ToString();
    }

    public void GradientDown()
    {
        if (GameOption.Instance.gradients[gradientIndex] <= Min)
        {
            return;
        }
            
        GameOption.Instance.gradients[gradientIndex] -= 1;
        gradientText.text = GameOption.Instance.gradients[gradientIndex].ToString();
    }
}
