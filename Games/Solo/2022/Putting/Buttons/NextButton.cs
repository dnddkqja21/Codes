using UnityEngine;
using UnityEngine.UI;


public class NextButton : MonoBehaviour
{   
    [Header("���")]
    public Toggle[] toggles;    
    [Header("�г�")]
    public GameObject[] panel;

    Button button;

    bool flag;    

    private void Awake()
    {
        button = GetComponent<Button>();
        ActivateBtn();
    }

    public void ActivateBtn()
    {
        foreach(var item in toggles)
        {
            flag = item.isOn;

            if(flag)
            {
                break;
            }
        }
        button.interactable = flag;
    }

    public void ToNextPanel()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                panel[i].SetActive(true);                
            }             
        }
    }
}
