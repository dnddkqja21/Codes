using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Quit_Button : MonoBehaviour
{
    public Image popup;
    public Image background;
  

    public void QuitButtonOn()
    {
        background.gameObject.SetActive(true);
        popup.gameObject.SetActive(true);
    }
}
