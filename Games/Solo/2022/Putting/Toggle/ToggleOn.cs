using UnityEngine;
using UnityEngine.UI;

public class ToggleOn : MonoBehaviour
{
    [Header("≈‰±€")]
    public Toggle toggle;
    void Start()
    {
        if(toggle.isOn)
        {
            toggle.Select();
        }
    }    
}
