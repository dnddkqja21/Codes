using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Esc_Button : MonoBehaviour
{
    public Image popup;
    public Image background;


    public void ESC()
    {
        background.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
    }
}
