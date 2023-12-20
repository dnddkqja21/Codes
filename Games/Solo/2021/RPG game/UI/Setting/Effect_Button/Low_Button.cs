using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Low_Button : MonoBehaviour
{
    public Image Low;
    public Image Middle;
    public Image High;
    
   
    public void SwitchOn()
    {
        Low.gameObject.SetActive(true);
        Middle.gameObject.SetActive(false);
        High.gameObject.SetActive(false);
    }
}
