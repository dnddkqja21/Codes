using UnityEngine;
using TMPro;

public class SetCountButton : MonoBehaviour
{
    [Header("ī��Ʈ �ؽ�Ʈ")]
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
