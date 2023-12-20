using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class High_Button : MonoBehaviour
{
    public Image Low;
    public Image Middle;
    public Image High;

   
    public void SwitchOn()
    {
        Low.gameObject.SetActive(false);
        Middle.gameObject.SetActive(false);
        High.gameObject.SetActive(true);
    }
}
