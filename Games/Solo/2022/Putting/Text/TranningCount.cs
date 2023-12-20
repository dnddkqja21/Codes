using UnityEngine;
using TMPro;

public class TranningCount : MonoBehaviour
{
    [Header("ÈÆ·Ã È½¼ö")]
    public TextMeshProUGUI maxCount;
    
    private void OnEnable()
    {
        maxCount.text = GameOption.Instance.TranningCount.ToString();
    }
}
