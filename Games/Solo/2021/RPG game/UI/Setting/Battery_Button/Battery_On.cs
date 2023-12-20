using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery_On : MonoBehaviour
{
    public Image On;
    public Image Off;
    

    public void SwithOff()
    {
        On.gameObject.SetActive(false);
        Off.gameObject.SetActive(true);
    }
}
