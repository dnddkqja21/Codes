using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Exit_Button : MonoBehaviour
{
    public Image ExitSettingWindow;
    

    public void OffWindow()
    {
        ExitSettingWindow.gameObject.SetActive(false);
    }
}
