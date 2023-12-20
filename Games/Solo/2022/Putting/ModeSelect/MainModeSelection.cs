using UnityEngine;
using UnityEngine.UI;

public class MainModeSelection : MonoBehaviour
{
    [Header("�Ʒ� ���")]
    public GameMode gameMode;    

    Toggle toggle;

    bool isOn;  

    private void Start()
    {
        toggle = GetComponent<Toggle>();  
    }

    public void OnModeSelection()
    {
        if (!isOn)
        {
            GameOption.Instance.selectedMode = (int)GameMode.NONE;
        }
        GameOption.Instance.selectedMode = (int)gameMode;

        isOn = toggle.isOn;        
    }
}
