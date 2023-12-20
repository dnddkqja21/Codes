using UnityEngine;
using UnityEngine.UI;

public class SelectUIAction : MonoBehaviour
{
    [Header("선택 오브젝트")]
    public GameObject[] selectedUnits;
    [Header("미선택 오브젝트")]
    public GameObject[] unselectedUnits;

    Toggle toggle;

    bool isOn;

    private void Start()
    {
        toggle = GetComponent<Toggle>();

        if (toggle.isOn)
        {
            toggle.Select();
            OnSelectToggle();
        }
    }

    public void OnSelectToggle()
    {     
        isOn = toggle.isOn;        

        for (int i = 0; i < selectedUnits.Length; i++)
        {
            selectedUnits[i].SetActive(isOn);            
        }
        for (int i = 0; i < unselectedUnits.Length; i++)
        {
            unselectedUnits[i].SetActive(!isOn);
        }
    } 
}
