using UnityEngine;
using TMPro;

public class SetCountButton : MonoBehaviour
{
    [Header("카운트 텍스트")]
    public TextMeshProUGUI countText; 

    public void PlusButton()
    {     
        GameOption.Instance.TranningCount += 5;        
        countText.text = GameOption.Instance.TranningCount.ToString();
    }

    public void MinusButton()
    {
        GameOption.Instance.TranningCount -= 5; 
        countText.text = GameOption.Instance.TranningCount.ToString();
    }

}
