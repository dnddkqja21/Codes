using UnityEngine;
using UnityEngine.UI;

public class ActiveLoadGradientButton : MonoBehaviour
{
    Button button;
    Toggle toggle;
    bool flag;
    void Start()
    {
        button = GameObject.Find("Load Gradient Button").GetComponent<Button>();
        toggle = GetComponent<Toggle>();
    }

    public void ActivateBtn()
    {   
        flag = toggle.isOn;   
        button.interactable = flag;
    }
}
