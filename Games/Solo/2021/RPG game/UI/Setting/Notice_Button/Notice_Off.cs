using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice_Off : MonoBehaviour
{
    public Image On;
    public Image Off;
   

    public void SwitchOn()
    {
        On.gameObject.SetActive(true);
        Off.gameObject.SetActive(false);
    }
}
