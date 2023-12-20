using UnityEngine;
using TMPro;

public class TranningCount : MonoBehaviour
{
    [Header("�Ʒ� Ƚ��")]
    public TextMeshProUGUI maxCount;
    
    private void OnEnable()
    {
        maxCount.text = GameOption.Instance.TranningCount.ToString();
    }
}
